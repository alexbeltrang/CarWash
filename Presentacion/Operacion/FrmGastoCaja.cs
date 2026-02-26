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

namespace CarWash.Presentacion.Operacion
{
    public partial class FrmGastoCaja : Form
    {
        CajaDiaria cajaDiaria = new CajaDiaria();
        decimal valorGastoCaja = 0;
        public FrmGastoCaja()
        {
            InitializeComponent();
        }

        private void FrmGastoCaja_Load(object sender, EventArgs e)
        {

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (validaCampos())
            {

                DatabaseQueryLDB.ExecuteNonQuery(
                        "INSERT INTO GastosCaja (idCaja,FechaRegistro,Concepto,Valor,Observaciones) values (?,?,?,?,?)",
                        cajaDiaria.idCaja,
                        DateTime.Now,
                        txtConcepto.Text,
                        Convert.ToDecimal(txtValor.Text),
                        txtObservacion.Text);

                valorGastoCaja = valorGastoCaja + Convert.ToDecimal(txtValor.Text);

                DatabaseQueryLDB.ExecuteNonQuery(
                       "UPDATE CajaDiaria SET TotalEgresos = ? WHERE idCaja = ?",
                       valorGastoCaja, cajaDiaria.idCaja
                       );

                MostrarToast("Gasto registrado correctamente", Color.FromArgb(40, 167, 69));
                cargaCajaDiaria();
                limpiacampos();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            limpiacampos();
        }

        private void limpiacampos()
        {
            txtConcepto.Text = string.Empty;
            txtValor.Text = string.Empty;
            txtObservacion.Text = string.Empty;
        }

        private void txtValor_Enter(object sender, EventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void txtValor_KeyDown(object sender, KeyEventArgs e)
        {
            // Detecta Ctrl+V (pegar)
            if (e.Control && e.KeyCode == Keys.V)
            {
                e.SuppressKeyPress = true; // Bloquea el pegado
            }
        }

        private void txtValor_KeyPress(object sender, KeyPressEventArgs e)
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


        private bool validaCampos()
        {
            cargaCajaDiaria();
            if (String.IsNullOrEmpty(txtConcepto.Text))
            {
                MostrarToast("Ingrese el concepto del Gasto", Color.FromArgb(220, 53, 69));
                return false;
            }
            else if (string.IsNullOrEmpty(txtValor.Text))
            {
                MostrarToast("Ingrese el valor del vale.", Color.FromArgb(220, 53, 69));
                return false;
            }
            else if (cajaDiaria == null)
            {
                MostrarToast("No existe caja abierta para el día de hoy.", Color.FromArgb(220, 53, 69));
                return false;
            }
            else
            {
                return true;
            }
        }

        private void MostrarToast(string mensaje, Color color)
        {
            Panel toast = new Panel();
            toast.Size = new Size(300, 50);
            toast.BackColor = color;
            toast.Left = this.Width - 320;
            toast.Top = 20;

            Label lbl = new Label();
            lbl.Text = mensaje;
            lbl.ForeColor = Color.White;
            lbl.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lbl.AutoSize = false;
            lbl.Dock = DockStyle.Fill;
            lbl.TextAlign = ContentAlignment.MiddleCenter;

            toast.Controls.Add(lbl);
            this.Controls.Add(toast);
            toast.BringToFront();

            Timer timer = new Timer();
            timer.Interval = 3000;
            timer.Tick += (s, e) =>
            {
                this.Controls.Remove(toast);
                timer.Stop();
            };
            timer.Start();
        }

        private void cargaCajaDiaria()
        {
            cajaDiaria = DatabaseQueryLDB.ExecuteList<CajaDiaria>(
                @"SELECT idCaja,FechaApertura,FechaCierre,MontoInicial,TotalIngresosEfectivo,TotalIngresosTransferencias,TotalIngresosDatafono,TotalEgresos,TotalFinal,Estado
                  FROM CajaDiaria
                  WHERE strftime('%Y-%m-%d',FechaApertura / 10000000 - 62135596800,'unixepoch') = date('now')  and Estado = 1;").FirstOrDefault();


            valorGastoCaja = cajaDiaria.TotalEgresos;
        }
    }
}
