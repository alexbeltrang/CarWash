using CarWash.Controladores;
using CarWash.DTOs;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarWash.Presentacion.Operacion
{
    public sealed partial class FrmFactura : Form
    {
        private readonly FacturasController source = new FacturasController();
        private readonly RetiroVehiculosController printing = new RetiroVehiculosController();
        private readonly CancellationTokenSource lifetime = new CancellationTokenSource();
        private readonly int? turnoConfirmado;
        private PreparacionFacturaDTO? preparation;
        private FacturaDTO? invoice;
        private Guid issueId = Guid.NewGuid();
        private Guid? printId;
        private ImpresionComprobanteDTO? printJob;
        private bool busy, issueUnknown, printKnown, printUnknown;
        private bool resourcesDisposed;

        public FrmFactura() : this(null, false) { }

        public FrmFactura(int? idTurno) : this(idTurno, false) { }

        public FrmFactura(int? idTurno, bool emisionConfirmada)
        {
            turnoConfirmado = emisionConfirmada ? idTurno : null;
            InitializeComponent();
            if (UIHelper.EnModoDiseno(this)) return;
            foreach (Control control in buyerFields.Controls)
            {
                if (control is TextBox box) box.TextChanged += (_, __) => UpdateControls();
                if (control is ComboBox combo) combo.SelectedIndexChanged += (_, __) => UpdateControls();
            }
            btnCerrar.Click += (_, __) => Close();
            search.Click += async (_, __) => await SearchAsync(); reload.Click += async (_, __) => await RefreshAsync();
            issue.Click += async (_, __) => await IssueAsync(); print.Click += async (_, __) => await PrintAsync(); checkPrint.Click += async (_, __) => await CheckPrintAsync();
            preview.Click += (_, __) => Preview();
            history.CellDoubleClick += async (_, args) =>
            {
                if (busy || args.RowIndex < 0 || !(history.Rows[args.RowIndex].Tag is FacturaDTO selected)) return;
                SetBusy(true);
                try { ShowInvoice(await source.ConsultarAsync(selected.IdTurno, lifetime.Token) ?? throw new IOException("Factura no encontrada.")); await ReadPrintAsync(); }
                catch (Exception error) { ShowError(error); }
                finally { SetBusy(false); }
            };
            Shown += async (_, __) =>
            {
                if (idTurno.HasValue) { lookup.Text = idTurno.Value.ToString(); await SearchAsync(); }
                await LoadHistoryAsync();
            };
            FormClosed += (_, __) => lifetime.Cancel();
            Resize += (_, __) => status.MaximumSize = new Size(Math.Max(400, ClientSize.Width - 40), 0);
            UpdateControls();
        }
        private async Task SearchAsync()
        {
            if (busy || string.IsNullOrWhiteSpace(lookup.Text)) return;
            if (lookup.Text.Trim() != (invoice?.IdTurno ?? preparation?.IdTurno)?.ToString())
            { invoice = null; preparation = null; issueUnknown = false; ResetPrint(); detail.Clear(); SetLogo(null); }
            SetBusy(true); status.Text = "";
            try
            {
                if (int.TryParse(lookup.Text, out var id)) await LoadTurnAsync(id);
                else
                {
                    var matches = await source.HistorialAsync(lifetime.Token, lookup.Text.Trim());
                    if (matches.Count > 0) { ShowInvoice(matches[0]); await ReadPrintAsync(); }
                    else ShowPreparation(await source.PrepararAsync(null, lookup.Text.Trim(), lifetime.Token));
                }
            }
            catch (Exception error) { ShowError(error); }
            finally { SetBusy(false); }
        }
        private async Task LoadTurnAsync(int id)
        {
            var existing = await source.ConsultarAsync(id, lifetime.Token);
            if (existing != null) { ShowInvoice(existing); await ReadPrintAsync(); }
            else ShowPreparation(await source.PrepararAsync(id, null, lifetime.Token));
        }
        private async Task RefreshAsync()
        {
            if (busy) return;
            SetBusy(true);
            try
            {
                if (invoice != null || preparation != null) await LoadTurnAsync(invoice?.IdTurno ?? preparation!.IdTurno);
                await ReadHistoryAsync();
            }
            catch (Exception error) { ShowError(error); }
            finally { SetBusy(false); }
        }
        private async Task LoadHistoryAsync()
        {
            if (busy) return;
            SetBusy(true);
            try { await ReadHistoryAsync(); }
            catch (Exception error) { ShowError(error); }
            finally { SetBusy(false); }
        }
        private async Task ReadHistoryAsync()
        {
            var rows = await source.HistorialAsync(lifetime.Token);
            if (IsDisposed) return;
            history.Rows.Clear();
            foreach (var item in rows)
            {
                var row = history.Rows.Add(item.Fecha, item.Prefijo + item.Numero, item.Placa, item.Comprador.Nombre, item.Total.ToString("N2"));
                history.Rows[row].Tag = item;
            }
        }
        private void ShowPreparation(PreparacionFacturaDTO value)
        {
            if (IsDisposed) return;
            var sameDraft = preparation?.IdTurno == value.IdTurno;
            if (!sameDraft) issueId = Guid.NewGuid();
            preparation = value; invoice = null; issueUnknown = false; ResetPrint();
            if (!sameDraft) SetBuyer(value.Comprador);
            SetLogo(value.Empresa.LogoBase64);
            var rate = value.Configuracion.TasaIva ?? throw new IOException("Configure la tasa de IVA.");
            var net = decimal.Round(value.Total / (1 + rate / 100), 2, MidpointRounding.AwayFromZero);
            detail.Text = string.Join(Environment.NewLine, value.Empresa.NombreComercial, value.Empresa.RazonSocial,
                "NIT: " + value.Empresa.Nit + "-" + value.Empresa.DigitoVerificacion, "FACTURA POR EMITIR", "Turno: " + value.IdTurno, "Placa: " + value.Placa,
                "Autorizacion: " + value.Configuracion.NumeroResolucion, "Vigencia: " + value.Configuracion.VigenteDesde + " a " + value.Configuracion.VigenteHasta,
                "Rango: " + value.Configuracion.Prefijo + value.Configuracion.DesdeNumero + " a " + value.Configuracion.Prefijo + value.Configuracion.HastaNumero,
                "Cantidad: 1", value.Concepto, "Base: " + net.ToString("N2"), "IVA " + rate + "%: " + (value.Total - net).ToString("N2"), "TOTAL COP: " + value.Total.ToString("N2"));
            status.Text = ""; UpdateControls();
        }
        private void ShowInvoice(FacturaDTO value)
        {
            if (IsDisposed) return;
            invoice = value; preparation = null; issueUnknown = false; ResetPrint(); SetBuyer(value.Comprador); SetLogo(value.Empresa.LogoBase64);
            detail.Text = value.Texto.Replace("\n", Environment.NewLine); status.Text = "Factura " + value.Prefijo + value.Numero; UpdateControls();
        }
        private void SetBuyer(CompradorFacturaDTO value)
        { name.Text = value.Nombre; type.SelectedItem = value.TipoDocumento; document.Text = value.NumeroDocumento; address.Text = value.Direccion; phone.Text = value.Telefono; email.Text = value.Correo; }
        private CompradorFacturaDTO Buyer() => new CompradorFacturaDTO
        {
            Nombre = name.Text.Trim(),
            TipoDocumento = type.Text,
            NumeroDocumento = document.Text.Trim(),
            Direccion = address.Text.Trim(),
            Telefono = phone.Text.Trim(),
            Correo = string.IsNullOrWhiteSpace(email.Text) ? null : email.Text.Trim()
        };
        private void SetLogo(string? encoded)
        {
            Image? image = null;
            if (encoded != null) using (var stream = new MemoryStream(Convert.FromBase64String(encoded))) using (var sourceImage = Image.FromStream(stream)) image = new Bitmap(sourceImage);
            var previous = logo.Image; logo.Image = image; logo.Visible = image != null; previous?.Dispose();
        }
        private async Task IssueAsync()
        {
            if (busy || preparation == null || invoice != null || issueUnknown || !issue.Enabled) return;
            if (preparation.IdTurno != turnoConfirmado && MessageBox.Show(this, "Emitir factura para " + name.Text.Trim() + " por COP " + preparation.Total.ToString("N2") + "?", Text,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            var draft = preparation;
            SetBusy(true);
            try { ShowInvoice(await source.EmitirAsync(draft, Buyer(), issueId, lifetime.Token)); await ReadPrintAsync(); }
            catch (Exception error)
            {
                issueUnknown = true;
                try
                {
                    var recovered = await source.ConsultarAsync(draft.IdTurno, lifetime.Token);
                    if (recovered != null) { ShowInvoice(recovered); await ReadPrintAsync(); }
                    else { issueUnknown = false; ShowError(error); }
                }
                catch { if (!IsDisposed) status.Text = "Emision sin confirmar. Consulte el turno antes de reintentar."; }
            }
            finally { SetBusy(false); }
        }
        private void ResetPrint() { printId = null; printJob = null; printKnown = false; printUnknown = false; }
        private async Task PrintAsync()
        {
            if (busy || invoice == null || !printKnown || printUnknown || printJob?.Estado == "PENDING" || printJob?.Estado == "SUBMITTING") return;
            if (printJob != null)
            {
                if (MessageBox.Show(this, "Revise el papel y la cola anteriores. Solicitar otra copia de esta factura?", "Impresion", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                printId = Guid.NewGuid();
            }
            if (!printId.HasValue) printId = Guid.NewGuid();
            SetBusy(true);
            try { printJob = await source.ImprimirAsync(invoice, printId.Value, lifetime.Token); ShowPrintState(); }
            catch (Exception error)
            {
                printUnknown = true;
                await ReadPrintAsync();
                if (printUnknown) ShowError(error);
            }
            finally { SetBusy(false); }
        }
        private async Task CheckPrintAsync()
        { if (busy || invoice == null) return; SetBusy(true); try { await ReadPrintAsync(); } finally { SetBusy(false); } }
        private async Task ReadPrintAsync()
        {
            if (invoice == null) return;
            try
            {
                var job = printId.HasValue ? await printing.ConsultarImpresionAsync(printId.Value, lifetime.Token) : await source.UltimaImpresionAsync(invoice, lifetime.Token);
                if (job != null && job.IdTurno != invoice.IdTurno) throw new IOException("Estado de impresion de otro turno.");
                if (IsDisposed) return;
                printJob = job; if (job != null) printId = job.SolicitudId; printKnown = true; printUnknown = false; ShowPrintState();
            }
            catch (Exception) { printUnknown = true; if (!IsDisposed) status.Text = "Impresion sin confirmar. Consulte su estado antes de repetir."; }
        }
        private void ShowPrintState()
        {
            if (IsDisposed) return;
            status.Text = printJob == null ? (printId.HasValue ? "Solicitud no encontrada. Reintente la misma solicitud." : "")
                : printJob.Estado == "PENDING" ? "En cola: " + printJob.Impresora
                : printJob.Estado == "SUBMITTING" ? "Enviando a " + printJob.Impresora
                : printJob.Estado == "SUBMITTED" ? "Enviado a " + printJob.Impresora + ". Salida de papel sin confirmar."
                : printJob.Estado == "FAILED" ? "No se pudo iniciar la impresion. Revise la impresora." : "Impresion incierta. Revise el papel y la cola.";
        }
        private void Preview()
        {
            if (busy || invoice == null) return;
            try
            {
                using (var documentPreview = new PrintDocument())
                using (var dialog = new Form { Text = "Factura " + invoice.Prefijo + invoice.Numero, Width = 700, Height = 800, StartPosition = FormStartPosition.CenterParent })
                {
                    var snapshot = invoice; var offset = 0;
                    documentPreview.DocumentName = "Factura " + snapshot.Prefijo + snapshot.Numero;
                    documentPreview.DefaultPageSettings.PaperSize = new PaperSize("Factura 80 mm", 315, 800); documentPreview.DefaultPageSettings.Margins = new Margins(8, 8, 8, 8);
                    documentPreview.BeginPrint += (_, __) => offset = 0;
                    documentPreview.PrintPage += (_, args) => { offset = DrawInvoicePage(args.Graphics, args.MarginBounds, snapshot.Texto, logo.Image, offset); args.HasMorePages = offset < snapshot.Texto.Length; };
                    var control = new PrintPreviewControl { Document = documentPreview, Dock = DockStyle.Fill, AutoZoom = true };
                    var tools = new ToolStrip { Dock = DockStyle.Top };
                    var previous = new ToolStripButton("<") { ToolTipText = "Pagina anterior" }; var next = new ToolStripButton(">") { ToolTipText = "Pagina siguiente" };
                    previous.Click += (_, __) => control.StartPage = Math.Max(0, control.StartPage - 1); next.Click += (_, __) => control.StartPage++;
                    tools.Items.Add(previous); tools.Items.Add(next); dialog.Controls.Add(control); dialog.Controls.Add(tools); dialog.ShowDialog(this);
                }
            }
            catch (Exception) { status.Text = "Vista previa no disponible. La factura se conserva."; }
        }
        public static int DrawInvoicePage(Graphics graphics, Rectangle bounds, string text, Image? logo, int offset)
        {
            if (offset == 0 && logo != null)
            {
                var scale = Math.Min(Math.Min(160f, bounds.Width) / logo.Width, Math.Min(100f, bounds.Height / 3f) / logo.Height);
                var width = logo.Width * scale; var height = logo.Height * scale;
                graphics.DrawImage(logo, bounds.Left + (bounds.Width - width) / 2f, bounds.Top, width, height);
                var used = (int)Math.Ceiling(height) + 10; bounds = new Rectangle(bounds.Left, bounds.Top + used, bounds.Width, bounds.Height - used);
            }
            return FrmRetiroVehiculo.DrawReceiptPage(graphics, bounds, text, offset);
        }
        private void ShowError(Exception error)
        { if (!IsDisposed && !lifetime.IsCancellationRequested) status.Text = error is IOException || error is UnauthorizedAccessException ? error.Message : "No se pudo completar la operacion. Consulte el estado."; }
        private void SetBusy(bool value) { if (IsDisposed) return; busy = value; UseWaitCursor = value; UpdateControls(); }
        private void UpdateControls()
        {
            if (IsDisposed) return;
            lookup.Enabled = search.Enabled = reload.Enabled = history.Enabled = !busy;
            buyerFields.Enabled = !busy && preparation != null && invoice == null && !issueUnknown;
            issue.Enabled = buyerFields.Enabled && name.Text.Trim().Length > 0 && type.SelectedIndex >= 0 && document.Text.Trim().Length > 0;
            preview.Enabled = checkPrint.Enabled = !busy && invoice != null;
            print.Enabled = preview.Enabled && printKnown && !printUnknown && printJob?.Estado != "PENDING" && printJob?.Estado != "SUBMITTING";
            print.Text = printJob != null ? "Otra copia" : printId.HasValue ? "Reintentar solicitud" : "Imprimir";
        }
        protected override void Dispose(bool disposing)
        { if (disposing && !resourcesDisposed) { resourcesDisposed = true; lifetime.Cancel(); lifetime.Dispose(); logo?.Image?.Dispose(); components?.Dispose(); } base.Dispose(disposing); }
    }
}