using CarWash.Database;
using CarWash.DTOs;
using CarWash.Entidades;
using CarWash.Presentacion.Operacion;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarWash.Presentacion.Principal
{
    public partial class FrmDashboard : Form
    {
        private Timer timerMinuto;
        private bool _formDetalleAbierto = false;

        public FrmDashboard()
        {
            InitializeComponent();
        }

        private void FrmDashboard_Load(object sender, EventArgs e)
        {
            timerMinuto = new Timer();
            timerMinuto.Interval = 60000; // 60000 ms = 1 minuto
            timerMinuto.Tick += TimerMinuto_Tick; // Evento que se ejecuta cada intervalo
            timerMinuto.Start(); // Inicia el Timer

            CargarEstadisticas();
            cargarVehiculosEnProceso();
        }


        private void CargarEstadisticas()
        {
            try
            {
                // Clientes
                //int totalClientes = DatabaseQueryLDB.ExecuteScalar<int>(
                //    "SELECT COUNT(*) FROM Cliente");
                int totalClientes = 0;
                lblClientes.Text = "Clientes Registrados: " + totalClientes;

                // Servicios activos (ejemplo campo Estado = 1)
                int serviciosActivos = DatabaseQueryLDB.ExecuteScalar<int>(
                    "SELECT COUNT(*) FROM Turnos WHERE Estado = 1");

                lblServicios.Text = "Servicios Activos: " + serviciosActivos;

                // Ventas del día (ejemplo con fecha actual)
                decimal ventasHoy = DatabaseQueryLDB.ExecuteScalar<decimal>(
                    "SELECT IFNULL(SUM(Valor),0) FROM Turnos WHERE  DATE(datetime((FechaHoraIngreso / 10000000) - 62135596800, 'unixepoch', 'localtime')) = DATE('now', 'localtime')  AND Pagado = 1");

                lblVentas.Text = "Ventas Hoy: $" + ventasHoy.ToString("N0");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando estadísticas: " + ex.Message);
            }
        }

        private void TimerMinuto_Tick(object sender, EventArgs e)
        {
            CargarEstadisticas();
            cargarVehiculosEnProceso();
        }


        private void cargarVehiculosEnProceso()
        {
            try
            {
                var vehiculosProceso = DatabaseQueryLDB.ExecuteList<GestionVehiculosDTO>(
                @"SELECT TUR.IdTurno,TUR.Placa,TUR.NombreCliente,TUR.NumeroCelular, TUR.NumeroTurno, strftime('%Y-%m-%d %H:%M',TUR.FechaHoraIngreso / 10000000 - 62135596800,'unixepoch') AS FechaHoraIngreso, TVH.Nombre TipoVehiculo, SER.Nombre Servicio, COALESCE(OPE.Nombres, '') || ' ' || COALESCE(OPE.Apellidos, '') AS OperadorAsignado 
                  FROM Turnos TUR INNER JOIN TipoVehiculo TVH ON TUR.IdTipoVehiculo = TVH.idTipoVehiculo 
                  INNER JOIN Servicios SER ON SER.idServicio = TUR.idServicio 
                  LEFT OUTER JOIN Operarios OPE ON TUR.idOperario = OPE.idOperario 
                  WHERE TUR.Estado = 0 
                  ORDER BY TUR.FechaHoraIngreso ASC");

                dvVehiculosProceso.DataSource = vehiculosProceso;

                dvVehiculosProceso.Columns["IdTurno"].Visible = false;
                dvVehiculosProceso.Columns["Placa"].HeaderText = "Placa";
                dvVehiculosProceso.Columns["NombreCliente"].HeaderText = "Nombre Cliente";
                dvVehiculosProceso.Columns["NumeroCelular"].HeaderText = "Celular";
                dvVehiculosProceso.Columns["NumeroTurno"].HeaderText = "N° Turno";
                dvVehiculosProceso.Columns["FechaHoraIngreso"].HeaderText = "Fecha Ingreso";
                dvVehiculosProceso.Columns["TipoVehiculo"].HeaderText = "Tipo Vehiculo";
                dvVehiculosProceso.Columns["Servicio"].HeaderText = "Servicio";
                dvVehiculosProceso.Columns["OperadorAsignado"].HeaderText = "Operador Asignado";
                dvVehiculosProceso.Columns["Estado"].Visible = false;
                dvVehiculosProceso.Columns["idOperario"].Visible = false;
                dvVehiculosProceso.Columns["Observaciones"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando vehículos en proceso: " + ex.Message);
            }
        }

        private void dvVehiculosProceso_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            // Si ya está abierto, no permitir otro
            if (_formDetalleAbierto)
                return;

            try
            {
                _formDetalleAbierto = true;

                DataGridViewRow fila = dvVehiculosProceso.Rows[e.RowIndex];
                int idTurno = Convert.ToInt32(fila.Cells["IdTurno"].Value);

                using (FrmGestionDetalleVehiculo frm = new FrmGestionDetalleVehiculo(idTurno))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        cargarVehiculosEnProceso();
                    }
                }
            }
            finally
            {
                _formDetalleAbierto = false;
            }
        }

    }
}
