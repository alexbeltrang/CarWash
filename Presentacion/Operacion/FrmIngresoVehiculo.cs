using CarWash.Controladores;
using CarWash.Database;
using CarWash.DTOs;
using CarWash.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace CarWash.Presentacion.Operacion
{
    public partial class FrmIngresoVehiculo : Form
    {
        CajaDiariaController cajaDiariaController = new CajaDiariaController();
        TipoVehiculoController tipoVehiculoController = new TipoVehiculoController();
        ServiciosController serviciosController = new ServiciosController();
        TurnosController turnosController = new TurnosController();
        TurnosServiciosController turnosServiciosController = new TurnosServiciosController();
        TurnosDiariosController turnosDiariosController = new TurnosDiariosController();

        decimal precioTotalServicio = 0;
        decimal precioBaseTotalServicio = 0;

        CajaDiaria cajaDiaria = new CajaDiaria();
        List<ServicioListaDTO> servicioCombos = new List<ServicioListaDTO>();
        List<ServicioComboDTO> servicios = new List<ServicioComboDTO>();
        public FrmIngresoVehiculo()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            btnGuardar.MouseEnter += (s, e) =>
            {
                btnGuardar.BackColor = Color.FromArgb(33, 136, 56);
            };

            btnGuardar.MouseLeave += (s, e) =>
            {
                btnGuardar.BackColor = Color.FromArgb(40, 167, 69);
            };

            btnCancelar.MouseEnter += (s, e) =>
            {
                btnCancelar.BackColor = Color.FromArgb(200, 35, 51);
            };

            btnCancelar.MouseLeave += (s, e) =>
            {
                btnCancelar.BackColor = Color.FromArgb(220, 53, 69);
            };

            this.Opacity = 0;
            Timer fade = new Timer();
            fade.Interval = 20;
            fade.Tick += (s, e) =>
            {
                if (this.Opacity < 1)
                    this.Opacity += 0.05;
                else
                    fade.Stop();
            };
            fade.Start();
        }

        private void FrmIngresoVehiculo_Load(object sender, EventArgs e)
        {
            txtPlaca.CharacterCasing = CharacterCasing.Upper;
            txtCliente.CharacterCasing = CharacterCasing.Upper;
            txtObservaciones.CharacterCasing = CharacterCasing.Upper;

            //panelCard.Left = (this.ClientSize.Width - panelCard.Width) / 2;
            //panelCard.Top = (this.ClientSize.Height - panelCard.Height) / 2;

            //panelCard.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panelCard.Width, panelCard.Height, 20, 20));
            EstilizarTextBox(txtPlaca);
            EstilizarTextBox(txtCliente);
            EstilizarTextBox(txtValor);
            EstilizarTextBox(txtCelular);
            EstilizarTextBox(txtValorBase);
            EstilizarTextBox(txtObservaciones);
            cargarTipoVehiculo();
            cargaCajaDiaria();
            CargarListaCompletaServicios();
            txtPlaca.Focus();
        }

        private void cargarTipoVehiculo()
        {

            // Aquí puedes cargar los combos con datos de la base de datos
            var servicios = tipoVehiculoController.GetAllTipoVehiculo().Where(t => !t.IsDelete).ToList();

            servicios.Insert(0, new TipoVehiculo
            {
                IdTipoVehiculo = 0,
                Nombre = "-- Seleccione --"
            });

            cmbTipoVehiculo.DataSource = servicios;
            cmbTipoVehiculo.DisplayMember = "Nombre";
            cmbTipoVehiculo.ValueMember = "idTipoVehiculo";
        }

        private void cargaCajaDiaria()
        {
            var cajaDiaria = cajaDiariaController.GetCajaActiva();
        }


        private void CargarListaCompletaServicios()
        {
            servicios = serviciosController.CargarListaCompletaServicios();
        }

        private void cargarListaServicio(int TipoVehiculo)
        {
            CargarListaCompletaServicios();
            servicioCombos = servicios
                .Where(s => s.idTipoVehiculo == TipoVehiculo)
                .Select(s => new ServicioListaDTO
                {
                    idServicio = s.idServicio,
                    Nombre = s.Nombre
                }).ToList();

            if (servicios == null)
            {
                MessageBox.Show("Servicios es NULL");
                return;
            }
            lstServiciosVehiculo.Items.Clear();

            foreach (var s in servicioCombos)
            {
                lstServiciosVehiculo.Items.Add(s);
            }
        }

        private bool ValidarCampos()
        {
            bool valido = true;
            cargaCajaDiaria();

            foreach (Control c in this.Controls)
            {
                if (c is TextBox txt && !txt.ReadOnly)
                {
                    if (txt.Tag?.ToString() == "opcional")
                        continue;
                    if (string.IsNullOrWhiteSpace(txt.Text))
                    {
                        txt.BackColor = Color.FromArgb(255, 230, 230);
                        valido = false;
                    }
                    else
                    {
                        txt.BackColor = Color.White;
                    }
                }
            }

            if (cmbTipoVehiculo.SelectedIndex == -1)
            {
                cmbTipoVehiculo.BackColor = Color.FromArgb(255, 230, 230);
                valido = false;
            }
            else
            {
                cmbTipoVehiculo.BackColor = Color.White;
            }

            if (lstServiciosVehiculo.CheckedItems.Count == 0)
            {
                MessageBox.Show("Debe seleccionar al menos un servicio.");
                valido = false;
            }

            return valido;
        }


        private bool validaCajadiaria()
        {

            bool retrono = true;
            cargaCajaDiaria();
            if (cajaDiaria == null)
            {
                MostrarToast("No existe caja abierta para el día de hoy.", Color.FromArgb(220, 53, 69));
                retrono = false;
            }
            return retrono;
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

        private void EstilizarTextBox(TextBox txt)
        {
            txt.BorderStyle = BorderStyle.None;
            txt.BackColor = Color.White;
            txt.Font = new Font("Segoe UI", 10);

            Panel linea = new Panel();
            linea.Height = 2;
            linea.Width = txt.Width;
            linea.BackColor = Color.FromArgb(15, 76, 129);
            linea.Left = txt.Left;
            linea.Top = txt.Bottom + 1;

            this.Controls.Add(linea);

            txt.GotFocus += (s, e) =>
            {
                linea.BackColor = Color.FromArgb(40, 167, 69);
            };

            txt.LostFocus += (s, e) =>
            {
                linea.BackColor = Color.FromArgb(15, 76, 129);
            };
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            Turnos turnos = new Turnos();
            if (!ValidarCampos())
            {
                MostrarToast("Complete todos los campos obligatorios", Color.FromArgb(220, 53, 69));
                return;
            }
            else if (!validaCajadiaria())
            {
                return;
            }


            precioBaseTotalServicio = Convert.ToDecimal(txtValorBase.Text);
            precioTotalServicio = Convert.ToDecimal(txtValor.Text);

            turnos.NumeroTurno = GenerarTurno();
            turnos.NombreCliente = txtCliente.Text.Trim();
            turnos.NumeroCelular = txtCelular.Text.Trim();
            turnos.Placa = txtPlaca.Text.Trim();
            turnos.FechaHoraIngreso = DateTime.Now;
            turnos.Marca = txtMarca.Text.Trim();
            turnos.NumeroOrden = txtNumOrden.Text.Trim();
            turnos.Valor = precioTotalServicio;
            turnos.ValorBaseComision = precioBaseTotalServicio;
            turnos.Pagado = false;
            turnos.Observaciones = txtObservaciones.Text.Trim();
            turnos.Estado = false;
            turnos.IdTipoVehiculo = Convert.ToInt32(cmbTipoVehiculo.SelectedValue);
            turnos.OperadorOcupado = false;
            turnos.idCajaDiaria = cajaDiaria.idCaja;

            long idTurno = turnosController.RegistrarTurno(turnos);

            List<int> serviciosSeleccionados = new List<int>();

            foreach (var item in lstServiciosVehiculo.CheckedItems)
            {
                var servicio = (ServicioListaDTO)item;
                TurnoServicios turnoServicios = new TurnoServicios
                {
                    idServicios = servicio.idServicio,
                    IdTurno = (int)idTurno,
                    IsDeleted = false
                };

                turnosServiciosController.RegistrarTurnoServicio(turnoServicios);
            }
            MostrarToast("Ingreso registrado correctamente", Color.FromArgb(40, 167, 69));
            CLEAR();
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

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            CLEAR();
        }

        private void CLEAR()
        {
            cmbTipoVehiculo.SelectedValue = 0;
            lstServiciosVehiculo.ClearSelected();

            foreach (Control c in this.Controls)
            {
                if (c is TextBox txt && !txt.ReadOnly)
                {
                    txt.Clear();
                }
            }

            cmbTipoVehiculo.SelectedIndex = -1;
            lstServiciosVehiculo.ClearSelected();
            for (int i = 0; i < lstServiciosVehiculo.Items.Count; i++)
                lstServiciosVehiculo.SetItemChecked(i, false);
            dgvHistorico.DataSource = null;
            txtPlaca.Clear();
            txtPlaca.Focus();
            lstServiciosVehiculo.Items.Clear();
        }

        private void cmbTipoVehiculo_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int tipoVehiculo = Convert.ToInt32(cmbTipoVehiculo.SelectedValue);
            cargarListaServicio(tipoVehiculo);

        }

        private string GenerarTurno()
        {
            string turno = "";
            string fechaHoy = DateTime.Today.ToString("yyyy-MM-dd");
            int? consecutivoActual = 0;
            var turnosSel = turnosDiariosController.consultaByFecha(fechaHoy);

            if (turnosSel.Count > 0)
            {
                consecutivoActual = turnosSel.FirstOrDefault().Consecutivo;
            }
            else
            {
                consecutivoActual = 0;
            }
            // Buscar consecutivo actual

            int nuevoConsecutivo = 0;

            if (consecutivoActual == 0)
            {
                TurnosDiarios turnosDiarios = new TurnosDiarios();
                turnosDiarios.Consecutivo = 1;
                turnosDiarios.Fecha = fechaHoy;
                nuevoConsecutivo = 1;
                turnosDiariosController.RegistrarTurnoDiario(turnosDiarios);
            }
            else
            {
                // Ya existe → incrementar
                nuevoConsecutivo = consecutivoActual.Value + 1;

                turnosSel.FirstOrDefault().Consecutivo = nuevoConsecutivo;

                turnosDiariosController.ActualizarTurnoDiario(turnosSel.FirstOrDefault());
            }

            turno = $"T-{DateTime.Today:yyyyMMdd}-{nuevoConsecutivo:000}";

            return turno;
        }

        private void txtPlaca_Leave(object sender, EventArgs e)
        {
            cargaDatosPlaca();
        }


        private void cargaDatosPlaca()
        {
            string placa = txtPlaca.Text.Trim().ToUpper();

            txtPlaca.Text = placa;

            if (string.IsNullOrWhiteSpace(placa))
                return;

            BuscarVehiculoPorPlaca(placa);
            CargarHistorico(placa);
        }
        private void BuscarVehiculoPorPlaca(string placa)
        {
            var vehiculo = turnosController.BuscarByPlaca(placa);

            //var vehiculo = DatabaseQueryLDB.ExecuteList<Turnos>(
            //    @"SELECT IdTurno,NombreCliente,NumeroCelular,Placa,IdTipoVehiculo, Estado, Marca FROM Turnos
            //      WHERE Placa = ?  ORDER BY FechaHoraIngreso DESC", placa).FirstOrDefault();

            if (vehiculo != null)
            {
                // Cargar datos en los controles
                cmbTipoVehiculo.SelectedValue = vehiculo.IdTipoVehiculo;
                cargarListaServicio(vehiculo.IdTipoVehiculo);
                txtCliente.Text = vehiculo.NombreCliente;
                txtCelular.Text = vehiculo.NumeroCelular;
                txtMarca.Text = vehiculo.Marca;
            }
            else
            {
                // Limpiar si no existe
                cmbTipoVehiculo.SelectedIndex = -1;
                lstServiciosVehiculo.ClearSelected();
                txtValor.Clear();
            }
        }

        private void CargarHistorico(string placa)
        {
            var historial = turnosController.HistoricoByPlaca(placa);

            dgvHistorico.DataSource = historial;
            dgvHistorico.Columns["Valor"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvHistorico.Columns["IdTurno"].Visible = false;
            dgvHistorico.Columns["NumeroCelular"].Visible = false;
            dgvHistorico.Columns["Valor"].Visible = false;
            dgvHistorico.Columns["NumeroTurno"].Visible = false;

            dgvHistorico.Columns["NumeroTurno"].HeaderText = "N° Turno";
            dgvHistorico.Columns["FechaHoraIngreso"].HeaderText = "Fecha Servicio";
            dgvHistorico.Columns["NombreCliente"].HeaderText = "Nombre Cliente";
            dgvHistorico.Columns["Placa"].HeaderText = "Placa";
            dgvHistorico.Columns["Marca"].HeaderText = "Marca";
            dgvHistorico.Columns["TipoVehiculo"].HeaderText = "Tipo Vehiculo";
            dgvHistorico.Columns["Servicio"].HeaderText = "Servicio";
            dgvHistorico.Columns["ValorPagado"].HeaderText = "Valor ($)";

        }

        private void txtPlaca_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) // Detecta la tecla Enter
            {
                e.SuppressKeyPress = true;
                cargaDatosPlaca();
                txtCliente.Focus();
            }
        }

        private void txtPlaca_Enter(object sender, EventArgs e)
        {
            CLEAR();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {

        }

        private void lstServiciosVehiculo_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            int tipoVehiculo = Convert.ToInt32(cmbTipoVehiculo.SelectedValue);

            if (tipoVehiculo > 0)
            {
                var servicioSel = (ServicioListaDTO)lstServiciosVehiculo.Items[e.Index];

                var servicio = servicios.FirstOrDefault(s => s.idServicio == servicioSel.idServicio && s.idTipoVehiculo == tipoVehiculo);


                decimal precio = servicio.precio ?? 0;
                decimal precioBaseComision = servicio.precioBaseComision ?? 0;

                decimal valorActual = 0;
                decimal valorBaseActual = 0;


                decimal.TryParse(txtValor.Text, out valorActual);
                decimal.TryParse(txtValorBase.Text, out valorBaseActual);


                if (e.NewValue == CheckState.Checked)
                {
                    valorActual += precio;
                    valorBaseActual += precioBaseComision;
                }
                else
                {
                    valorActual -= precio;
                    valorBaseActual -= precioBaseComision;
                }

                txtValor.Text = valorActual.ToString("N2");
                txtValorBase.Text = valorBaseActual.ToString("N2");

                precioTotalServicio = valorActual;
                precioBaseTotalServicio = valorBaseActual;
            }
        }

        private void txtValor_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtValor.Text))
            {
                precioTotalServicio = Convert.ToDecimal(txtValor.Text);
            }

        }

        private void txtValorBase_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtValor.Text))
            {
                precioBaseTotalServicio = Convert.ToDecimal(txtValorBase.Text);
            }

        }

        private void TextBox_SelectAll(object sender, EventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void txtValor_MouseClick(object sender, MouseEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void txtValorBase_MouseClick(object sender, MouseEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void txtMarca_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void txtCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private void txtPlaca_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }
    }
}
