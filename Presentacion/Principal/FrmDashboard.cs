using CarWash.Database;
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
        }

    }
}
