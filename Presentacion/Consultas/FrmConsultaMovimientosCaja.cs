using CarWash.Controladores;
using CarWash.Database;
using CarWash.DTOs;
using CarWash.Entidades;
using CarWash.Utilidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace CarWash.Presentacion.Consultas
{
    public partial class FrmConsultaMovimientosCaja : Form
    {
        private readonly CajaComandosClient comandos = new CajaComandosClient();
        private readonly CancellationTokenSource vida = new CancellationTokenSource();
        private CajaImpactoDTO? impacto;
        private CajaMovimientoDTO? seleccionado;
        private CajaComandoDTO? pendiente;
        private int objetivoPendiente;
        private bool turnoPendiente;
        private bool ocupado;
        private CajaResultadoDTO? resultadoConfirmado;
        private CajaCierreVersionDTO? cierreSeleccionado;
        private bool detalleCierreVigente;

        decimal precioTotalServicio = 0;
        decimal precioBaseTotalServicio = 0;

        TurnosMovimientosController turnosMovimientosController = new TurnosMovimientosController();
        FormaPagoController formaPagoController = new FormaPagoController();
        OperariosController operariosController = new OperariosController();

        CajaDiaria cajaDiaria = new CajaDiaria();
        public FrmConsultaMovimientosCaja()
        {
            InitializeComponent();
            if (UIHelper.EnModoDiseno(this)) return;
            accionesAdmin.Visible = clsConnection.EsAdministrador;
            dtgMovimientos.MultiSelect = false;
            dtgMovimientos.SelectionChanged += (sender, args) =>
            {
                if (pendiente != null || ocupado) return;
                cierreSeleccionado = dtgMovimientos.SelectedRows.Count == 1 ? dtgMovimientos.SelectedRows[0].DataBoundItem as CajaCierreVersionDTO : null;
                detalleCierreVigente = false;
                var next = dtgMovimientos.SelectedRows.Count == 1 ? dtgMovimientos.SelectedRows[0].DataBoundItem as CajaMovimientoDTO : null;
                if (next?.Id == seleccionado?.Id && cierreSeleccionado == null) return;
                seleccionado = next;
                impacto = null;
                impactoDetalle.Clear();
                if (cierreSeleccionado != null) MostrarCierre();
                if (seleccionado?.Importe != null)
                {
                    importeCorreccion.Value = Math.Min(importeCorreccion.Maximum, seleccionado.Importe.Value);
                    medioCorreccion.SelectedValue = seleccionado.IdFormaPago;
                }
                EstadoControles();
            };
            alcance.SelectedIndex = 0;
            alcance.SelectedIndexChanged += (sender, args) => { impacto = null; impactoDetalle.Clear(); EstadoControles(); };
            if (!clsConnection.EsAdministrador)
            {
                EstadoControles();
                Shown += (sender, args) =>
                {
                    MessageBox.Show("Solo el perfil Administrador puede consultar ventas.", "Sin permiso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Close();
                };
                return;
            }
            cargarFormasPago();
            CargaOperarios();
            EstadoControles();
        }

        private void EstadoControles()
        {
            if (IsDisposed || Disposing) return;
            var editable = !ocupado && pendiente == null && clsConnection.EsAdministrador;
            cajaConsulta.Enabled = paginaConsulta.Enabled = cargarCaja.Enabled = btnBuscar.Enabled = dtgMovimientos.Enabled = alcance.Enabled = editable;
            dtpFechaInicial.Enabled = dtpFechaFinal.Enabled = cargarCierres.Enabled = editable;
            verCierre.Enabled = editable && cierreSeleccionado != null;
            reabrir.Enabled = !ocupado && clsConnection.EsAdministrador && (pendiente is CajaReaperturaDTO || editable && detalleCierreVigente && cierreSeleccionado?.Vigente == true && !cierreSeleccionado.EnRevision);
            recerrar.Enabled = editable && detalleCierreVigente && cierreSeleccionado?.Vigente == true && cierreSeleccionado.EnRevision;
            reabrir.Text = pendiente is CajaReaperturaDTO ? "Consultar reapertura" : "Reabrir en revision";
            importeCorreccion.Enabled = medioCorreccion.Enabled = motivoAdmin.Enabled = editable;
            verImpacto.Enabled = editable && seleccionado != null;
            corregir.Enabled = !ocupado && clsConnection.EsAdministrador && (pendiente is CajaCorreccionCobroDTO || pendiente == null && impacto != null && alcance.SelectedIndex == 0 && impacto.Bloqueos.Count == 0);
            borrar.Enabled = !ocupado && clsConnection.EsAdministrador && (pendiente is CajaBorradoDTO || pendiente == null && impacto != null && impacto.Bloqueos.Count == 0);
        }

        private async Task CargarCierresAsync()
        {
            if (ocupado || pendiente != null || !clsConnection.EsAdministrador) return;
            ocupado = true;
            EstadoControles();
            try
            {
                var page = await comandos.CierresAsync(dtpFechaInicial.Value, dtpFechaFinal.Value, (int)paginaConsulta.Value, vida.Token);
                if (IsDisposed || Disposing) return;
                cierreSeleccionado = null;
                detalleCierreVigente = false;
                seleccionado = null;
                impacto = null;
                impactoDetalle.Clear();
                dtgMovimientos.DataSource = page.Items;
                dtgMovimientos.ClearSelection();
                estadoAdmin.Text = page.TotalCount + " versiones de cierre. Pagina " + page.Pagina + ".";
            }
            catch (Exception error) { if (!IsDisposed && !Disposing) estadoAdmin.Text = "Historico no disponible: " + error.Message; }
            finally { ocupado = false; EstadoControles(); }
        }

        private void MostrarCierre()
        {
            if (cierreSeleccionado == null) return;
            var row = cierreSeleccionado;
            impactoDetalle.Text = "Jornada #" + row.IdCaja + " / Version " + row.Version + (row.Vigente ? " vigente" : " reemplazada")
                + (row.EnRevision ? " / EN REVISION" : "") + Environment.NewLine
                + (row.Origen == "LEGADO_NO_VERIFICADO" ? "LEGADO NO VERIFICADO" : "VERIFICADO") + Environment.NewLine
                + "Responsable: " + (row.Responsable ?? "Desconocido") + " / " + row.ConfirmadoEnUtc?.ToString("g") + Environment.NewLine
                + "Contado: " + (row.EfectivoContado ?? "Desconocido") + " / Diferencia: " + (row.Diferencia ?? "Desconocida") + Environment.NewLine
                + "Motivo revision: " + row.MotivoRevision + " / Justificacion: " + row.JustificacionDiferencia + Environment.NewLine + row.Snapshot.ToString();
            estadoAdmin.Text = row.EnRevision ? "Revision historica abierta. Solo correcciones de existentes." : "Version historica de solo lectura.";
        }

        private async Task DetalleCierreAsync()
        {
            if (ocupado || pendiente != null || !clsConnection.EsAdministrador || cierreSeleccionado == null) return;
            ocupado = true;
            detalleCierreVigente = false;
            EstadoControles();
            try
            {
                var detail = await comandos.CierreAsync(cierreSeleccionado.IdCaja, cierreSeleccionado.Version, vida.Token);
                if (IsDisposed || Disposing) return;
                cierreSeleccionado = detail;
                detalleCierreVigente = true;
                MostrarCierre();
            }
            catch (Exception error) { if (!IsDisposed && !Disposing) estadoAdmin.Text = "Version no disponible: " + error.Message; }
            finally { ocupado = false; EstadoControles(); }
        }

        private async Task ReabrirAsync(bool confirmado)
        {
            if (!confirmado || ocupado || !clsConnection.EsAdministrador || pendiente != null && !(pendiente is CajaReaperturaDTO)) return;
            if (pendiente == null)
            {
                if (!detalleCierreVigente || cierreSeleccionado?.Vigente != true || cierreSeleccionado.EnRevision || string.IsNullOrWhiteSpace(motivoAdmin.Text)) return;
                objetivoPendiente = cierreSeleccionado.IdCaja;
                pendiente = new CajaReaperturaDTO { RevisionEsperada = cierreSeleccionado.Revision, VersionEsperada = cierreSeleccionado.Version, Motivo = motivoAdmin.Text.Trim() };
            }
            ocupado = true;
            EstadoControles();
            try
            {
                var result = await comandos.ReabrirAsync(objetivoPendiente, (CajaReaperturaDTO)pendiente, vida.Token);
                if (IsDisposed || Disposing) return;
                resultadoConfirmado = result;
                pendiente = null;
                detalleCierreVigente = false;
                cierreSeleccionado = await comandos.CierreAsync(objetivoPendiente, (int)result.Resultado["version"]!, vida.Token);
                if (IsDisposed || Disposing) return;
                detalleCierreVigente = true;
                MostrarCierre();
            }
            catch (Exception error)
            {
                if (IsDisposed || Disposing) return;
                if ((error as CajaHttpException)?.EscrituraRechazada == true) { pendiente = null; detalleCierreVigente = false; }
                estadoAdmin.Text = (pendiente == null ? "Consulte nuevamente la version. " : "Resultado pendiente. UUID " + pendiente.IdOperacion + ". ") + error.Message;
            }
            finally { ocupado = false; EstadoControles(); }
        }

        private async void cargarCierres_Click(object sender, EventArgs e) => await CargarCierresAsync();
        private async void verCierre_Click(object sender, EventArgs e) => await DetalleCierreAsync();
        private async void reabrir_Click(object sender, EventArgs e) => await ReabrirAsync(MessageBox.Show(this,
            "Reabrir en revision historica la jornada #" + (pendiente is CajaReaperturaDTO ? objetivoPendiente : cierreSeleccionado?.IdCaja) + "? La caja operativa no cambiara."
            + Environment.NewLine + "Motivo: " + motivoAdmin.Text, "Revision administrativa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes);
        private async void recerrar_Click(object sender, EventArgs e)
        {
            if (ocupado || pendiente != null || !detalleCierreVigente || cierreSeleccionado?.EnRevision != true || !clsConnection.EsAdministrador) return;
            using (var form = new CarWash.Presentacion.Administracion.FrmCierreCaja(cierreSeleccionado.IdCaja)) form.ShowDialog(this);
            await DetalleCierreAsync();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if ((ocupado || pendiente != null) && e.CloseReason == CloseReason.UserClosing)
            { e.Cancel = true; estadoAdmin.Text = "Operacion pendiente. Consulte el UUID antes de salir: " + pendiente?.IdOperacion; }
            base.OnFormClosing(e);
        }

        private async Task CargarCajaAsync()
        {
            if (ocupado || pendiente != null || !clsConnection.EsAdministrador) return;
            ocupado = true;
            EstadoControles();
            try
            {
                var page = await comandos.MovimientosAsync((int)cajaConsulta.Value, (int)paginaConsulta.Value, vida.Token);
                if (IsDisposed || Disposing) return;
                impacto = null;
                seleccionado = null;
                cierreSeleccionado = null;
                detalleCierreVigente = false;
                impactoDetalle.Clear();
                dtgMovimientos.DataSource = page.Items;
                dtgMovimientos.ClearSelection();
                estadoAdmin.Text = page.TotalCount + " movimientos. Pagina " + page.Pagina + ".";
                if (page.FuentesInvalidas.Count > 0) estadoAdmin.Text = "Fuentes no verificables: " + string.Join(", ", page.FuentesInvalidas.Select(row => row.Fuente + " #" + row.Id + ": " + row.Codigo));
            }
            catch (Exception error) { if (!IsDisposed && !Disposing) estadoAdmin.Text = "No se pudo consultar: " + error.Message; }
            finally { ocupado = false; EstadoControles(); }
        }

        private async Task PrevisualizarAsync()
        {
            if (ocupado || pendiente != null || !clsConnection.EsAdministrador) return;
            seleccionado = dtgMovimientos.SelectedRows.Count == 1 ? dtgMovimientos.SelectedRows[0].DataBoundItem as CajaMovimientoDTO : null;
            if (seleccionado == null) return;
            ocupado = true;
            impacto = null;
            EstadoControles();
            try
            {
                var turn = alcance.SelectedIndex == 1;
                var preview = await comandos.ImpactoAsync(turn ? seleccionado.IdTurno : seleccionado.Id, turn, vida.Token);
                if (IsDisposed || Disposing) return;
                impacto = preview;
                if (!turn && seleccionado.Importe.HasValue)
                {
                    importeCorreccion.Value = Math.Min(importeCorreccion.Maximum, seleccionado.Importe.Value);
                    medioCorreccion.SelectedValue = seleccionado.IdFormaPago;
                }
                impactoDetalle.Text = (turn ? "Turno #" + seleccionado.IdTurno : "Cobro #" + seleccionado.Id) + Environment.NewLine
                    + string.Join("; ", preview.Cajas.Select(row => "Caja #" + row.IdCaja + " rev " + row.Revision + " version " + row.VersionVigente)) + Environment.NewLine
                    + string.Join("; ", preview.Identificadores.Select(row => row.Key + ": " + string.Join(", ", row.Value))) + Environment.NewLine
                    + string.Join("; ", preview.Movimientos.Select(row => "#" + row.Id + " " + row.Importe + " " + row.Categoria + " caja #" + row.IdCaja))
                    + Environment.NewLine + "Trabajos: " + string.Join(", ", preview.Trabajos);
                estadoAdmin.Text = preview.Bloqueos.Count == 0 ? "Impacto vigente." : string.Join("; ", preview.Bloqueos.Select(row => row.Fuente + " #" + row.Id + ": " + row.Codigo));
            }
            catch (Exception error) { if (!IsDisposed && !Disposing) estadoAdmin.Text = "Impacto no disponible: " + error.Message; }
            finally { ocupado = false; EstadoControles(); }
        }

        private async Task AplicarAsync(bool eliminar, bool confirmado)
        {
            if (!confirmado || ocupado || !clsConnection.EsAdministrador || pendiente is CajaReaperturaDTO) return;
            if (pendiente == null)
            {
                if (impacto == null || seleccionado == null || impacto.Bloqueos.Count != 0 || motivoAdmin.Text.Trim().Length == 0)
                { estadoAdmin.Text = "Impacto vigente y motivo requeridos."; return; }
                turnoPendiente = alcance.SelectedIndex == 1;
                if (!eliminar && turnoPendiente) return;
                objetivoPendiente = turnoPendiente ? seleccionado.IdTurno : seleccionado.Id;
                var revisions = impacto.Cajas.ToDictionary(row => row.IdCaja, row => row.Revision);
                pendiente = eliminar ? (CajaComandoDTO)new CajaBorradoDTO
                { RevisionesEsperadas = revisions, ImpactoToken = impacto.ImpactoToken, Motivo = motivoAdmin.Text.Trim() }
                : new CajaCorreccionCobroDTO
                {
                    RevisionEsperada = revisions[seleccionado.IdCaja!.Value], RevisionesEsperadas = revisions,
                    ImpactoToken = impacto.ImpactoToken, Importe = CajaComandosClient.Dinero(importeCorreccion.Value),
                    IdFormaPago = Convert.ToInt32(medioCorreccion.SelectedValue), Motivo = motivoAdmin.Text.Trim()
                };
            }
            if (eliminar != (pendiente is CajaBorradoDTO)) return;
            ocupado = true;
            EstadoControles();
            try
            {
                var result = pendiente is CajaBorradoDTO deletion
                    ? await comandos.BorrarAsync(objetivoPendiente, turnoPendiente, deletion, vida.Token)
                    : await comandos.CorregirCobroAsync(objetivoPendiente, (CajaCorreccionCobroDTO)pendiente, vida.Token);
                if (IsDisposed || Disposing) return;
                resultadoConfirmado = result;
                pendiente = null;
                impacto = null;
                ocupado = false;
                await CargarCajaAsync();
                var summary = await comandos.ResumenAsync((int)cajaConsulta.Value, vida.Token);
                if (!IsDisposed && !Disposing) estadoAdmin.Text = "Confirmado. Efectivo esperado: " + summary.EfectivoEsperado.ToString("N2");
            }
            catch (Exception error)
            {
                if (IsDisposed || Disposing) return;
                var rejection = error as CajaHttpException;
                if (rejection?.EscrituraRechazada == true) { pendiente = null; impacto = null; }
                estadoAdmin.Text = (pendiente == null ? "No confirmado: " : "Resultado pendiente. UUID " + pendiente.IdOperacion + ". ") + error.Message;
            }
            finally { ocupado = false; EstadoControles(); }
        }

        private async void cargarCaja_Click(object sender, EventArgs e) => await CargarCajaAsync();
        private async void verImpacto_Click(object sender, EventArgs e) => await PrevisualizarAsync();
        private bool ConfirmarAccion(bool eliminar) => MessageBox.Show(this,
            (eliminar ? "Borrado definitivo, sin devolucion de dinero." : "Corregir a " + importeCorreccion.Value.ToString("N2") + " / " + medioCorreccion.Text)
                + Environment.NewLine + impactoDetalle.Text + Environment.NewLine + "Motivo: " + motivoAdmin.Text,
            "Confirmacion administrativa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes;
        private async void corregir_Click(object sender, EventArgs e) => await AplicarAsync(false, ConfirmarAccion(false));
        private async void borrar_Click(object sender, EventArgs e) => await AplicarAsync(true, ConfirmarAccion(true));

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
            medioCorreccion.DataSource = formasPago.Where(row => row.IdFormaPago > 0).ToList();
            medioCorreccion.DisplayMember = "Nombre";
            medioCorreccion.ValueMember = "IdFormaPago";
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
                if (!clsConnection.EsAdministrador)
                    throw new System.Security.SecurityException("Solo el perfil Administrador puede consultar ventas.");
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
                dtgMovimientos.DataSource = null;
                lblTotal.Text = "Valor Total Movimiento: No disponible";
                MessageBox.Show(ex.Message, "Consulta no disponible", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (ex is UnauthorizedAccessException)
                {
                    using (var login = new CarWash.Presentacion.Login.FrmLogin(true)) login.ShowDialog(this);
                }
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
