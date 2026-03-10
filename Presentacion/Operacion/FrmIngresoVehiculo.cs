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
    public partial class FrmIngresoVehiculo : Form
    {
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
            var servicios = DatabaseQueryLDB.ExecuteList<TipoVehiculo>("select idTipoVehiculo, Nombre from TipoVehiculo  where IsDelete = ?", 0);
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
            cajaDiaria = DatabaseQueryLDB.ExecuteList<CajaDiaria>(
                @"SELECT idCaja,FechaApertura,FechaCierre,MontoInicial,TotalIngresosEfectivo,TotalIngresosTransferencias,TotalIngresosDatafono,TotalEgresos,TotalFinal,Estado
                  FROM CajaDiaria
                  WHERE  Estado = 1;").FirstOrDefault();

        }


        private void CargarListaCompletaServicios()
        {
            // Aquí puedes cargar los combos con datos de la base de datos
            servicios = DatabaseQueryLDB.ExecuteList<ServicioComboDTO>(
               @"SELECT ser.idServicio, ser.Nombre, psr.precio, psr.PrecioBaseComision, tpv.idTipoVehiculo
                  FROM PrecioServicioVehiculo psr 
                  INNER JOIN TipoVehiculo tpv ON psr.IdTipoVehiculo = tpv.idTipoVehiculo 
                  INNER JOIN Servicios ser ON ser.idServicio = psr.idServicio  
                  WHERE ser.IsDelete = ?", 0);

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


            long idTurno = DatabaseQueryLDB.ExecuteInsert(
                    "INSERT INTO Turnos (NumeroTurno,NombreCliente,NumeroCelular,Placa," +
                    " FechaHoraIngreso,Valor,Pagado,Observaciones,IdTipoVehiculo," +
                    " Estado,ValorBaseComision,idCajaDiaria,Marca,NumeroOrden) values (?,?,?,?,?,?,?,?,?,?,?,?,?,?)",
                    GenerarTurno(),
                    txtCliente.Text.Trim(),
                    txtCelular.Text.Trim(),
                    txtPlaca.Text.Trim(),
                    DateTime.Now,
                    precioTotalServicio,
                    false,
                    txtObservaciones.Text.Trim(),
                    Convert.ToInt32(cmbTipoVehiculo.SelectedValue),
                    false,
                    precioBaseTotalServicio,
                    cajaDiaria.idCaja,
                    txtMarca.Text.Trim(),
                    txtNumOrden.Text.Trim()
                );


            List<int> serviciosSeleccionados = new List<int>();

            foreach (var item in lstServiciosVehiculo.CheckedItems)
            {
                var servicio = (ServicioListaDTO)item;

                DatabaseQueryLDB.ExecuteInsert(
                     "INSERT INTO TurnoServicios (idServicios,IdTurno,IsDeleted) values (?,?,?)",
                     servicio.idServicio,
                     idTurno,
                     0);
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
                @"SELECT IdTurno,NombreCliente,NumeroCelular,Placa,IdTipoVehiculo, Estado, Marca FROM Turnos
                  WHERE Placa = ?  ORDER BY FechaHoraIngreso DESC", placa).FirstOrDefault();

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
            var historial = DatabaseQueryLDB.ExecuteList<IngresoVehiculoDTO>(
                @"SELECT TUR.IdTurno,TUR.NumeroTurno, strftime('%Y-%m-%d %H:%M',TUR.FechaHoraIngreso / 10000000 - 62135596800,'unixepoch') AS FechaHoraIngreso, TUR.NombreCliente,TUR.Placa,
                  TVH.Nombre TipoVehiculo, printf('$ %.2f', TUR.Valor) ValorPagado, TUR.Marca
                  FROM Turnos TUR INNER JOIN TipoVehiculo TVH ON TUR.IdTipoVehiculo = TVH.idTipoVehiculo
                  WHERE TUR.Placa = ?
                  ORDER BY TUR.FechaHoraIngreso DESC",
                placa
            );
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
