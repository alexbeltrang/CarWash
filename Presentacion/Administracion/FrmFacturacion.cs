using CarWash.Controladores;
using CarWash.DTOs;
using CarWash.Utilidades;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarWash.Presentacion.Administracion
{
    public sealed partial class FrmFacturacion : Form
    {
        private readonly FacturasController controller = new FacturasController();
        private readonly CancellationTokenSource lifetime = new CancellationTokenSource();
        private int revision;
        private bool loaded, dirty, setting;

        public FrmFacturacion()
        {
            InitializeComponent();
            if (UIHelper.EnModoDiseno(this)) return;
            save.Click += async (_, __) => await SaveAsync();
            reload.Click += async (_, __) => { if (!dirty || ConfirmDiscard()) await LoadAsync(); };
            btnCerrar.Click += (_, __) => Close();
            foreach (Control control in fields.Controls)
            {
                if (control is TextBox box) box.TextChanged += (_, __) => { if (!setting) dirty = true; };
                if (control is DateTimePicker picker) picker.ValueChanged += (_, __) => { if (!setting) dirty = true; };
                if (control is CheckBox check) check.CheckedChanged += (_, __) => { if (!setting) dirty = true; };
            }
            Shown += async (_, __) => await LoadAsync();
            FormClosing += (_, args) => { if (dirty && !ConfirmDiscard()) args.Cancel = true; };
            FormClosed += (_, __) => { lifetime.Cancel(); lifetime.Dispose(); };
            SetBusy(false);
        }
        private bool ConfirmDiscard() => MessageBox.Show(this, "Descartar los cambios sin guardar?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        private async Task LoadAsync()
        {
            if (!clsConnection.EsAdministrador) { status.Text = "Solo administradores pueden configurar facturacion."; SetBusy(false); return; }
            SetBusy(true);
            try { SetData(await controller.ConfiguracionAsync(lifetime.Token)); loaded = true; status.Text = ""; }
            catch (Exception error) { loaded = false; ShowError(error); }
            finally { if (!IsDisposed) SetBusy(false); }
        }
        private async Task SaveAsync()
        {
            if (!loaded || !clsConnection.EsAdministrador) return;
            SetBusy(true);
            try { SetData(await controller.GuardarConfiguracionAsync(GetData(), lifetime.Token)); status.Text = "Configuracion guardada."; }
            catch (Exception error) { ShowError(error); }
            finally { if (!IsDisposed) SetBusy(false); }
        }
        private ConfiguracionFacturacionDTO GetData()
        {
            if (!date.Checked || !from.Checked || !until.Checked || !long.TryParse(first.Text, out var initial)
                || !long.TryParse(last.Text, out var final) || !long.TryParse(next.Text, out var following)
                || !decimal.TryParse(rate.Text, NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out var iva))
                throw new IOException("Complete fechas, rango y tasa de IVA (cero si corresponde). ");
            return new ConfiguracionFacturacionDTO { Revision = revision, Habilitada = enabled.Checked, Prefijo = prefix.Text.Trim(), NumeroResolucion = resolution.Text.Trim(),
                FechaResolucion = date.Value.ToString("yyyy-MM-dd"), VigenteDesde = from.Value.ToString("yyyy-MM-dd"), VigenteHasta = until.Value.ToString("yyyy-MM-dd"),
                DesdeNumero = initial, HastaNumero = final, SiguienteNumero = following, TasaIva = iva };
        }
        private void SetData(ConfiguracionFacturacionDTO? data)
        {
            if (IsDisposed) return;
            setting = true;
            try
            {
                revision = data?.Revision ?? 0; enabled.Checked = data?.Habilitada ?? false; prefix.Text = data?.Prefijo ?? ""; resolution.Text = data?.NumeroResolucion ?? "";
                first.Text = data?.DesdeNumero.ToString() ?? ""; last.Text = data?.HastaNumero.ToString() ?? ""; next.Text = data?.SiguienteNumero.ToString() ?? "";
                rate.Text = data?.TasaIva?.ToString(CultureInfo.CurrentCulture) ?? "";
                SetDate(date, data?.FechaResolucion); SetDate(from, data?.VigenteDesde); SetDate(until, data?.VigenteHasta); dirty = false;
            }
            finally { setting = false; }
        }
        private static void SetDate(DateTimePicker picker, string? value)
        { picker.Checked = DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed); if (picker.Checked) picker.Value = parsed; }
        private void SetBusy(bool busy)
        { UseWaitCursor = busy; fields.Enabled = reload.Enabled = !busy && clsConnection.EsAdministrador; save.Enabled = !busy && loaded && clsConnection.EsAdministrador; }
        private void ShowError(Exception error)
        { if (!IsDisposed && !lifetime.IsCancellationRequested) status.Text = error is IOException || error is UnauthorizedAccessException ? error.Message : "No se pudo guardar/cargar. Recargue antes de reintentar."; }
    }
}