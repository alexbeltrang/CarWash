using CarWash.Controladores;
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

namespace CarWash.Presentacion.Consultas
{
    public partial class FrmConsultaMovimientosCaja : Form
    {
        decimal precioTotalServicio = 0;
        decimal precioBaseTotalServicio = 0;

        TurnosMovimientosController turnosMovimientosController = new TurnosMovimientosController();
        FormaPagoController formaPagoController = new FormaPagoController();
        OperariosController operariosController = new OperariosController();

        CajaDiaria cajaDiaria = new CajaDiaria();
        public FrmConsultaMovimientosCaja()
        {
            InitializeComponent();
            cargarFormasPago();
            CargaOperarios();
        }


        private void cargarFormasPago()
        {
            // Aquí puedes cargar los combos con datos de la base de datos
            var formasPago = formaPagoController.GetAllFormaPago();

            formasPago.Insert(0, new FormaPago
            {
                IdFormaPago = 0,
                Nombre = "-- Seleccione --"
            });
            cmbFormaPago.DataSource = formasPago;
            cmbFormaPago.DisplayMember = "Nombre";
            cmbFormaPago.ValueMember = "IdFormaPago";
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
            );

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= 0x20000; // sombra
                return cp;
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



        private void CLEAR()
        {
            cmbFormaPago.SelectedValue = 0;

            foreach (Control c in this.Controls)
            {
                if (c is TextBox txt && !txt.ReadOnly)
                {
                    txt.Clear();
                }
            }

            cmbFormaPago.SelectedIndex = -1;
            dtgMovimientos.DataSource = null;
        }


        private void CargarHistorico()
        {
            try
            {
                var historial = turnosMovimientosController.GetHistoricoMovimientos(dtpFechaInicial.Value,
                    dtpFechaFinal.Value,
                    (int?)cmbFormaPago.SelectedValue,
                    (int?)cmbOperario.SelectedValue);

                // ===== SUMAS =====
                decimal totalValor = historial.Sum(x => x.Valor);
                decimal totalBase = historial.Sum(x => x.ValorBaseComision);

                lblTotal.Text = "Valor Total Movimiento: " + totalValor.ToString("N0");




                dtgMovimientos.DataSource = historial;


                dtgMovimientos.Columns["Valor"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dtgMovimientos.Columns["ValorBaseComision"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                dtgMovimientos.Columns["FormaPago"].HeaderText = "Forma Pago";
                dtgMovimientos.Columns["FechaHoraIngreso"].HeaderText = "Fecha Servicio";
                dtgMovimientos.Columns["ClienteCredito"].HeaderText = "Cliente Crédito";
                dtgMovimientos.Columns["Placa"].HeaderText = "Placa";
                dtgMovimientos.Columns["TipoVehiculo"].HeaderText = "Tipo Vehiculo";
                //dtgMovimientos.Columns["Servicio"].HeaderText = "Servicio";
                dtgMovimientos.Columns["Valor"].HeaderText = "Valor ($)";
                dtgMovimientos.Columns["ValorBaseComision"].HeaderText = "Valor Base ($)";

            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
        }


        private void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarHistorico();
        }

        private void FrmConsultaMovimientosCaja_Load(object sender, EventArgs e)
        {
            dtpFechaInicial.Value = DateTime.Now;
            dtpFechaFinal.Value = DateTime.Now;
        }

        private void CargaOperarios()
        {
            var operadoresDisponibles = operariosController.GetOperariosActivos();


            operadoresDisponibles.Insert(0, new Operarios
            {
                idOperario = 0,
                Nombres = "-- Seleccione --"
            });

            cmbOperario.DataSource = operadoresDisponibles;
            cmbOperario.DisplayMember = "Nombres";
            cmbOperario.ValueMember = "idOperario";
            cmbOperario.SelectedIndex = 0;

        }
    }
}
