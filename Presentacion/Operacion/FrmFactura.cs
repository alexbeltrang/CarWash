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
    public sealed class FrmFactura : Form
    {
        private readonly FacturasController source = new FacturasController();
        private readonly RetiroVehiculosController printing = new RetiroVehiculosController();
        private readonly CancellationTokenSource lifetime = new CancellationTokenSource();
        private readonly int? turnoConfirmado;
        private readonly TextBox lookup = new TextBox { MaxLength = 20, CharacterCasing = CharacterCasing.Upper, Width = 160 };
        private readonly Button search = new Button { Text = "Consultar", AutoSize = true }, reload = new Button { Text = "Actualizar", AutoSize = true };
        private readonly Button issue = new Button { Text = "Emitir factura", AutoSize = true, Image = SystemIcons.Information.ToBitmap(), TextImageRelation = TextImageRelation.ImageBeforeText };
        private readonly Button preview = new Button { Text = "Vista previa", AutoSize = true }, print = new Button { Text = "Imprimir", AutoSize = true }, checkPrint = new Button { Text = "Consultar impresion", AutoSize = true };
        private readonly Label status = new Label { AutoSize = true, MaximumSize = new Size(820, 0), Margin = new Padding(4) };
        private readonly TextBox detail = new TextBox { ReadOnly = true, Multiline = true, TabStop = false, ScrollBars = ScrollBars.Vertical, Dock = DockStyle.Fill, Font = new Font("Consolas", 10) };
        private readonly PictureBox logo = new PictureBox { SizeMode = PictureBoxSizeMode.Zoom, Dock = DockStyle.Top, Height = 70, Visible = false };
        private readonly TextBox name = new TextBox(), document = new TextBox(), address = new TextBox(), phone = new TextBox(), email = new TextBox();
        private readonly ComboBox type = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly TableLayoutPanel buyerFields = new TableLayoutPanel { AutoSize = true, Dock = DockStyle.Top, ColumnCount = 2, Padding = new Padding(0, 8, 12, 0) };
        private readonly DataGridView history = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false, RowHeadersVisible = false,
            MultiSelect = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
        private PreparacionFacturaDTO? preparation;
        private FacturaDTO? invoice;
        private Guid issueId = Guid.NewGuid();
        private Guid? printId;
        private ImpresionComprobanteDTO? printJob;
        private bool busy, issueUnknown, printKnown, printUnknown;
        private bool resourcesDisposed;

        public FrmFactura(int? idTurno = null) : this(idTurno, false) { }

        public FrmFactura(int? idTurno, bool emisionConfirmada)
        {
            turnoConfirmado = emisionConfirmada ? idTurno : null;
            Text = "Facturas de venta en papel"; Name = "FrmFactura";
            Size = new Size(1040, 800); MinimumSize = new Size(800, 650); StartPosition = FormStartPosition.CenterParent;
            var top = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Top, Padding = new Padding(12) };
            top.Controls.AddRange(new Control[] { new Label { Text = "Turno o placa", AutoSize = true, Margin = new Padding(0, 7, 8, 0) }, lookup, search, reload });
            var body = new TableLayoutPanel { ColumnCount = 2, Dock = DockStyle.Fill, Padding = new Padding(12) };
            body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40)); body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
            buyerFields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130)); buyerFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            type.Items.AddRange(new object[] { "CC", "NIT", "CE", "TI", "PAS", "PEP", "PPT" });
            AddBuyer("Comprador *", name, 160); AddBuyer("Tipo documento *", type); AddBuyer("Documento *", document, 30);
            AddBuyer("Direccion", address, 200); AddBuyer("Telefono", phone, 30); AddBuyer("Correo", email, 254);
            var buyerScroll = new Panel { Dock = DockStyle.Fill, AutoScroll = true }; buyerScroll.Controls.Add(buyerFields);
            var documentArea = new Panel { Dock = DockStyle.Fill }; documentArea.Controls.Add(detail); documentArea.Controls.Add(logo);
            body.Controls.Add(buyerScroll, 0, 0); body.Controls.Add(documentArea, 1, 0);
            history.Columns.Add("date", "Fecha"); history.Columns.Add("number", "Factura"); history.Columns.Add("plate", "Placa"); history.Columns.Add("buyer", "Comprador"); history.Columns.Add("total", "Total COP");
            history.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            var sections = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2 };
            sections.RowStyles.Add(new RowStyle(SizeType.Percent, 72)); sections.RowStyles.Add(new RowStyle(SizeType.Percent, 28)); sections.Controls.Add(body, 0, 0); sections.Controls.Add(history, 0, 1);
            var actions = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Fill, Padding = new Padding(12) };
            actions.Controls.AddRange(new Control[] { issue, preview, print, checkPrint });
            var footer = new TableLayoutPanel { AutoSize = true, Dock = DockStyle.Bottom, ColumnCount = 1 };
            footer.Controls.Add(actions); footer.Controls.Add(status); Controls.Add(sections); Controls.Add(top); Controls.Add(footer);
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

        private void AddBuyer(string label, Control control, int max = 0)
        {
            var row = buyerFields.RowCount++; buyerFields.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            buyerFields.Controls.Add(new Label { Text = label, AutoSize = true, Margin = new Padding(0, 10, 8, 10) }, 0, row);
            control.Dock = DockStyle.Fill; control.Margin = new Padding(0, 6, 0, 6); buyerFields.Controls.Add(control, 1, row);
            if (control is TextBox box) { box.MaxLength = max; box.TextChanged += (_, __) => UpdateControls(); }
            if (control is ComboBox combo) combo.SelectedIndexChanged += (_, __) => UpdateControls();
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
        private CompradorFacturaDTO Buyer() => new CompradorFacturaDTO { Nombre = name.Text.Trim(), TipoDocumento = type.Text, NumeroDocumento = document.Text.Trim(),
            Direccion = address.Text.Trim(), Telefono = phone.Text.Trim(), Correo = string.IsNullOrWhiteSpace(email.Text) ? null : email.Text.Trim() };
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
        { if (disposing && !resourcesDisposed) { resourcesDisposed = true; lifetime.Cancel(); lifetime.Dispose(); logo.Image?.Dispose(); } base.Dispose(disposing); }
    }
}