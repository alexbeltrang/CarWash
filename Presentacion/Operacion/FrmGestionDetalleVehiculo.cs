using CarWash.Controladores;
using CarWash.Database;
using CarWash.DTOs;
using CarWash.Entidades;
using CarWash.Enum;
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
using FormaPago = CarWash.Enum.FormaPago;
using CancellationTokenSource = System.Threading.CancellationTokenSource;


namespace CarWash.Presentacion.Operacion
{
    public partial class FrmGestionDetalleVehiculo : Form
    {
        private int _idTurno;
        decimal valorPagar = 0;
        decimal valorPagarTotal = 0;
        List<GestionVehiculosDTO> vehiculosProceso = new List<GestionVehiculosDTO>();
        private readonly CajaComandosClient comandos = new CajaComandosClient();
        private readonly CancellationTokenSource lifetime = new CancellationTokenSource();
        private CajaResumenDTO? resumen;
        private CajaSaldoDTO? saldo;
        private CajaCobroDTO? pendiente;
        private CajaOperarioDTO? operacionOperario;
        private List<CajaMedioDTO> medios = new List<CajaMedioDTO>();
        private bool ocupado;
        private bool terminado;
        private bool facturaPendiente;
        private CajaResultadoDTO? ultimoCobro;
        TurnosController turnosController = new TurnosController();
        ServiciosController serviciosController = new ServiciosController();
        ClienteCreditoController clienteCreditoController = new ClienteCreditoController();

        public FrmGestionDetalleVehiculo(int idTurno)
        {
            InitializeComponent();
            _idTurno = idTurno;
        }

        private async void FrmGestionDetalleVehiculo_Load(object sender, EventArgs e)
        {
            if (UIHelper.EnModoDiseno(this) || terminado || pendiente != null || operacionOperario != null) return;
            cmbClienteCredito.Visible = false;
            this.Region = System.Drawing.Region.FromHrgn(
            CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            cargarDetalleTurnoVehiculo(_idTurno);
            cargaClienteCredito();
            await CargarFinanzasAsync(true);
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
            (
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
            );

        private void FrmGestionDetalleVehiculo_FormClosed(object sender, FormClosedEventArgs e)
        {
            cerrarForm();
        }

        private void cargaClienteCredito()
        {
            try
            {

                var clienteCredito = clienteCreditoController.ClienteCreditoActivos();

                clienteCredito.Insert(0, new ClienteCredito
                {
                    idClienteCredito = 0,
                    Nombre = "-- Seleccione --"
                });

                cmbClienteCredito.DataSource = clienteCredito;
                cmbClienteCredito.DisplayMember = "Nombre";
                cmbClienteCredito.ValueMember = "idClienteCredito";
                cmbClienteCredito.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando cliente crédito: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async Task CargarFinanzasAsync(bool ajustarImporte)
        {
            if (ocupado || pendiente != null || operacionOperario != null || terminado || IsDisposed) return;
            ocupado = true;
            resumen = null;
            ActualizarControles();
            try
            {
                var active = await comandos.ActivaAsync(lifetime.Token);
                if (IsDisposed) return;
                if (active == null) { estadoCobro.Text = "No hay caja abierta."; return; }
                var loaded = await comandos.ResumenAsync(active.idCaja, lifetime.Token);
                var balance = await comandos.SaldoAsync(_idTurno, lifetime.Token);
                var media = await comandos.MediosAsync(lifetime.Token);
                if (IsDisposed) return;
                resumen = loaded;
                saldo = balance;
                medios = media;
                valorPagarTotal = balance.SaldoPendiente;
                if (ajustarImporte) txtValorPagar.Text = valorPagarTotal.ToString("N2");
                estadoCobro.Text = "Saldo verificado: " + valorPagarTotal.ToString("N2");
            }
            catch (Exception error) when (error is System.IO.IOException || error is System.Net.Http.HttpRequestException || error is OperationCanceledException || error is UnauthorizedAccessException)
            { if (!IsDisposed) estadoCobro.Text = "No se pudo verificar caja y saldo. Consulte nuevamente."; }
            finally { ocupado = false; if (!IsDisposed) ActualizarControles(); }
        }

        private void ActualizarControles()
        {
            var editable = !ocupado && pendiente == null && operacionOperario == null && !terminado;
            txtValorPagar.Enabled = txtObservaciones.Enabled = cmbClienteCredito.Enabled = editable;
            foreach (var item in new[] { (rbEfectivo, "EFECTIVO"), (rbTransferencia, "TRANSFERENCIA"), (rbDatafono, "DATAFONO"), (rbCredito, "CREDITO") })
                item.Item1.Enabled = editable && medios.Count(row => CajaComandosClient.Categoria(row.Nombre) == item.Item2) == 1;
            btnFinalizar.Enabled = !ocupado && !terminado && operacionOperario == null && resumen != null;
            btnFinalizar.Text = pendiente == null ? "Registrar pago" : "Consultar pago";
            btnAsignarOperador.Enabled = !ocupado && pendiente == null && !terminado && resumen != null && saldo?.IdCajaOrigen == resumen.IdCaja;
            btnAsignarOperador.Text = operacionOperario == null ? "Asignar operador" : "Consultar operacion";
            btnLiberarOperador.Enabled = editable && resumen != null && saldo?.IdCajaOrigen == resumen.IdCaja;
            btnConsultarCaja.Enabled = editable;
        }

        private async void btnConsultarCaja_Click(object sender, EventArgs e) => await CargarFinanzasAsync(false);

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if ((ocupado || pendiente != null || operacionOperario != null) && e.CloseReason == CloseReason.UserClosing)
            { e.Cancel = true; estadoCobro.Text = "Operacion pendiente. Consulte el UUID: " + (pendiente?.IdOperacion ?? operacionOperario?.IdOperacion); }
            base.OnFormClosing(e);
        }

        private void cerrarForm()
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void cargarDetalleTurnoVehiculo(int idTurno)
        {
            try
            {
                vehiculosProceso = turnosController.VehiculoEnProceso(idTurno);


                if (vehiculosProceso != null && vehiculosProceso.Count > 0)
                {
                    lblTurno.Text = "Turno N°: " + vehiculosProceso[0].NumeroTurno;
                    lblPlaca.Text = "Placa: " + vehiculosProceso[0].Placa;
                    lblCliente.Text = "Cliente: " + vehiculosProceso[0].NombreCliente;
                    lblCelular.Text = "Número Celular: " + vehiculosProceso[0].NumeroCelular;
                    lblFechaIngreso.Text = "Fecha Hora Ingreso: " + vehiculosProceso[0].FechaHoraIngreso;
                    lblTipoVehiculo.Text = "Tipo Vehículo: " + vehiculosProceso[0].TipoVehiculo;
                    if (saldo == null) txtObservaciones.Text = vehiculosProceso[0].Observaciones;

                    if (vehiculosProceso[0].idOperario.HasValue)
                    {
                        lblOperadorAsignado.Text = "Operador Asignado: " + vehiculosProceso[0].OperadorAsignado;
                    }
                    else
                    {
                        lblEstado.Text = "Estado: Sin Operador Asignado";
                        lblOperadorAsignado.Text = "Operador Asignado: SIN ASIGNAR";
                        lblEstado.ForeColor = Color.White;
                        lblEstado.Location = new Point(500, 20);
                    }
                    if (vehiculosProceso[0].idOperario.HasValue && !vehiculosProceso[0].Estado)
                    {
                        lblEstado.Text = "Estado: En Proceso";
                        lblEstado.ForeColor = Color.White;
                        lblEstado.Location = new Point(627, 20);
                    }
                    else if (vehiculosProceso[0].idOperario.HasValue)
                    {
                        lblEstado.Text = "Estado: Finalizado";
                        lblEstado.ForeColor = Color.Green;
                        lblEstado.Location = new Point(627, 20);
                    }
                    cargaServiciosContratados(idTurno);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando vehículos en proceso: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void cargaServiciosContratados(int idTurno)
        {
            var serviciosAdquiridos = serviciosController.ServiciosPorTurno(idTurno);

            lstServiciosAdquiridos.DataSource = serviciosAdquiridos;
            lstServiciosAdquiridos.DisplayMember = "Nombre";
            lstServiciosAdquiridos.ValueMember = "IdServicio";
            lstServiciosAdquiridos.ClearSelected();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnAsignarOperador_Click(object sender, EventArgs e)
        {
            if (ocupado) return;
            if (operacionOperario != null) { await GuardarOperarioAsync(null, false); return; }
            if (validarAsignacionOperador())
            {
                using (FrmAsignarOperador frm = new FrmAsignarOperador(vehiculosProceso[0].Placa, vehiculosProceso[0].NombreCliente))
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    if (!frm.OperadorSeleccionadoId.HasValue)
                    {
                        MessageBox.Show("No se seleccionó un operador válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    await GuardarOperarioAsync(frm.OperadorSeleccionadoId.Value, false);
                }
            }
        }

        private async Task GuardarOperarioAsync(int? idOperario, bool liberar)
        {
            if (ocupado || resumen == null || pendiente != null) return;
            operacionOperario = operacionOperario ?? new CajaOperarioDTO { IdCaja = resumen.IdCaja, RevisionEsperada = resumen.Revision,
                IdOperario = idOperario, Liberar = liberar };
            ocupado = true;
            ActualizarControles();
            try
            {
                var result = await comandos.OperarioAsync(_idTurno, operacionOperario, lifetime.Token);
                if (IsDisposed) return;
                if ((int?)result.Resultado["idTurno"] != _idTurno) throw new System.IO.IOException();
                resumen.Revision = result.Revisiones[resumen.IdCaja];
                operacionOperario = null;
                estadoCobro.Text = "Cambio de operador confirmado.";
                cargarDetalleTurnoVehiculo(_idTurno);
            }
            catch (CajaHttpException error) when (error.EscrituraRechazada)
            { if (!IsDisposed) { operacionOperario = null; resumen = null; estadoCobro.Text = error.Message + " Consulte nuevamente."; } }
            catch (Exception error) when (error is System.IO.IOException || error is System.Net.Http.HttpRequestException || error is OperationCanceledException || error is UnauthorizedAccessException)
            { if (!IsDisposed) estadoCobro.Text = "Resultado desconocido. UUID: " + operacionOperario?.IdOperacion; }
            finally { ocupado = false; if (!IsDisposed) ActualizarControles(); }
        }

        private Turnos obtenerTurnoParaGestion()
        {
            var turno = turnosController.consultaTurnoId(vehiculosProceso[0].IdTurno);

            // BuscarByPlaca devuelve solo datos parciales; actualizarlo sobrescribiria el turno completo.
            if (turno != null && turno.IdTurno > 0 && turno.FechaHoraIngreso != default(DateTime))
                return turno;

            return null;
        }




        private bool validarAsignacionOperador()
        {
            if (vehiculosProceso[0].idOperario.HasValue && vehiculosProceso[0].idOperario > 0)
            {
                return MessageBox.Show(
                    "El turno ya tiene un operador asignado. ¿Desea cambiarlo?",
                    "Confirmación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2
                ) == DialogResult.Yes;
            }

            return true;

        }

        private async void btnFinalizar_Click(object sender, EventArgs e)
        {
            var answer = MessageBox.Show(this, "Confirmar importe y medio del pago?", "Cobro", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            await CobrarAsync(answer == DialogResult.Yes);
            if (!facturaPendiente || IsDisposed) return;
            facturaPendiente = false;
            try
            {
                FacturarCobroConfirmado(this, MessageBox.Show, () => { using (var factura = new FrmFactura(_idTurno, true)) factura.ShowDialog(this); });
            }
            catch (Exception) { MessageBox.Show("El cobro finalizo. No se pudo abrir la factura; consultela desde Facturas.", "Factura", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            finally { cerrarForm(); }
        }

        private async Task CobrarAsync(bool confirmacion)
        {
            if (!confirmacion || ocupado || terminado || resumen == null || saldo == null || operacionOperario != null) return;
            if (pendiente == null && !validarTurno()) return;
            var category = rbEfectivo.Checked ? "EFECTIVO" : rbTransferencia.Checked ? "TRANSFERENCIA" : rbDatafono.Checked ? "DATAFONO" : rbCredito.Checked ? "CREDITO" : "";
            var medium = medios.Where(row => CajaComandosClient.Categoria(row.Nombre) == category).ToList();
            if (pendiente == null && medium.Count != 1) { estadoCobro.Text = "Medio de pago no verificable."; return; }
            pendiente = pendiente ?? new CajaCobroDTO { IdCaja = resumen.IdCaja, RevisionEsperada = resumen.Revision,
                Importe = CajaComandosClient.Dinero(valorPagar), IdFormaPago = medium[0].IdFormaPago,
                Finalizar = !saldo.Finalizado && valorPagar == saldo.SaldoPendiente,
                IdClienteCredito = category == "CREDITO" ? (int?)Convert.ToInt32(cmbClienteCredito.SelectedValue) : null, Observaciones = txtObservaciones.Text.Trim() };
            ocupado = true;
            ActualizarControles();
            try
            {
                var result = await comandos.CobrarAsync(_idTurno, pendiente, lifetime.Token);
                if (IsDisposed) return;
                if ((int?)result.Resultado["idTurno"] != _idTurno || result.Resultado["saldoPendiente"] == null
                    || result.Resultado["finalizado"] == null || result.Resultado["pagado"] == null) throw new System.IO.IOException();
                var remaining = (decimal)result.Resultado["saldoPendiente"]!;
                var final = (bool)result.Resultado["finalizado"]! && (bool)result.Resultado["pagado"]!;
                facturaPendiente = pendiente.Finalizar && !saldo.Finalizado && final;
                terminado = final;
                ultimoCobro = result;
                pendiente = null;
                saldo.SaldoPendiente = remaining;
                saldo.Finalizado = final;
                resumen.Revision = result.Revisiones[resumen.IdCaja];
                valorPagarTotal = remaining;
                if (!final) txtValorPagar.Text = remaining.ToString("N2");
                if (final) { lblEstado.Text = "Estado: Finalizado"; lblEstado.ForeColor = Color.Green; }
                estadoCobro.Text = final ? "Pago final confirmado." : "Pago parcial confirmado. Saldo: " + remaining.ToString("N2");
            }
            catch (CajaHttpException error) when (error.EscrituraRechazada)
            { if (!IsDisposed) { pendiente = null; resumen = null; estadoCobro.Text = error.Message + " Consulte nuevamente."; } }
            catch (Exception error) when (error is System.IO.IOException || error is System.Net.Http.HttpRequestException || error is OperationCanceledException || error is UnauthorizedAccessException)
            { if (!IsDisposed) estadoCobro.Text = "Resultado desconocido. Conserve los datos. UUID: " + pendiente?.IdOperacion; }
            finally { ocupado = false; if (!IsDisposed) ActualizarControles(); }
        }


        private static void FacturarCobroConfirmado(IWin32Window owner,
            Func<IWin32Window, string, string, MessageBoxButtons, MessageBoxIcon, MessageBoxDefaultButton, DialogResult> preguntar,
            Action abrirFactura)
        {
            if (preguntar(owner, "¿Desea generar la factura?", "Factura", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes) abrirFactura();
        }

        private bool validarTurno()
        {
            bool esValido = true;
            if (vehiculosProceso.Count == 0 || vehiculosProceso[0].idOperario == null)
            {
                MessageBox.Show("Debe asignar un operador antes de finalizar el turno.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                esValido = false;
            }
            if (!decimal.TryParse(txtValorPagar.Text, out valorPagar) || valorPagar <= 0 || decimal.Round(valorPagar, 2) != valorPagar || valorPagar > saldo?.SaldoPendiente)
            {
                MessageBox.Show("El valor a pagar es obligatorio.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                esValido = false;
            }

            if (string.IsNullOrWhiteSpace(txtObservaciones.Text) && txtObservaciones.Tag?.ToString() != "opcional")
            {
                MessageBox.Show("Las observaciones son obligatorias.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                esValido = false;
            }
            if (resumen == null || resumen.Estado != "ABIERTA")
            {
                MessageBox.Show("No hay apertura de caja para el día de hoy.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                esValido = false;
            }
            if (rbCredito.Checked && (cmbClienteCredito.SelectedIndex == 0 || cmbClienteCredito.SelectedValue == null))
            {
                MessageBox.Show("Debe seleccionar un cliente para crédito.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                esValido = false;
            }
            return esValido;
        }

        private async void btnLiberarOperador_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Confirmar liberacion del operador?", "Operador", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes) await GuardarOperarioAsync(null, true);
        }


        private void FormaPago_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb == null || !rb.Checked)
                return;

            // Resetear todos primero
            ResetRadioButtons();

            // Aplicar color según el seleccionado
            if (rb == rbEfectivo)
            {
                rb.BackColor = Color.FromArgb(40, 167, 69);
                rb.ForeColor = Color.White;
                activaClienteCredito(false);

            }
            else if (rb == rbTransferencia)
            {
                rb.BackColor = Color.FromArgb(0, 123, 255);
                rb.ForeColor = Color.White;
                activaClienteCredito(false);

            }
            else if (rb == rbDatafono)
            {
                rb.BackColor = Color.FromArgb(255, 140, 0);
                rb.ForeColor = Color.White;
                activaClienteCredito(false);

            }
            else if (rb == rbCredito)
            {
                rb.BackColor = Color.FromArgb(255, 140, 0);
                rb.ForeColor = Color.White;
                activaClienteCredito(true);
                cmbClienteCredito.SelectedIndex = 0;
            }
        }


        private void activaClienteCredito(bool estado)
        {
            cmbClienteCredito.Visible = estado;
            lblClienteCredito.Visible = estado;
        }
        private void ResetRadioButtons()
        {
            rbEfectivo.BackColor = Color.WhiteSmoke;
            rbTransferencia.BackColor = Color.WhiteSmoke;
            rbDatafono.BackColor = Color.WhiteSmoke;
            rbCredito.BackColor = Color.WhiteSmoke;

            rbEfectivo.ForeColor = Color.Black;
            rbTransferencia.ForeColor = Color.Black;
            rbDatafono.ForeColor = Color.Black;
            rbCredito.ForeColor = Color.Black;
        }

        private void txtValorPagar_Leave(object sender, EventArgs e)
        {
            decimal.TryParse(txtValorPagar.Text, out valorPagar);
        }

        private void txtValorPagar_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = sender as TextBox;

            // Permitir teclas de control (backspace, delete, tab, etc.)
            if (char.IsControl(e.KeyChar))
                return;

            // Permitir solo dígitos
            if (char.IsDigit(e.KeyChar))
                return;

            // Permitir un solo punto decimal
            if (e.KeyChar == '.' && !tb.Text.Contains("."))
                return;

            // Si llega aquí, se bloquea la tecla
            e.Handled = true;
        }

        private void txtValorPagar_KeyDown(object sender, KeyEventArgs e)
        {
            // Detecta Ctrl+V (pegar)
            if (e.Control && e.KeyCode == Keys.V)
            {
                e.SuppressKeyPress = true; // Bloquea el pegado
            }
        }

        private void txtValorPagar_TextChanged(object sender, EventArgs e)
        {
            decimal.TryParse(txtValorPagar.Text, out valorPagar);
        }
    }
}
