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
                MessageBox.Show("Error cargando vehículos en proceso: " + ex.Message);
            }
        }


        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAsignarOperador_Click(object sender, EventArgs e)
        {
            FrmAsignarOperador frm = new FrmAsignarOperador(vehiculosProceso[0].Placa, vehiculosProceso[0].NombreCliente);

            if (frm.ShowDialog() == DialogResult.OK)
            {
                int idOperador = frm.OperadorSeleccionadoId.Value;

                DatabaseQueryLDB.ExecuteNonQuery(
                   "UPDATE Turnos SET idOperario = ? WHERE IdTurno = ?",
                   idOperador, vehiculosProceso[0].IdTurno
                   );

                cargarDetalleTurnoVehiculo(vehiculosProceso[0].IdTurno);
            }
        }
    }
}
