using CarWash.Database;
using CarWash.DTOs;
using CarWash.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace CarWash.Presentacion.Operacion
{
    public partial class FrmGestionDetalleVehiculo : Form
    {
        private int _idTurno;
        decimal valorPagar = 0;
        List<GestionVehiculosDTO> vehiculosProceso = new List<GestionVehiculosDTO>();
        private List<CajaDiaria> cajaDiaria = new List<CajaDiaria>();
        public FrmGestionDetalleVehiculo(int idTurno)
        {
            InitializeComponent();
            _idTurno = idTurno;
        }

        private void FrmGestionDetalleVehiculo_Load(object sender, EventArgs e)
        {

            this.Region = System.Drawing.Region.FromHrgn(
            CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            cargarDetalleTurnoVehiculo(_idTurno);
            cargaCajaDiaria();
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
            (
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
            );

        private void FrmGestionDetalleVehiculo_FormClosed(object sender, FormClosedEventArgs e)
        {
            cerrarForm();
        }

        private void cargaCajaDiaria()
        {
            try
            {
                cajaDiaria = DatabaseQueryLDB.ExecuteList<CajaDiaria>(
                @"SELECT idCaja,FechaApertura,FechaCierre,MontoInicial,TotalIngresosEfectivo,TotalIngresosTransferencias,TotalIngresosDatafono,TotalEgresos,TotalFinal,Estado
                  FROM CajaDiaria
                  WHERE  strftime('%Y-%m-%d',FechaApertura / 10000000 - 62135596800,'unixepoch') = date('now');");
                if (cajaDiaria == null)
                {
                    MessageBox.Show("No hay apertura de caja para el día de hoy.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando Cajas Diarias: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cerrarForm()
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void cargarDetalleTurnoVehiculo(int idTurno)
        {
            try
            {
                vehiculosProceso = DatabaseQueryLDB.ExecuteList<GestionVehiculosDTO>(
               @"SELECT TUR.IdTurno,TUR.Placa,TUR.NombreCliente,TUR.NumeroCelular, TUR.NumeroTurno, strftime('%Y-%m-%d %H:%M',TUR.FechaHoraIngreso / 10000000 - 62135596800,'unixepoch') AS FechaHoraIngreso, TVH.Nombre TipoVehiculo, SER.Nombre Servicio,  COALESCE(OPE.Nombres, '') || ' ' || COALESCE(OPE.Apellidos, '') AS OperadorAsignado, TUR.Valor, TUR.idOperario, TUR.Observaciones   
                  FROM Turnos TUR INNER JOIN TipoVehiculo TVH ON TUR.IdTipoVehiculo = TVH.idTipoVehiculo 
                  INNER JOIN Servicios SER ON SER.idServicio = TUR.idServicio 
                  LEFT OUTER JOIN Operarios OPE ON TUR.idOperario = OPE.idOperario 
                  WHERE TUR.IdTurno = ? ", idTurno);

                if (vehiculosProceso != null && vehiculosProceso.Count > 0)
                {
                    lblTurno.Text = "Turno N°: " + vehiculosProceso[0].NumeroTurno;
                    lblPlaca.Text = "Placa: " + vehiculosProceso[0].Placa;
                    lblCliente.Text = "Cliente: " + vehiculosProceso[0].NombreCliente;
                    lblCelular.Text = "Número Celular: " + vehiculosProceso[0].NumeroCelular;
                    lblFechaIngreso.Text = "Fecha Hora Ingreso: " + vehiculosProceso[0].FechaHoraIngreso;
                    lblTipoVehiculo.Text = "Tipo Vehículo: " + vehiculosProceso[0].TipoVehiculo;
                    lblServicio.Text = "Servicio Atendido: " + vehiculosProceso[0].Servicio;
                    txtValorPagar.Text = vehiculosProceso[0].Valor.ToString("N2");
                    valorPagar = vehiculosProceso[0].Valor;
                    txtObservaciones.Text = vehiculosProceso[0].Observaciones;

                    if (vehiculosProceso[0].idOperario.HasValue)
                    {
                        lblOperadorAsignado.Text = "Operador Asignado: " + vehiculosProceso[0].OperadorAsignado;
                    }
                    else if (!vehiculosProceso[0].idOperario.HasValue)
                    {
                        lblEstado.Text = "Estado: Sin Operador Asignado";
                        lblOperadorAsignado.Text = "Operador Asignado: SIN ASIGNAR";
                        lblEstado.ForeColor = Color.White;
                        lblEstado.Location = new Point(500, 20);
                    }
                    else if (!vehiculosProceso[0].Estado)
                    {
                        lblEstado.Text = "Estado: En Proceso";
                        lblEstado.ForeColor = Color.White;
                        lblEstado.Location = new Point(627, 20);
                    }
                    else
                    {
                        lblEstado.Text = "Estado: Finalizado";
                        lblEstado.ForeColor = Color.Green;
                        lblEstado.Location = new Point(627, 20);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando vehículos en proceso: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAsignarOperador_Click(object sender, EventArgs e)
        {
            if (validarAsignacionOperador())
            {
                FrmAsignarOperador frm = new FrmAsignarOperador(vehiculosProceso[0].Placa, vehiculosProceso[0].NombreCliente);

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    int idOperador = frm.OperadorSeleccionadoId.Value;

                    DatabaseQueryLDB.ExecuteNonQuery(
                       "UPDATE Turnos SET idOperario = ?, OperadorOcupado =  1 WHERE IdTurno = ?",
                       idOperador, vehiculosProceso[0].IdTurno
                       );

                    cargarDetalleTurnoVehiculo(vehiculosProceso[0].IdTurno);
                }
            }
        }

        private bool validarAsignacionOperador()
        {
            bool esValido = true;
            if (vehiculosProceso[0].idOperario.HasValue)
            {
                MessageBox.Show("El turno ya tiene un operador asignado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                esValido = false;
            }
            return esValido;

        }

        private void btnFinalizar_Click(object sender, EventArgs e)
        {
            try
            {
                if (validarTurno())
                {
                    decimal valorEfectivo = cajaDiaria[0].TotalIngresosEfectivo;
                    decimal valorTransferencias = cajaDiaria[0].TotalIngresosTransferencias;
                    decimal valorDatafono = cajaDiaria[0].TotalIngresosDatafono;
                    decimal valorFinal = 0;

                    string formaPago = string.Empty;
                    if (rbEfectivo.Checked)
                    {
                        formaPago = "EFECTIVO";
                        valorEfectivo = valorEfectivo + valorPagar;
                    }
                    else if (rbTransferencia.Checked)
                    {
                        formaPago = "TRANSFERENCIA";
                        valorTransferencias = valorTransferencias + valorPagar;
                    }
                    else if (rbDatafono.Checked)
                    {
                        formaPago = "DATAFONO";
                        valorDatafono = valorDatafono + valorPagar;
                    }
                    valorFinal = cajaDiaria[0].TotalFinal + valorPagar;

                    DatabaseQueryLDB.ExecuteNonQuery(
                        "UPDATE Turnos SET Valor = ?, Observaciones = ?, FormaPago = ?, Estado = 1, Pagado = 1, OperadorOcupado = 0 WHERE IdTurno = ?",
                        valorPagar, txtObservaciones.Text.Trim(), formaPago, vehiculosProceso[0].IdTurno
                        );

                    DatabaseQueryLDB.ExecuteNonQuery(
                        "UPDATE CajaDiaria SET TotalIngresosEfectivo = ?, TotalIngresosTransferencias = ?, TotalIngresosDatafono = ?, TotalFinal = ? WHERE idCaja = ?",
                        valorEfectivo, valorTransferencias, valorDatafono, valorFinal, cajaDiaria[0].idCaja
                        );

                    MessageBox.Show("Turno finalizado exitosamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cerrarForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al finalizar el servcio: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private bool validarTurno()
        {
            bool esValido = true;
            if (vehiculosProceso[0].idOperario == null)
            {
                MessageBox.Show("Debe asignar un operador antes de finalizar el turno.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                esValido = false;
            }
            if (string.IsNullOrWhiteSpace(txtValorPagar.Text))
            {
                MessageBox.Show("El valor a pagar es obligatorio.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                esValido = false;
            }

            if (string.IsNullOrWhiteSpace(txtObservaciones.Text))
            {
                MessageBox.Show("Las observaciones son obligatorias.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                esValido = false;
            }

            return esValido;
        }

        private void btnLiberarOperador_Click(object sender, EventArgs e)
        {
            DatabaseQueryLDB.ExecuteNonQuery(
                   "UPDATE Turnos SET OperadorOcupado = 0 WHERE IdTurno = ?",
                   vehiculosProceso[0].IdTurno
                   );
            MessageBox.Show("Operador liberado Exitosamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            cerrarForm();
        }


        private void FormaPago_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb == null || !rb.Checked)
                return;

            // Resetear todos primero
            ResetRadioButtons();

            // Aplicar color según el seleccionado
            if (rb == rbEfectivo)
            {
                rb.BackColor = Color.FromArgb(40, 167, 69);
                rb.ForeColor = Color.White;
            }
            else if (rb == rbTransferencia)
            {
                rb.BackColor = Color.FromArgb(0, 123, 255);
                rb.ForeColor = Color.White;
            }
            else if (rb == rbDatafono)
            {
                rb.BackColor = Color.FromArgb(255, 140, 0);
                rb.ForeColor = Color.White;
            }
        }

        private void ResetRadioButtons()
        {
            rbEfectivo.BackColor = Color.WhiteSmoke;
            rbTransferencia.BackColor = Color.WhiteSmoke;
            rbDatafono.BackColor = Color.WhiteSmoke;

            rbEfectivo.ForeColor = Color.Black;
            rbTransferencia.ForeColor = Color.Black;
            rbDatafono.ForeColor = Color.Black;
        }

        private void txtValorPagar_Leave(object sender, EventArgs e)
        {
            valorPagar = Convert.ToDecimal(txtValorPagar.Text);
        }

        private void txtValorPagar_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtValorPagar_KeyDown(object sender, KeyEventArgs e)
        {
            // Detecta Ctrl+V (pegar)
            if (e.Control && e.KeyCode == Keys.V)
            {
                e.SuppressKeyPress = true; // Bloquea el pegado
            }
        }
    }
}
