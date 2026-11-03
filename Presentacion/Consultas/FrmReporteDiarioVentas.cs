using CarWash.Controladores;
using CarWash.DTOs;
using CarWash.Entidades;
using CarWash.Utilidades;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CarWash.Presentacion.Consultas
{
    public class FrmReporteDiarioVentas : Form
    {
        private readonly TurnosMovimientosController turnosMovimientosController = new TurnosMovimientosController();
        private readonly CajaDiariaController cajaDiariaController = new CajaDiariaController();
        private readonly ValesOperariosController valesOperariosController = new ValesOperariosController();

        private Label lblTitulo;
        private Label lblFecha;
        private Label lblTotalVentas;
        private Label lblTotalGastos;
        private Label lblTotalVales;
        private Label lblNeto;
        private Label lblDetalleCaja;
        private Button btnActualizar;
        private DataGridView dtgMovimientos;

        public FrmReporteDiarioVentas()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            lblTitulo = new Label();
            lblFecha = new Label();
            lblTotalVentas = new Label();
            lblTotalGastos = new Label();
            lblTotalVales = new Label();
            lblNeto = new Label();
            lblDetalleCaja = new Label();
            btnActualizar = new Button();
            dtgMovimientos = new DataGridView();

            SuspendLayout();

            BackColor = Color.FromArgb(20, 90, 150);
            ClientSize = new Size(1000, 650);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Reporte Diario";

            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(20, 10);
            lblTitulo.Text = "REPORTE DIARIO DE VENTAS";

            btnActualizar.BackColor = Color.FromArgb(21, 101, 192);
            btnActualizar.FlatAppearance.BorderSize = 0;
            btnActualizar.FlatStyle = FlatStyle.Flat;
            btnActualizar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnActualizar.ForeColor = Color.White;
            btnActualizar.Location = new Point(850, 18);
            btnActualizar.Size = new Size(120, 35);
            btnActualizar.Text = "Actualizar";
            btnActualizar.UseVisualStyleBackColor = false;
            btnActualizar.Click += btnActualizar_Click;

            lblFecha.AutoSize = true;
            lblFecha.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFecha.ForeColor = Color.White;
            lblFecha.Location = new Point(20, 60);

            lblTotalVentas.AutoSize = true;
            lblTotalVentas.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalVentas.ForeColor = Color.White;
            lblTotalVentas.Location = new Point(20, 95);

            lblTotalGastos.AutoSize = true;
            lblTotalGastos.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalGastos.ForeColor = Color.White;
            lblTotalGastos.Location = new Point(20, 125);

            lblTotalVales.AutoSize = true;
            lblTotalVales.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalVales.ForeColor = Color.White;
            lblTotalVales.Location = new Point(20, 155);

            lblNeto.AutoSize = true;
            lblNeto.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNeto.ForeColor = Color.White;
            lblNeto.Location = new Point(20, 185);

            lblDetalleCaja.AutoSize = true;
            lblDetalleCaja.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDetalleCaja.ForeColor = Color.White;
            lblDetalleCaja.Location = new Point(20, 215);

            dtgMovimientos.AllowUserToAddRows = false;
            dtgMovimientos.AllowUserToDeleteRows = false;
            dtgMovimientos.AllowUserToResizeColumns = false;
            dtgMovimientos.AllowUserToResizeRows = false;
            dtgMovimientos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dtgMovimientos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dtgMovimientos.BackgroundColor = Color.White;
            dtgMovimientos.BorderStyle = BorderStyle.None;
            dtgMovimientos.Location = new Point(20, 250);
            dtgMovimientos.ReadOnly = true;
            dtgMovimientos.RowHeadersVisible = false;
            dtgMovimientos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dtgMovimientos.Size = new Size(950, 380);

            Controls.Add(lblTitulo);
            Controls.Add(btnActualizar);
            Controls.Add(lblFecha);
            Controls.Add(lblTotalVentas);
            Controls.Add(lblTotalGastos);
            Controls.Add(lblTotalVales);
            Controls.Add(lblNeto);
            Controls.Add(lblDetalleCaja);
            Controls.Add(dtgMovimientos);

            Load += FrmReporteDiarioVentas_Load;

            ResumeLayout(false);
            PerformLayout();
        }

        private void FrmReporteDiarioVentas_Load(object sender, EventArgs e)
        {
            CargarReporte();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarReporte();
        }

        private void CargarReporte()
        {
            try
            {
                if (!clsConnection.EsAdministrador)
                    throw new System.Security.SecurityException("Solo el perfil Administrador puede consultar ventas.");
                CargarDatosReporte();
            }
            catch (Exception exception)
            {
                dtgMovimientos.DataSource = null;
                lblTotalVentas.Text = "Total ventas: No disponible";
                lblTotalGastos.Text = "Total gastos: No disponible";
                lblTotalVales.Text = "Total vales operarios: No disponible";
                lblNeto.Text = "Neto del dia: No disponible";
                lblDetalleCaja.Text = string.Empty;
                MessageBox.Show(exception.Message, "Consulta no disponible", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (exception is UnauthorizedAccessException)
                {
                    using (var login = new CarWash.Presentacion.Login.FrmLogin(true)) login.ShowDialog(this);
                }
            }
        }

        private void CargarDatosReporte()
        {
            var fechaHoy = DateTime.Today;
            lblFecha.Text = "Fecha: " + fechaHoy.ToString("dd/MM/yyyy");

            List<ConsultaMovimientosDTO> historial = turnosMovimientosController.GetHistoricoMovimientos(fechaHoy, fechaHoy, null, null)
                ?? new List<ConsultaMovimientosDTO>();

            decimal totalVentas = historial.Sum(x => x.Valor);
            decimal totalGastos = 0;
            decimal totalVales = 0;

            CajaDiaria cajaActiva = cajaDiariaController.GetCajaActiva();
            if (cajaActiva != null && cajaActiva.idCaja > 0)
            {
                totalGastos = cajaActiva.TotalEgresos;

                var vales = valesOperariosController.GetAllValesOperarios(cajaActiva.idCaja);
                totalVales = vales.Sum(v => v.Valor);

                lblDetalleCaja.Text =
                    "Caja - Efectivo: " + cajaActiva.TotalIngresosEfectivo.ToString("N0") +
                    " | Transferencia: " + cajaActiva.TotalIngresosTransferencias.ToString("N0") +
                    " | Datafono: " + cajaActiva.TotalIngresosDatafono.ToString("N0") +
                    " | Crédito: " + cajaActiva.TotalIngresosCredito.ToString("N0");
            }
            else
            {
                lblDetalleCaja.Text = "No hay caja activa para hoy.";
            }

            decimal neto = totalVentas - totalGastos - totalVales;

            lblTotalVentas.Text = "Total ventas: " + totalVentas.ToString("N0");
            lblTotalGastos.Text = "Total gastos: " + totalGastos.ToString("N0");
            lblTotalVales.Text = "Total vales operarios: " + totalVales.ToString("N0");
            lblNeto.Text = "Neto del día: " + neto.ToString("N0");

            dtgMovimientos.DataSource = historial;

            if (dtgMovimientos.Columns["Valor"] != null)
                dtgMovimientos.Columns["Valor"].HeaderText = "Valor ($)";

            if (dtgMovimientos.Columns["ValorBaseComision"] != null)
                dtgMovimientos.Columns["ValorBaseComision"].HeaderText = "Valor Base ($)";

            if (dtgMovimientos.Columns["FechaHoraIngreso"] != null)
                dtgMovimientos.Columns["FechaHoraIngreso"].HeaderText = "Fecha Servicio";

            if (dtgMovimientos.Columns["ClienteCredito"] != null)
                dtgMovimientos.Columns["ClienteCredito"].HeaderText = "Cliente Crédito";
        }
    }
}
