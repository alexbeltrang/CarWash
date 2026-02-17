using CarWash.Database;
using CarWash.DTOs;
using CarWash.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarWash.Presentacion.Operacion
{
    public partial class FrmIngresoVehiculo : Form
    {
        ServicioComboDTO servicioSeleccionado = new ServicioComboDTO();

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

            panelCard.Left = (this.ClientSize.Width - panelCard.Width) / 2;
            panelCard.Top = (this.ClientSize.Height - panelCard.Height) / 2;

            panelCard.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panelCard.Width, panelCard.Height, 20, 20));
            EstilizarTextBox(txtPlaca);
            EstilizarTextBox(txtCliente);
            EstilizarTextBox(txtValor);
            EstilizarTextBox(txtCelular);
            EstilizarTextBox(txtObservaciones);
            cargarTipoVehiculo();
            txtPlaca.Focus();
        }

        private void cargarTipoVehiculo()
        {
            // Aquí puedes cargar los combos con datos de la base de datos
            var servicios = DatabaseQueryLDB.ExecuteList<TipoVehiculo>("select idTipoVehiculo, Nombre from TipoVehiculo  where IsDelete = ?", 0);
            servicios.Insert(0, new TipoVehiculo
            {
                IdTipoVehiculo = 0,
                Nombre = "-- Seleccione --"
            });
            cmbTipoVehiculo.DataSource = servicios;
            cmbTipoVehiculo.DisplayMember = "Nombre";
            cmbTipoVehiculo.ValueMember = "idTipoVehiculo";
            cmbTipoVehiculo.SelectedIndex = 0;
        }


        private void cargarComboServicio(int TipoVehiculo)
        {
            // Aquí puedes cargar los combos con datos de la base de datos
            var servicios = DatabaseQueryLDB.ExecuteList<ServicioComboDTO>(
                @"SELECT ser.idServicio, ser.Nombre, psr.precio 
                  FROM PrecioServicioVehiculo psr 
                  INNER JOIN TipoVehiculo tpv ON psr.IdTipoVehiculo = tpv.idTipoVehiculo 
                  INNER JOIN Servicios ser ON ser.idServicio = psr.idServicio 
                  WHERE ser.IsDelete = ? and psr.idTipoVehiculo = ?",
                0, TipoVehiculo);

            servicios.Insert(0, new ServicioComboDTO
            {
                idServicio = 0,
                Nombre = "-- Seleccione --",
                precio = 0
            });
            cmbServicio.DataSource = servicios;
            cmbServicio.DisplayMember = "Nombre";
            cmbServicio.ValueMember = "idServicio";
            cmbServicio.SelectedValue = 0;

        }

        private bool ValidarCampos()
        {
            bool valido = true;

            foreach (Control c in panelCard.Controls)
            {
                if (c is TextBox txt && !txt.ReadOnly)
                {
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

            if (cmbServicio.SelectedIndex == -1)
            {
                cmbServicio.BackColor = Color.FromArgb(255, 230, 230);
                valido = false;
            }
            else
            {
                cmbServicio.BackColor = Color.White;
            }

            return valido;
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

            panelCard.Controls.Add(linea);

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
            if (!ValidarCampos())
            {
                MostrarToast("Complete todos los campos obligatorios", Color.FromArgb(220, 53, 69));
                return;
            }
            //Turnos turnos = new Turnos();
            //turnos.NumeroTurno = GenerarTurno();
            //turnos.NombreCliente = txtCliente.Text.Trim();
            //turnos.NumeroCelular = txtCelular.Text.Trim();
            //turnos.Placa = txtPlaca.Text.Trim();
            //turnos.FechaHoraIngreso = DateTime.Now;
            //turnos.Valor = servicioSeleccionado.precio;
            //turnos.Pagado = false;
            //turnos.Observaciones = txtObservaciones.Text.Trim();
            //turnos.IdTipoVehiculo = Convert.ToInt32(cmbTipoVehiculo.SelectedValue);
            //turnos.idServicio = servicioSeleccionado.idServicio;

            var precion = servicioSeleccionado.precio;

            DatabaseQueryLDB.ExecuteNonQuery(
                    "INSERT INTO Turnos (NumeroTurno,NombreCliente,NumeroCelular,Placa,FechaHoraIngreso,Valor,Pagado,Observaciones,IdTipoVehiculo,idServicio,Estado) values (?,?,?,?,?,?,?,?,?,?,?)",
                    GenerarTurno(),
                    txtCliente.Text.Trim(),
                    txtCelular.Text.Trim(),
                    txtPlaca.Text.Trim(),
                    DateTime.Now,
                    servicioSeleccionado.precio,
                    false,
                    txtObservaciones.Text.Trim(),
                    Convert.ToInt32(cmbTipoVehiculo.SelectedValue),
                    servicioSeleccionado.idServicio,
                    true
                );

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
            cmbServicio.SelectedValue = 0;

            foreach (Control c in panelCard.Controls)
            {
                if (c is TextBox txt && !txt.ReadOnly)
                {
                    txt.Clear();
                }
            }

            cmbTipoVehiculo.SelectedIndex = -1;
            cmbServicio.SelectedIndex = -1;
            servicioSeleccionado = null;
            dgvHistorico.DataSource = null;
            txtPlaca.Focus();
        }

        private void cmbTipoVehiculo_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int tipoVehiculo = Convert.ToInt32(cmbTipoVehiculo.SelectedValue);
            cargarComboServicio(tipoVehiculo);

        }

        private void cmbServicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbServicio.SelectedIndex > -1)
            {
                servicioSeleccionado = (ServicioComboDTO)cmbServicio.SelectedItem;
                if (cmbServicio.SelectedItem is ServicioComboDTO servicio)
                {
                    txtValor.Text = servicio.precio.ToString("N2");
                }
            }

        }


        private string GenerarTurno()
        {
            string turno = "";
            string fechaHoy = DateTime.Today.ToString("yyyy-MM-dd");

            // Buscar consecutivo actual
            int? consecutivoActual = DatabaseQueryLDB.ExecuteScalar<int?>(
                "SELECT Consecutivo FROM TurnosDiarios WHERE Fecha = ?",
                fechaHoy
            );

            int nuevoConsecutivo;

            if (!consecutivoActual.HasValue)
            {
                nuevoConsecutivo = 1;

                DatabaseQueryLDB.ExecuteNonQuery(
                    "INSERT INTO TurnosDiarios (Fecha, Consecutivo) VALUES (?, ?)",
                    fechaHoy,
                    nuevoConsecutivo
                );
            }
            else
            {
                // Ya existe → incrementar
                nuevoConsecutivo = consecutivoActual.Value + 1;

                DatabaseQueryLDB.ExecuteNonQuery(
                    "UPDATE TurnosDiarios SET Consecutivo = ? WHERE Fecha = ?",
                    nuevoConsecutivo,
                    fechaHoy
                );
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
            var vehiculo = DatabaseQueryLDB.ExecuteList<Turnos>(
                @"SELECT IdTurno,NombreCliente,NumeroCelular,Placa,IdTipoVehiculo, Estado FROM Turnos
                  WHERE Placa = ?  ORDER BY FechaHoraIngreso DESC", placa).FirstOrDefault();

            if (vehiculo != null)
            {
                if (!vehiculo.Estado)
                {
                    // Cargar datos en los controles
                    cmbTipoVehiculo.SelectedValue = vehiculo.IdTipoVehiculo;
                    cargarComboServicio(vehiculo.IdTipoVehiculo);
                    txtCliente.Text = vehiculo.NombreCliente;
                    txtCelular.Text = vehiculo.NumeroCelular;
                }
                //else
                //{
                //    MessageBox.Show("Vehículo ya tiene un ingreso activo.");
                //    CLEAR();
                //}
            }
            else
            {
                // Limpiar si no existe
                cmbTipoVehiculo.SelectedIndex = -1;
                cmbServicio.DataSource = null;
                txtValor.Clear();
            }
        }

        private void CargarHistorico(string placa)
        {
            var historial = DatabaseQueryLDB.ExecuteList<IngresoVehiculoDTO>(
                @"SELECT TUR.IdTurno,TUR.NumeroTurno, strftime('%Y-%m-%d %H:%M',TUR.FechaHoraIngreso / 10000000 - 62135596800,'unixepoch') AS FechaHoraIngreso, TUR.NombreCliente,TUR.Placa,TVH.Nombre TipoVehiculo, SER.Nombre Servicio, printf('$ %.2f', TUR.Valor) Valor
                  FROM Turnos TUR INNER JOIN TipoVehiculo TVH ON TUR.IdTipoVehiculo = TVH.idTipoVehiculo
                  INNER JOIN Servicios SER ON SER.idServicio = TUR.idServicio
                  WHERE TUR.Placa = ?
                  ORDER BY TUR.FechaHoraIngreso DESC",
                placa
            );
            dgvHistorico.DataSource = historial;
            dgvHistorico.Columns["Valor"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvHistorico.Columns["IdTurno"].Visible = false;

            dgvHistorico.Columns["NumeroTurno"].HeaderText = "N° Turno";
            dgvHistorico.Columns["FechaHoraIngreso"].HeaderText = "Fecha Servicio";
            dgvHistorico.Columns["NombreCliente"].HeaderText = "Nombre Cliente";
            dgvHistorico.Columns["Placa"].HeaderText = "Placa";
            dgvHistorico.Columns["TipoVehiculo"].HeaderText = "Tipo Vehiculo";
            dgvHistorico.Columns["Servicio"].HeaderText = "Servicio";
            dgvHistorico.Columns["Valor"].HeaderText = "Valor ($)";

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
    }
}
