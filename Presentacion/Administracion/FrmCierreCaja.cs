using CarWash.Database;
using CarWash.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarWash.Presentacion.Administracion
{
    public partial class FrmCierreCaja : Form
    {
        private decimal totalEnCaja = 0;
        private decimal totalValesOperadores = 0;
        private decimal diferencia = 0;
        private List<CajaDiaria> cajaDiaria = new List<CajaDiaria>();
        public FrmCierreCaja()
        {
            InitializeComponent();
            btnCerrarCaja.Click += BtnCerrarCaja_Click;
            txtValorEfectivo.Text = "0";
            txtValorTransferencia.Text = "0";
            txtValorDatafono.Text = "0";
            txtValorCredito.Text = "0";
            txtValorGastos.Text = "0";
            txtValorVales.Text = "0";
        }



        private void FrmCierreCaja_Load(object sender, EventArgs e)
        {
            using (var ms = new System.IO.MemoryStream(Properties.Resources.splash_car_icon))
            {
                this.Icon = new Icon(ms);
            }
            cargaCaja();
            cargaVales();
            CargarDatosCaja();
        }


        private void CargarDatosCaja()
        {
            try
            {
                var totalIgresosmasCaja = cajaDiaria[0].MontoInicial + cajaDiaria[0].TotalFinal;
                var valorDescuentos = cajaDiaria[0].TotalEgresos + cajaDiaria[0].TotalIngresosCredito + cajaDiaria[0].TotalIngresosTransferencias + totalValesOperadores;


                var totalEnCaja = totalIgresosmasCaja - valorDescuentos;


                // Asignar valores a labels
                lblMontoInicialValor.Text = cajaDiaria[0].MontoInicial.ToString("N2");
                lblIngresosValor.Text = cajaDiaria[0].TotalFinal.ToString("N2");
                lblGastosValor.Text = cajaDiaria[0].TotalEgresos.ToString("N2");
                lblTotalCajaValor.Text = totalEnCaja.ToString("N2");
                lblIngresosEfectivo.Text = cajaDiaria[0].TotalIngresosEfectivo.ToString("N2");
                lblIngresosTransferencia.Text = cajaDiaria[0].TotalIngresosTransferencias.ToString("N2");
                lblIngresosDatafono.Text = cajaDiaria[0].TotalIngresosDatafono.ToString("N2");
                lblVentasCredito.Text = cajaDiaria[0].TotalIngresosCredito.ToString("N2");
                calculaDiferencia();


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando datos de caja: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void cargaCaja()
        {
            cajaDiaria = DatabaseQueryLDB.ExecuteList<CajaDiaria>(
               @"SELECT idCaja,FechaApertura,FechaCierre,MontoInicial,TotalIngresosCredito,TotalIngresosEfectivo,TotalIngresosTransferencias,TotalIngresosDatafono,TotalEgresos,TotalFinal,Estado
                  FROM CajaDiaria
                  WHERE  Estado = 1;");
        }


        private void cargaVales()
        {
            var vales = DatabaseQueryLDB.ExecuteList<ValesOperarios>(
               @"SELECT IdValeOperario,idOperario,idCaja,FechaRegsitro,Valor,Motivo,Descontado,FechaDescuento
                 FROM ValesOperarios
                  WHERE  idCaja = ?;", cajaDiaria[0].idCaja);

            foreach (var v in vales)
            {
                totalValesOperadores += v.Valor;
            }

            lblTotalValesOperarios.Text = totalValesOperadores.ToString("N2");
        }

        private void calculaDiferencia()
        {
            if (diferencia < 0)
            {
                lblDiferenciaValor.ForeColor = Color.Red;
                lblDiferenciaValor.Text = diferencia.ToString("N2");
            }
            else
            {
                lblDiferenciaValor.ForeColor = Color.FromArgb(40, 167, 69);
                lblDiferenciaValor.Text = diferencia.ToString("N2");
            }
        }


        private decimal ObtenerMontoFisicoEnCaja()
        {
            // Aquí podrías abrir un InputBox para que el cajero ingrese el efectivo
            // Por ahora asumimos que coincide con el totalEnCaja
            return totalEnCaja;
        }



        private void CerrarCaja()
        {
            try
            {
                // Actualiza el registro de caja
                DatabaseQueryLDB.ExecuteNonQuery(
                    @"UPDATE CajaDiaria SET 
                        Estado = 0, 
                        FechaCierre = ?
                      WHERE idCaja = ?",
                    DateTime.Now, cajaDiaria[0].idCaja
                );

                MessageBox.Show("Caja cerrada exitosamente.", "Cierre Caja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cerrando la caja: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCerrarVentana_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnCerrarCaja_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Desea cerrar la caja y registrar los totales?",
                "Confirmar Cierre", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                CerrarCaja();
            }
        }

        private void txtValorEfectivo_KeyDown(object sender, KeyEventArgs e)
        {
            // Detecta Ctrl+V (pegar)
            if (e.Control && e.KeyCode == Keys.V)
            {
                e.SuppressKeyPress = true; // Bloquea el pegado
            }
        }

        private void txtValorEfectivo_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = sender as TextBox;

            // Permitir teclas de control (backspace, delete, tab, etc.)
            if (char.IsControl(e.KeyChar))
                return;

            // Permitir solo dígitos
            if (char.IsDigit(e.KeyChar))
                return;

            // Permitir un solo punto decimal
            if (e.KeyChar == '.' && !tb.Text.Contains("."))
                return;

            // Si llega aquí, se bloquea la tecla
            e.Handled = true;
        }

        private void txtValorTransferencia_KeyDown(object sender, KeyEventArgs e)
        {
            // Detecta Ctrl+V (pegar)
            if (e.Control && e.KeyCode == Keys.V)
            {
                e.SuppressKeyPress = true; // Bloquea el pegado
            }
        }

        private void txtValorTransferencia_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = sender as TextBox;

            // Permitir teclas de control (backspace, delete, tab, etc.)
            if (char.IsControl(e.KeyChar))
                return;

            // Permitir solo dígitos
            if (char.IsDigit(e.KeyChar))
                return;

            // Permitir un solo punto decimal
            if (e.KeyChar == '.' && !tb.Text.Contains("."))
                return;

            // Si llega aquí, se bloquea la tecla
            e.Handled = true;
        }

        private void txtValorDatafono_KeyDown(object sender, KeyEventArgs e)
        {
            // Detecta Ctrl+V (pegar)
            if (e.Control && e.KeyCode == Keys.V)
            {
                e.SuppressKeyPress = true; // Bloquea el pegado
            }
        }

        private void txtValorDatafono_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = sender as TextBox;

            // Permitir teclas de control (backspace, delete, tab, etc.)
            if (char.IsControl(e.KeyChar))
                return;

            // Permitir solo dígitos
            if (char.IsDigit(e.KeyChar))
                return;

            // Permitir un solo punto decimal
            if (e.KeyChar == '.' && !tb.Text.Contains("."))
                return;

            // Si llega aquí, se bloquea la tecla
            e.Handled = true;
        }

        private void btnRecalcular_Click(object sender, EventArgs e)
        {
            decimal valorIngresos = 0;

            valorIngresos = Convert.ToDecimal(txtValorDatafono.Text) +
                            Convert.ToDecimal(txtValorEfectivo.Text) +
                            Convert.ToDecimal(txtValorTransferencia.Text) +
                            Convert.ToDecimal(txtValorCredito.Text) +
                            Convert.ToDecimal(txtValorGastos.Text) +
                            Convert.ToDecimal(txtValorVales.Text);



            diferencia = valorIngresos - (cajaDiaria[0].TotalFinal + cajaDiaria[0].MontoInicial);
            calculaDiferencia();
        }



        private void TextBox_SelectAll(object sender, EventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void txtValorEfectivo_MouseClick(object sender, MouseEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void txtValorTransferencia_MouseClick(object sender, MouseEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void txtValorDatafono_MouseClick(object sender, MouseEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void txtValorCredito_MouseClick(object sender, MouseEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void txtValorGastos_MouseClick(object sender, MouseEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void txtValorVales_MouseClick(object sender, MouseEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }
    }
}
