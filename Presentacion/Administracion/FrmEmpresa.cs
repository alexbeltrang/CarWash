using CarWash.Controladores;
using CarWash.DTOs;
using CarWash.Utilidades;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarWash.Presentacion.Administracion
{
    public sealed partial class FrmEmpresa : Form
    {
        private readonly EmpresaController controller = new EmpresaController();
        private readonly CancellationTokenSource lifetime = new CancellationTokenSource();
        private int revision;
        private string? logo;
        private bool loaded, settingData, dirty;

        public FrmEmpresa()
        {
            InitializeComponent();
            if (UIHelper.EnModoDiseno(this)) return;
            foreach (Control control in fields.Controls)
            {
                if (control is TextBox box) box.TextChanged += (_, __) => { if (!settingData) dirty = true; };
                if (control is ComboBox combo) combo.SelectedIndexChanged += (_, __) => { if (!settingData) dirty = true; };
            }
            nit.TextChanged += (_, __) =>
            {
                dv.Text = nit.Text.Length >= 5 && nit.Text.All(char.IsDigit) ? CheckDigit(nit.Text).ToString() : "";
            };
            button21.Click += (_, __) => LoadLogo();
            button22.Click += (_, __) => { logo = null; SetPreview(); dirty = true; };
            btnCerrar.Click += (_, __) => Close();
            save.Click += async (_, __) => await SaveAsync();
            reload.Click += async (_, __) =>
            {
                if (dirty && MessageBox.Show(this, "Descartar los cambios y recargar?", Text, MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                await LoadAsync();
            };
            Shown += async (_, __) => await LoadAsync();
            FormClosing += (_, args) => { if (dirty && MessageBox.Show(this, "Descartar los cambios sin guardar?", Text, MessageBoxButtons.YesNo) != DialogResult.Yes) args.Cancel = true; };
            FormClosed += (_, __) => { lifetime.Cancel(); lifetime.Dispose(); preview.Image?.Dispose(); };
            SetBusy(false);
        }

        private async Task LoadAsync()
        {
            if (!clsConnection.EsAdministrador) { status.Text = "Solo administradores pueden editar la empresa."; save.Enabled = false; return; }
            SetBusy(true); status.Text = "Cargando empresa...";
            try { SetData(await controller.ConsultarAsync(lifetime.Token) ?? new EmpresaDTO()); loaded = true; status.Text = ""; }
            catch (OperationCanceledException) { }
            catch (Exception error) { loaded = false; status.Text = error is IOException || error is UnauthorizedAccessException ? error.Message : "No se pudo cargar la empresa."; }
            finally { if (!IsDisposed) SetBusy(false); }
        }

        private async Task SaveAsync()
        {
            if (!loaded || !clsConnection.EsAdministrador) return;
            SetBusy(true); status.Text = "Guardando...";
            try { SetData(await controller.GuardarAsync(GetData(), lifetime.Token)); status.Text = "Empresa guardada."; }
            catch (OperationCanceledException) { }
            catch (Exception error) { status.Text = error is IOException || error is UnauthorizedAccessException ? error.Message : "No se pudo guardar. Recargue antes de reintentar."; }
            finally { if (!IsDisposed) SetBusy(false); }
        }

        private EmpresaDTO GetData() => new EmpresaDTO
        {
            Revision = revision, NombreComercial = nombre.Text.Trim(), RazonSocial = razon.Text.Trim(), TipoPersona = persona.Text,
            Nit = nit.Text.Trim(), DigitoVerificacion = dv.Text, Direccion = direccion.Text.Trim(), Municipio = municipio.Text.Trim(),
            Departamento = departamento.Text.Trim(), Pais = pais.Text.Trim(), Telefono = telefono.Text.Trim(), Correo = correo.Text.Trim(),
            ResponsabilidadesTributarias = responsabilidades.Text.Trim(), LogoBase64 = logo
        };

        private void SetData(EmpresaDTO data)
        {
            settingData = true;
            try
            {
            revision = data.Revision; nombre.Text = data.NombreComercial; razon.Text = data.RazonSocial;
            persona.SelectedItem = data.TipoPersona; nit.Text = data.Nit; dv.Text = data.DigitoVerificacion;
            direccion.Text = data.Direccion; municipio.Text = data.Municipio; departamento.Text = data.Departamento;
            pais.Text = data.Pais; telefono.Text = data.Telefono; correo.Text = data.Correo;
            responsabilidades.Text = data.ResponsabilidadesTributarias; logo = data.LogoBase64; SetPreview();
                dirty = false;
            }
            finally { settingData = false; }
        }

        private void SetBusy(bool busy)
        {
            fields.Enabled = !busy && clsConnection.EsAdministrador; preview.Parent.Enabled = fields.Enabled; reload.Enabled = fields.Enabled;
            save.Enabled = !busy && loaded && clsConnection.EsAdministrador;
        }

        private void LoadLogo()
        {
            using (var dialog = new OpenFileDialog { Filter = "Logo PNG/JPEG|*.png;*.jpg;*.jpeg", CheckFileExists = true })
            {
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                try
                {
                    if (new FileInfo(dialog.FileName).Length > 1_048_576) throw new IOException("El logo debe ocupar hasta 1 MB.");
                    var bytes = File.ReadAllBytes(dialog.FileName);
                    using (var stream = new MemoryStream(bytes))
                    using (var image = Image.FromStream(stream))
                        if (image.Width > 4096 || image.Height > 4096
                            || image.RawFormat.Guid != System.Drawing.Imaging.ImageFormat.Png.Guid && image.RawFormat.Guid != System.Drawing.Imaging.ImageFormat.Jpeg.Guid)
                            throw new IOException("Seleccione un logo PNG/JPEG de hasta 4096 pixeles.");
                    logo = Convert.ToBase64String(bytes); SetPreview(); dirty = true; status.Text = "";
                }
                catch (Exception error) when (error is IOException || error is ArgumentException || error is OutOfMemoryException)
                { status.Text = "Seleccione una imagen PNG/JPEG valida de hasta 1 MB y 4096 pixeles."; }
            }
        }

        private void SetPreview()
        {
            Image? image = null;
            if (logo != null) using (var stream = new MemoryStream(Convert.FromBase64String(logo))) using (var source = Image.FromStream(stream)) image = new Bitmap(source);
            var previous = preview.Image; preview.Image = image; previous?.Dispose();
        }

        private static int CheckDigit(string value)
        {
            int[] weights = { 3, 7, 13, 17, 19, 23, 29, 37, 41, 43, 47, 53, 59, 67, 71 };
            var sum = 0;
            for (var index = 0; index < value.Length; index++) sum += (value[value.Length - 1 - index] - '0') * weights[index];
            var remainder = sum % 11; return remainder > 1 ? 11 - remainder : remainder;
        }
    }
}