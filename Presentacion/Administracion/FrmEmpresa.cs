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
    public sealed class FrmEmpresa : Form
    {
        private readonly EmpresaController controller = new EmpresaController();
        private readonly CancellationTokenSource lifetime = new CancellationTokenSource();
        private readonly TextBox nombre = new TextBox(), razon = new TextBox(), nit = new TextBox(), dv = new TextBox();
        private readonly TextBox direccion = new TextBox(), municipio = new TextBox(), departamento = new TextBox(), pais = new TextBox();
        private readonly TextBox telefono = new TextBox(), correo = new TextBox(), responsabilidades = new TextBox();
        private readonly ComboBox persona = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly PictureBox preview = new PictureBox { SizeMode = PictureBoxSizeMode.Zoom, Dock = DockStyle.Fill };
        private readonly Label status = new Label { AutoSize = true, MaximumSize = new Size(650, 0) };
        private readonly Button save = new Button { Text = "Guardar", AutoSize = true, Image = SystemIcons.Information.ToBitmap(), TextImageRelation = TextImageRelation.ImageBeforeText };
        private readonly Button reload = new Button { Text = "Recargar", AutoSize = true };
        private readonly TableLayoutPanel fields = new TableLayoutPanel { AutoSize = true, Dock = DockStyle.Top, ColumnCount = 2, Padding = new Padding(12) };
        private int revision;
        private string? logo;
        private bool loaded, settingData, dirty;

        public FrmEmpresa()
        {
            Text = "Empresa del autolavado";
            Size = new Size(860, 740); MinimumSize = new Size(720, 620); StartPosition = FormStartPosition.CenterParent;
            fields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 210));
            fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            AddField("Nombre del autolavado *", nombre, 120);
            AddField("Razon social / nombre legal *", razon, 160);
            persona.Items.AddRange(new object[] { "Natural", "Juridica" });
            AddField("Tipo de persona *", persona);
            AddField("NIT (sin DV) *", nit, 15);
            dv.ReadOnly = true; AddField("Digito de verificacion", dv, 1);
            AddField("Direccion *", direccion, 200); AddField("Municipio *", municipio, 100);
            AddField("Departamento *", departamento, 100); AddField("Pais *", pais, 80);
            AddField("Telefono *", telefono, 30); AddField("Correo *", correo, 254);
            AddField("Responsabilidades segun RUT", responsabilidades, 500);
            nit.TextChanged += (_, __) =>
            {
                dv.Text = nit.Text.Length >= 5 && nit.Text.All(char.IsDigit) ? CheckDigit(nit.Text).ToString() : "";
            };
            var logoPanel = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 3, Padding = new Padding(12) };
            logoPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize)); logoPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); logoPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            logoPanel.Controls.Add(new Label { Text = "Logo del autolavado", AutoSize = true }, 0, 0);
            logoPanel.Controls.Add(preview, 0, 1);
            var logoButtons = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Fill };
            var loadLogo = new Button { Text = "Cargar logo", AutoSize = true };
            var removeLogo = new Button { Text = "Quitar logo", AutoSize = true };
            loadLogo.Click += (_, __) => LoadLogo();
            removeLogo.Click += (_, __) => { logo = null; SetPreview(); dirty = true; };
            logoButtons.Controls.AddRange(new Control[] { loadLogo, removeLogo }); logoPanel.Controls.Add(logoButtons, 0, 2);
            var body = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2 };
            body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 72)); body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28));
            var scroll = new Panel { Dock = DockStyle.Fill, AutoScroll = true }; scroll.Controls.Add(fields);
            body.Controls.Add(scroll, 0, 0); body.Controls.Add(logoPanel, 1, 0);
            var footer = new FlowLayoutPanel { Dock = DockStyle.Bottom, AutoSize = true, Padding = new Padding(12), FlowDirection = FlowDirection.LeftToRight };
            footer.Controls.AddRange(new Control[] { save, reload, status });
            save.Click += async (_, __) => await SaveAsync();
            reload.Click += async (_, __) =>
            {
                if (dirty && MessageBox.Show(this, "Descartar los cambios y recargar?", Text, MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                await LoadAsync();
            };
            Controls.Add(body); Controls.Add(footer);
            Shown += async (_, __) => await LoadAsync();
            FormClosing += (_, args) => { if (dirty && MessageBox.Show(this, "Descartar los cambios sin guardar?", Text, MessageBoxButtons.YesNo) != DialogResult.Yes) args.Cancel = true; };
            FormClosed += (_, __) => { lifetime.Cancel(); lifetime.Dispose(); preview.Image?.Dispose(); };
            SetBusy(false);
        }

        private void AddField(string label, Control control, int maxLength = 0)
        {
            var row = fields.RowCount++;
            fields.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            fields.Controls.Add(new Label { Text = label, AutoSize = true, Margin = new Padding(0, 10, 8, 10) }, 0, row);
            control.Dock = DockStyle.Fill; control.Margin = new Padding(0, 6, 0, 6);
            if (control is TextBox box) { box.MaxLength = maxLength; box.TextChanged += (_, __) => { if (!settingData) dirty = true; }; }
            if (control is ComboBox combo) combo.SelectedIndexChanged += (_, __) => { if (!settingData) dirty = true; };
            fields.Controls.Add(control, 1, row);
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