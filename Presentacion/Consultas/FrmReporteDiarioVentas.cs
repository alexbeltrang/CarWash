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
    public partial class FrmReporteDiarioVentas : Form
    {
        private readonly TurnosMovimientosController turnosMovimientosController = new TurnosMovimientosController();
        private readonly CajaDiariaController cajaDiariaController = new CajaDiariaController();
        private readonly ValesOperariosController valesOperariosController = new ValesOperariosController();

        public FrmReporteDiarioVentas()
        {
            InitializeComponent();
            if (UIHelper.EnModoDiseno(this)) return;
            Load += FrmReporteDiarioVentas_Load;
            btnActualizar.Click += btnActualizar_Click;
            btnCerrar.Click += (_, __) => Close();
            Resize += (_, __) =>
            {
                foreach (var label in new[] { lblFecha, lblTotalVentas, lblTotalGastos, lblTotalVales, lblNeto, lblDetalleCaja })
                    label.MaximumSize = new Size(Math.Max(300, ClientSize.Width - 100), 0);
            };
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
