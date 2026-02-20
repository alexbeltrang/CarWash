using CarWash.Database;
using CarWash.DTOs;
using CarWash.Entidades;
using CarWash.Presentacion.Operacion;
using SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace CarWash.Presentacion.Principal
{
    public partial class FrmDashboard : Form
    {
        private Timer timerMinuto;
        private bool _formDetalleAbierto = false;
        private List<GestionVehiculosDTO> vehiculosProceso;

        public FrmDashboard()
        {
            InitializeComponent();
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect,
            int nBottomRect, int nWidthEllipse, int nHeightEllipse);


        private void FrmDashboard_Load(object sender, EventArgs e)
        {
            timerMinuto = new Timer();
            timerMinuto.Interval = 10000; // 60000 ms = 1 minuto
            timerMinuto.Tick += TimerMinuto_Tick; // Evento que se ejecuta cada intervalo
            timerMinuto.Start(); // Inicia el Timer
            this.BackColor = Color.FromArgb(18, 18, 18);
            flpOperadores.BackColor = Color.FromArgb(28, 28, 28);


            CargarEstadisticas();
            cargarVehiculosEnProceso();
            CargarDashboardOperadores();
        }


        private void CargarEstadisticas()
        {
            try
            {
                // Servicios activos (ejemplo campo Estado = 1)
                int serviciosActivos = DatabaseQueryLDB.ExecuteScalar<int>(
                    "SELECT COUNT(*) FROM Turnos WHERE Estado = 0");

                lblServicios.Text = "Servicios Activos: " + serviciosActivos;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando estadísticas: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TimerMinuto_Tick(object sender, EventArgs e)
        {
            timerMinuto.Stop();
            CargarEstadisticas();
            cargarVehiculosEnProceso();
            CargarDashboardOperadores();
            timerMinuto.Start();
        }


        private void cargarVehiculosEnProceso()
        {

            try
            {
                vehiculosProceso = DatabaseQueryLDB.ExecuteList<GestionVehiculosDTO>(
               @"SELECT TUR.IdTurno,TUR.Placa,TUR.NombreCliente,TUR.NumeroCelular, TUR.NumeroTurno, strftime('%Y-%m-%d %H:%M',TUR.FechaHoraIngreso / 10000000 - 62135596800,'unixepoch') AS FechaHoraIngreso, 
                        TVH.Nombre TipoVehiculo, SER.Nombre Servicio, COALESCE(OPE.Nombres, '') || ' ' || COALESCE(OPE.Apellidos, '') AS OperadorAsignado, '$ ' || printf('%.2f', Valor) AS ValorCliente, TUR.OperadorOcupado
                        FROM Turnos TUR INNER JOIN TipoVehiculo TVH ON TUR.IdTipoVehiculo = TVH.idTipoVehiculo 
                        INNER JOIN Servicios SER ON SER.idServicio = TUR.idServicio 
                        LEFT OUTER JOIN Operarios OPE ON TUR.idOperario = OPE.idOperario 
                        WHERE TUR.Estado = 0 
                        ORDER BY TUR.FechaHoraIngreso ASC");


                llenarDataGrid(vehiculosProceso);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando vehículos en proceso: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void llenarDataGrid(List<GestionVehiculosDTO> vehiculos)
        {
            dvVehiculosProceso.SuspendLayout();
            dvVehiculosProceso.DataSource = null;
            dvVehiculosProceso.DataSource = vehiculos;


            dvVehiculosProceso.Columns["IdTurno"].Visible = false;
            dvVehiculosProceso.Columns["Placa"].HeaderText = "Placa";
            dvVehiculosProceso.Columns["NombreCliente"].HeaderText = "Nombre Cliente";
            dvVehiculosProceso.Columns["NumeroCelular"].HeaderText = "Celular";
            dvVehiculosProceso.Columns["NumeroTurno"].HeaderText = "N° Turno";
            dvVehiculosProceso.Columns["FechaHoraIngreso"].HeaderText = "Fecha Ingreso";
            dvVehiculosProceso.Columns["TipoVehiculo"].HeaderText = "Tipo Vehiculo";
            dvVehiculosProceso.Columns["Servicio"].HeaderText = "Servicio";
            dvVehiculosProceso.Columns["OperadorAsignado"].HeaderText = "Operador Asignado";
            dvVehiculosProceso.Columns["ValorCliente"].HeaderText = "Valor";
            dvVehiculosProceso.Columns["OperadorOcupado"].HeaderText = "En servicio";
            dvVehiculosProceso.Columns["Estado"].Visible = false;
            dvVehiculosProceso.Columns["idOperario"].Visible = false;
            dvVehiculosProceso.Columns["Observaciones"].Visible = false;
            dvVehiculosProceso.Columns["Valor"].Visible = false;
            dvVehiculosProceso.ResumeLayout();
        }


        private void CargarDashboardOperadores()
        {
            flpOperadores.SuspendLayout();
            flpOperadores.Controls.Clear();

            var lista = DatabaseQueryLDB.ExecuteList<DashboardOperadoresDTO>(
            @"WITH Diferencia AS (
                    SELECT 
                        o.Nombres || ' ' || o.Apellidos AS NombreOperador,
                        t.Placa,
                        (strftime('%s','now','localtime') - 
                        (t.FechaHoraIngreso / 10000000 - 62135596800)
                        ) AS Segundos
                    FROM Turnos t
                    INNER JOIN Operarios o ON o.idOperario = t.idOperario
                    WHERE t.Estado = 0 and t.OperadorOcupado = 1
                )
                SELECT 
                    NombreOperador,
                    Placa,
                    Segundos / 60 AS MinutosTranscurridos,
                    Segundos / 3600 AS Horas,
                    (Segundos % 3600) / 60 AS MinutosRestantes
                FROM Diferencia
                ORDER BY MinutosTranscurridos DESC;");

            int ranking = 1;

            foreach (var item in lista)
            {
                Panel card = CrearTarjeta(item, ranking);
                flpOperadores.Controls.Add(card);
                ranking++;
            }

            // 🔥 KPIs
            lblTotalProceso.Text = "En proceso: " + lista.Count;

            if (lista.Count > 0)
            {
                lblPromedioTiempo.Text = "Promedio: " +
                    lista.Average(x => x.MinutosTranscurridos).ToString("0") + " min";

                lblMasLento.Text = "Más lento: " + lista.First().NombreOperador;
            }
            else
            {
                lblPromedioTiempo.Text = "Promedio: 0 min";
                lblMasLento.Text = "Más lento: -";
            }

            flpOperadores.ResumeLayout();
        }


        private Panel CrearTarjeta(DashboardOperadoresDTO item, int ranking)
        {
            Panel panel = new Panel();
            panel.Width = 140;
            panel.Height = 115; // 👈 MÁS ALTA
            panel.Margin = new Padding(8);
            panel.BackColor = ObtenerColorTiempo(item.MinutosTranscurridos);

            // ===== RANKING =====
            Label lblRanking = new Label();
            lblRanking.Text = "#" + ranking;
            lblRanking.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            lblRanking.ForeColor = Color.White;
            lblRanking.AutoSize = true;
            lblRanking.Location = new Point(5, 5);
            panel.Controls.Add(lblRanking);

            // ===== NOMBRE =====
            Label lblNombre = new Label();
            lblNombre.Text = item.NombreOperador;
            lblNombre.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            lblNombre.ForeColor = Color.White;
            lblNombre.AutoSize = false;
            lblNombre.Size = new Size(130, 20);
            lblNombre.Location = new Point(5, 22);
            lblNombre.TextAlign = ContentAlignment.MiddleLeft;
            panel.Controls.Add(lblNombre);

            // ===== PLACA =====
            Label lblPlaca = new Label();
            lblPlaca.Text = "🚗 " + item.Placa;
            lblPlaca.Font = new Font("Segoe UI", 8);
            lblPlaca.ForeColor = Color.WhiteSmoke;
            lblPlaca.AutoSize = false;
            lblPlaca.Size = new Size(130, 20);
            lblPlaca.Location = new Point(5, 45);
            lblPlaca.TextAlign = ContentAlignment.MiddleLeft;
            panel.Controls.Add(lblPlaca);

            // ===== TIEMPO =====
            Label lblTiempo = new Label();
            lblTiempo.Text = item.MinutosTranscurridos + " min";
            lblTiempo.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblTiempo.ForeColor = Color.White;
            lblTiempo.AutoSize = false;
            lblTiempo.Size = new Size(130, 25);
            lblTiempo.Location = new Point(5, 75);
            lblTiempo.TextAlign = ContentAlignment.MiddleCenter;
            panel.Controls.Add(lblTiempo);

            return panel;
        }
        private Color ObtenerColorTiempo(int minutos)
        {
            if (minutos <= 30)
                return Color.FromArgb(39, 174, 96); // Verde

            if (minutos <= 60)
                return Color.FromArgb(241, 196, 15); // Amarillo

            if (minutos <= 90)
                return Color.FromArgb(230, 126, 34); // Naranja

            return Color.FromArgb(192, 57, 43); // Rojo fuerte
        }

        private void AplicarBordesRedondeados(Control control, int radio)
        {
            control.Region = Region.FromHrgn(
                CreateRoundRectRgn(0, 0, control.Width, control.Height, radio, radio));
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

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            string filtro = txtBuscar.Text.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(filtro))
            {
                llenarDataGrid(vehiculosProceso);
                return;
            }

            var filtrados = vehiculosProceso
                .Where(x =>
                    (!string.IsNullOrEmpty(x.Placa) && x.Placa.ToLower().Contains(filtro))
                )
                .ToList();

            llenarDataGrid(filtrados);
        }
    }
}
