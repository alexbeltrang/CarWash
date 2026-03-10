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

        CajaDiaria cajaDiaria = new CajaDiaria();
        public FrmConsultaMovimientosCaja()
        {
            InitializeComponent();
            cargarFormasPago();
        }


        private void cargarFormasPago()
        {
            // Aquí puedes cargar los combos con datos de la base de datos
            var formasPago = DatabaseQueryLDB.ExecuteList<FormaPago>("select IdFormaPago, Nombre from FormaPago");
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
                string sql = @"SELECT TUR.Placa, 
                       strftime('%Y-%m-%d %H:%M',TUR.FechaHoraIngreso / 10000000 - 62135596800,'unixepoch') AS FechaHoraIngreso, 
                       tum.MontoPagado as Valor, 
                       TUR.ValorBaseComision, 
                       TUR.Pagado, 
                       TUR.Observaciones, 
                       frp.Nombre as FormaPago, 
                       tpv.Nombre TipoVehiculo, 
                       COALESCE(opr.Nombres,'') || ' ' || COALESCE(opr.Apellidos,'') Operario,
                       CLC.Nombre as ClienteCredito
                FROM Turnos tur INNER JOIN TurnosMovimientos tum ON tur.IdTurno = tum.IdTurno 
                INNER JOIN FormaPago frp on tum.IdFormaPago = frp.IdFormaPago
                INNER JOIN TipoVehiculo tpv on tur.IdTipoVehiculo = tpv.IdTipoVehiculo
                INNER JOIN Operarios opr on tur.idOperario = opr.idOperario
                LEFT OUTER JOIN ClienteCredito CLC on tur.idClienteCredito = CLC.idClienteCredito
                WHERE 1 = 1 ";

                List<object> parametros = new List<object>();

                if (cmbFormaPago.SelectedIndex > 0)
                {
                    sql += " AND tum.IdFormaPago = ? ";
                    parametros.Add(cmbFormaPago.SelectedValue);
                }

                sql += " AND date(TUR.FechaHoraIngreso / 10000000 - 62135596800,'unixepoch') BETWEEN ? AND ? ";

                parametros.Add(dtpFechaInicial.Value.ToString("yyyy-MM-dd"));
                parametros.Add(dtpFechaFinal.Value.ToString("yyyy-MM-dd"));

                var historial = DatabaseQueryLDB.ExecuteList<ConsultaMovimientosDTO>(
                    sql,
                    parametros.ToArray()
                );

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
    }
}
