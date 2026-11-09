using CarWash.Controladores;
using CarWash.DTOs;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarWash.Presentacion.Operacion
{
    public sealed partial class FrmRetiroVehiculo : Form
    {
        private readonly RetiroVehiculosController source = new RetiroVehiculosController();
        private readonly CancellationTokenSource lifetime = new CancellationTokenSource();
        private ComprobanteRetiroDTO? receipt;
        private bool busy;
        private bool checkRequired;
        private Guid? printRequestId;
        private ImpresionComprobanteDTO? printJob;
        private bool printHistoryKnown;
        private bool printUnknown;

        public FrmRetiroVehiculo()
        {
            InitializeComponent();
            if (UIHelper.EnModoDiseno(this)) return;
            Resize += (_, __) => printStatus.MaximumSize = new Size(Math.Max(200, ClientSize.Width - 60), 0);
            btnCerrar.Click += (_, __) => Close();
            search.Click += async (_, __) => await SearchAsync();
            refresh.Click += async (_, __) => await RefreshAsync();
            confirm.Click += async (_, __) => await ConfirmAsync();
            preview.Click += (_, __) => PreviewReceipt();
            print.Click += async (_, __) => await PrintAsync();
            printCheck.Click += async (_, __) => await CheckPrintAsync();
            email.Click += async (_, __) => await EmailAsync();
            reviewed.CheckedChanged += (_, __) => UpdateControls();
            plate.TextChanged += (_, __) => { if (!busy) { receipt = null; detail.Clear(); reviewed.Checked = false; checkRequired = false; status.Text = ""; ResetPrint(); UpdateControls(); } };
            FormClosed += (_, __) => lifetime.Cancel();
            UpdateControls();
        }

        private async Task SearchAsync()
        {
            if (busy || string.IsNullOrWhiteSpace(plate.Text)) return;
            receipt = null; detail.Clear(); checkRequired = false;
            ResetPrint();
            SetBusy(true);
            try { ShowReceipt(await source.EmitirAsync(plate.Text, lifetime.Token)); await ReadPrintAsync(); }
            catch (Exception error) { ShowError(error); }
            finally { SetBusy(false); }
        }

        private async Task RefreshAsync()
        {
            if (busy || receipt == null) return;
            SetBusy(true);
            try { ShowReceipt(await source.ConsultarAsync(receipt.IdTurno, lifetime.Token)); checkRequired = false; }
            catch (Exception error) { ShowError(error); }
            finally { SetBusy(false); }
        }

        private async Task ConfirmAsync()
        {
            if (busy || checkRequired || receipt == null || receipt.EntregadoEn.HasValue || !reviewed.Checked) return;
            if (MessageBox.Show(this, "Confirmar entrega del vehiculo " + receipt.Placa + "?", "Retiro", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            var id = receipt.IdTurno;
            SetBusy(true);
            try { ShowReceipt(await source.EntregarAsync(id, lifetime.Token)); }
            catch (Exception error)
            {
                checkRequired = true;
                try { ShowReceipt(await source.ConsultarAsync(id, lifetime.Token)); checkRequired = false; }
                catch { }
                if (receipt?.EntregadoEn == null) ShowError(error);
            }
            finally { if (!IsDisposed) reviewed.Checked = false; SetBusy(false); }
        }

        private async Task EmailAsync()
        {
            if (busy || receipt == null) return;
            SetBusy(true);
            try
            {
                var result = await source.CorreoAsync(receipt.IdTurno, lifetime.Token);
                if (!IsDisposed) status.Text = result == "ACCEPTED" ? "Correo aceptado por el servidor de envio (sin duplicar envios previos)."
                    : result == "NOT_APPLICABLE" ? "El ingreso no tiene un correo utilizable." : "Estado del correo: " + result;
            }
            catch (Exception error) { ShowError(error); }
            finally { SetBusy(false); }
        }

        private void ResetPrint()
        {
            printRequestId = null; printJob = null; printHistoryKnown = false; printUnknown = false; printStatus.Text = "";
        }

        private async Task PrintAsync()
        {
            if (busy || receipt == null || !printHistoryKnown || printUnknown || printJob?.Estado == "PENDING" || printJob?.Estado == "SUBMITTING") return;
            if (printJob != null)
            {
                if (MessageBox.Show(this, "Revise la salida de papel anterior. Solicitar otra copia?", "Impresion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                printRequestId = Guid.NewGuid();
            }
            if (!printRequestId.HasValue) printRequestId = Guid.NewGuid();
            var id = printRequestId.Value;
            var turnId = receipt.IdTurno;
            var width = 80;
            printJob = null;
            SetBusy(true);
            try { printJob = await source.ImprimirAsync(turnId, id, width, lifetime.Token); ShowPrintState(); }
            catch (Exception error)
            {
                printUnknown = true;
                await ReadPrintAsync();
                if (printUnknown && !IsDisposed) printStatus.Text = error is UnauthorizedAccessException ? error.Message : "Impresion sin confirmar. Consulte el estado antes de repetir.";
            }
            finally { SetBusy(false); }
        }

        private async Task CheckPrintAsync()
        {
            if (busy || receipt == null) return;
            SetBusy(true);
            try { await ReadPrintAsync(); } finally { SetBusy(false); }
        }

        private async Task ReadPrintAsync()
        {
            if (receipt == null) return;
            try
            {
                var job = printRequestId.HasValue ? await source.ConsultarImpresionAsync(printRequestId.Value, lifetime.Token)
                    : await source.UltimaImpresionAsync(receipt.IdTurno, lifetime.Token);
                if (IsDisposed) return;
                printJob = job;
                if (job != null) printRequestId = job.SolicitudId;
                printHistoryKnown = true; printUnknown = false; ShowPrintState();
            }
            catch (Exception error)
            {
                printUnknown = true;
                if (!IsDisposed) printStatus.Text = error is UnauthorizedAccessException ? error.Message : "Impresion no disponible. Consulte el estado antes de solicitar una copia.";
            }
        }

        private void ShowPrintState()
        {
            if (IsDisposed) return;
            printStatus.Text = printJob == null ? (printRequestId.HasValue ? "Solicitud no encontrada; puede reintentar con el mismo identificador." : "")
                : printJob.Estado == "PENDING" ? "En cola para " + printJob.Impresora
                : printJob.Estado == "SUBMITTING" ? "Enviando a " + printJob.Impresora
                : printJob.Estado == "SUBMITTED" ? "Enviado a " + printJob.Impresora + ". Salida de papel sin confirmar."
                : printJob.Estado == "FAILED" ? "No se pudo iniciar la impresion. Revise la impresora."
                : "Resultado de impresion incierto. Revise papel/cola antes de solicitar otra copia.";
        }

        private void ShowReceipt(ComprobanteRetiroDTO value)
        {
            if (IsDisposed) return;
            receipt = value; reviewed.Checked = false;
            detail.Text = ReceiptText(value);
            status.Text = value.EntregadoEn.HasValue ? "Vehiculo entregado: " + value.EntregadoEn.Value.ToLocalTime().ToString("g") : "Pendiente de entrega";
            UpdateControls();
        }

        private void ShowError(Exception error)
        {
            if (!IsDisposed && !lifetime.IsCancellationRequested)
                status.Text = error is UnauthorizedAccessException || error is System.IO.IOException ? error.Message : "No se pudo completar la operacion. Consulte el estado.";
        }

        private void SetBusy(bool value) { if (IsDisposed) return; busy = value; UseWaitCursor = value; UpdateControls(); }
        private void UpdateControls()
        {
            if (IsDisposed) return;
            plate.Enabled = search.Enabled = !busy;
            refresh.Enabled = preview.Enabled = email.Enabled = printCheck.Enabled = !busy && receipt != null;
            print.Enabled = !busy && receipt != null && printHistoryKnown && !printUnknown && printJob?.Estado != "PENDING" && printJob?.Estado != "SUBMITTING";
            print.Text = printJob != null ? "Otra copia" : printRequestId.HasValue ? "Reintentar solicitud" : "Imprimir";
            reviewed.Enabled = !busy && !checkRequired && receipt != null && !receipt.EntregadoEn.HasValue;
            confirm.Enabled = reviewed.Enabled && reviewed.Checked;
            if (checkRequired) status.Text = "Entrega sin confirmar. Consulte el estado antes de reintentar.";
        }

        public static string ReceiptText(ComprobanteRetiroDTO value)
        {
            var culture = CultureInfo.GetCultureInfo("es-CO");
            var text = new StringBuilder();
            var company = CarWash.Controladores.EmpresaController.Actual;
            if (company != null)
                text.AppendLine(company.NombreComercial).AppendLine(company.RazonSocial)
                    .AppendLine("NIT: " + company.Nit + "-" + company.DigitoVerificacion)
                    .AppendLine(company.Direccion).AppendLine(company.Municipio + " / " + company.Departamento + " / " + company.Pais)
                    .AppendLine("Telefono: " + company.Telefono).AppendLine(company.Correo).AppendLine(company.ResponsabilidadesTributarias).AppendLine();
            text.AppendLine("RECIBO DE INGRESO").AppendLine();
            text.AppendLine("Placa: " + value.Placa).AppendLine("Marca: " + value.Marca).AppendLine("Cliente: " + value.Cliente)
                .AppendLine("Ingreso: " + value.FechaIngreso.ToLocalTime().ToString("g", culture)).AppendLine();
            text.AppendLine("SERVICIOS AUTORIZADOS");
            foreach (var service in value.Servicios) text.AppendLine(service.Nombre).AppendLine("  " + service.Precio.ToString("C2", culture));
            text.AppendLine().AppendLine("TOTAL: " + value.Total.ToString("C2", culture)).AppendLine().AppendLine("Gracias por confiar en nosotros.");
            if (value.EntregadoEn.HasValue) text.AppendLine("ENTREGADO: " + value.EntregadoEn.Value.ToLocalTime().ToString("g", culture)).AppendLine("Encargado: " + value.EntregadoPor);
            text.AppendLine("Software: CarWash");
            return text.ToString();
        }

        private void PreviewReceipt()
        {
            if (busy || receipt == null) return;
            try
            {
                using (var document = new PrintDocument())
                using (var font = new Font("Consolas", 9))
                using (var logoStream = CarWash.Controladores.EmpresaController.Actual?.LogoBase64 == null ? null : new System.IO.MemoryStream(Convert.FromBase64String(CarWash.Controladores.EmpresaController.Actual.LogoBase64)))
                using (var logo = logoStream == null ? null : Image.FromStream(logoStream))
                {
                    var text = ReceiptText(receipt);
                    var offset = 0;
                    var width = 315;
                    document.DocumentName = "Comprobante de ingreso";
                    document.DefaultPageSettings.PaperSize = new PaperSize("Comprobante", width, 800);
                    document.DefaultPageSettings.Margins = new Margins(8, 8, 8, 8);
                    document.BeginPrint += (_, __) => offset = 0;
                    document.PrintPage += (_, args) =>
                    {
                        var bounds = args.MarginBounds;
                        if (offset == 0 && logo != null)
                        {
                            var scale = Math.Min(Math.Min(160f, bounds.Width) / logo.Width, Math.Min(100f, bounds.Height / 3f) / logo.Height);
                            var logoWidth = logo.Width * scale; var logoHeight = logo.Height * scale;
                            args.Graphics.DrawImage(logo, bounds.Left + (bounds.Width - logoWidth) / 2f, bounds.Top, logoWidth, logoHeight);
                            var used = (int)Math.Ceiling(logoHeight) + 10;
                            bounds = new Rectangle(bounds.Left, bounds.Top + used, bounds.Width, bounds.Height - used);
                        }
                        offset = DrawReceiptPage(args.Graphics, bounds, text, offset);
                        args.HasMorePages = offset < text.Length;
                    };
                    using (var previewDialog = new Form { Text = "Vista previa", Width = 700, Height = 800, StartPosition = FormStartPosition.CenterParent })
                    {
                        var control = new PrintPreviewControl { Document = document, Dock = DockStyle.Fill, AutoZoom = true };
                        var tools = new ToolStrip { Dock = DockStyle.Top };
                        var previous = new ToolStripButton("<") { ToolTipText = "Pagina anterior" };
                        var next = new ToolStripButton(">") { ToolTipText = "Pagina siguiente" };
                        previous.Click += (_, __) => control.StartPage = Math.Max(0, control.StartPage - 1);
                        next.Click += (_, __) => control.StartPage++;
                        tools.Items.Add(previous); tools.Items.Add(next);
                        previewDialog.Controls.Add(control); previewDialog.Controls.Add(tools);
                        previewDialog.ShowDialog(this);
                    }
                }
            }
            catch (Exception) { status.Text = "No se pudo imprimir. Revise impresora, controlador y papel; el comprobante se conserva."; }
        }

        public static int DrawReceiptPage(Graphics graphics, Rectangle bounds, string text, int offset)
        {
            using (var font = new Font("Consolas", 9))
            using (var format = StringFormat.GenericTypographic)
            {
                format.FormatFlags &= ~StringFormatFlags.NoClip;
                int characters, lines;
                var remaining = text.Substring(offset);
                graphics.MeasureString(remaining, font, bounds.Size, format, out characters, out lines);
                if (characters <= 0) throw new InvalidOperationException("El papel no tiene area imprimible.");
                graphics.DrawString(remaining.Substring(0, characters), font, Brushes.Black, bounds, format);
                return offset + characters;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) { lifetime.Cancel(); lifetime.Dispose(); components?.Dispose(); }
            base.Dispose(disposing);
        }
    }
}