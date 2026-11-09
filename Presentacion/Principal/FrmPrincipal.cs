using CarWash.Database;
using CarWash.Presentacion.Administracion;
using CarWash.Presentacion.Administracion.Nomina;
using CarWash.Presentacion.Consultas;
using CarWash.Presentacion.Operacion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarWash.Presentacion.Principal
{
    public partial class FrmPrincipal : Form
    {
        private bool readingCompany;

        public FrmPrincipal()
        {
            InitializeComponent();
        }

        private void FacturacionMenu_Click(object sender, EventArgs e)
        {
            if (!CarWash.Utilidades.clsConnection.EsAdministrador) return;
            using (var form = new FrmFacturacion()) form.ShowDialog(this);
        }

        private async void EmpresaMenu_Click(object sender, EventArgs e)
        {
            if (!CarWash.Utilidades.clsConnection.EsAdministrador) return;
            using (var form = new FrmEmpresa()) form.ShowDialog(this);
            await RefreshCompanyAsync();
        }

        private void RetiroMenu_Click(object sender, EventArgs e)
        {
            using (var form = new FrmRetiroVehiculo()) form.ShowDialog(this);
        }

        private void FacturasMenu_Click(object sender, EventArgs e)
        {
            using (var form = new FrmFactura()) form.ShowDialog(this);
        }

        protected override async void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            if (UIHelper.EnModoDiseno(this)) return;
            ActualizarPermisosMenu();
            if (!readingCompany) await RefreshCompanyAsync();
        }

        private void ActualizarPermisosMenu()
        {
            mocimientosToolStripMenuItem.Enabled = CarWash.Utilidades.clsConnection.EsAdministrador;
            reporteDiarioToolStripMenuItem.Enabled = CarWash.Utilidades.clsConnection.EsAdministrador;
            empresaMenu.Enabled = CarWash.Utilidades.clsConnection.EsAdministrador;
            empresaMenu.Visible = empresaMenu.Enabled;
            facturacionMenu.Visible = facturacionMenu.Enabled = CarWash.Utilidades.clsConnection.EsAdministrador;
        }

        private async Task RefreshCompanyAsync()
        {
            if (UIHelper.EnModoDiseno(this) || readingCompany || IsDisposed) return;
            readingCompany = true;
            try
            {
                var company = await new CarWash.Controladores.EmpresaController().ConsultarAsync(System.Threading.CancellationToken.None);
                if (IsDisposed) return;
                Text = company == null ? "CarWash" : company.NombreComercial + " - CarWash";
                empresaIdentity.Text = company?.NombreComercial ?? "";
                Image? image = null;
                if (company?.LogoBase64 != null)
                    using (var stream = new System.IO.MemoryStream(Convert.FromBase64String(company.LogoBase64)))
                    using (var logo = Image.FromStream(stream)) image = new Bitmap(logo);
                var previous = empresaIdentity.Image; empresaIdentity.Image = image; previous?.Dispose();
            }
            catch { if (!IsDisposed) { Text = "CarWash"; empresaIdentity.Text = ""; var image = empresaIdentity.Image; empresaIdentity.Image = null; image?.Dispose(); } }
            finally { readingCompany = false; }
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode)
            {
                return;
            }

            this.WindowState = FormWindowState.Maximized;
            this.Bounds = Screen.PrimaryScreen.Bounds;
            cargaDashBoard();

        }


        private void AbrirFormulario(Form formulario)
        {
            panelContent.Controls.Clear();

            formulario.TopLevel = false;
            formulario.FormBorderStyle = FormBorderStyle.None;
            formulario.Dock = DockStyle.Fill;

            panelContent.Controls.Add(formulario);
            panelContent.Tag = formulario;

            formulario.Show();
        }


        private void btnDashboard_Click(object sender, EventArgs e)
        {
            cargaDashBoard();
        }

        private void cargaDashBoard()
        {
            AbrirFormularioEnPanel(new FrmDashboard(), splitContainer1.Panel2);
            AbrirFormularioEnPanel(new FrmIngresoVehiculo(), splitContainer1.Panel1);
        }

        private async void FrmPrincipal_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (UIHelper.EnModoDiseno(this)) return;
            try { await CarWash.Controladores.LoginController.CerrarSesionAsync(); }
            finally { Application.Exit(); }
        }

        private void tlsIngresoVehiculo_Click(object sender, EventArgs e)
        {
            Form formularioAbierto = Application.OpenForms["FrmIngresoVehiculo"];

            if (formularioAbierto == null)
            {
                FrmIngresoVehiculo frmIngreso = new FrmIngresoVehiculo();
                frmIngreso.Show();
            }
            else
            {
                formularioAbierto.BringToFront();
            }
        }


        private void AbrirFormularioEnPanel(Form formulario, Control contenedor)
        {
            formulario.TopLevel = false;
            formulario.FormBorderStyle = FormBorderStyle.None;
            formulario.Dock = DockStyle.Left;

            contenedor.Controls.Clear();
            contenedor.Controls.Add(formulario);

            formulario.Show();
        }

        private void abrirCajaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form formularioAbierto = Application.OpenForms["FrmAperturaCaja"];

            if (formularioAbierto == null)
            {
                using (FrmAperturaCaja frmApertura = new FrmAperturaCaja())
                {
                    frmApertura.ShowDialog(this);
                }
            }
            else
            {
                formularioAbierto.BringToFront();
            }


        }

        private void cerrarCajaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form formularioAbierto = Application.OpenForms["FrmCierreCaja"];

            if (formularioAbierto == null)
            {
                using (FrmCierreCaja frmApertura = new FrmCierreCaja())
                {
                    frmApertura.ShowDialog(this);
                }
            }
            else
            {
                formularioAbierto.BringToFront();
            }
        }

        private void regsitrarValesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void registrarPropinaRecibidaQRToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void mocimientosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CarWash.Utilidades.clsConnection.EsAdministrador) return;
            Form formularioAbierto = Application.OpenForms["FrmConsultaMovimientosCaja"];

            if (formularioAbierto == null)
            {
                using (FrmConsultaMovimientosCaja frmApertura = new FrmConsultaMovimientosCaja())
                {
                    frmApertura.ShowDialog(this);
                }
            }
            else
            {
                formularioAbierto.BringToFront();
            }
        }

        private void asistenciaOperariosToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void registrarPropinaRecibidaQRToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form formularioAbierto = Application.OpenForms["FrmRegistroPropinasOperarios"];

            if (formularioAbierto == null)
            {
                using (FrmRegistroPropinasOperarios frmApertura = new FrmRegistroPropinasOperarios())
                {
                    frmApertura.ShowDialog(this);
                }
            }
            else
            {
                formularioAbierto.BringToFront();
            }
        }

        private void registrarValesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form formularioAbierto = Application.OpenForms["FrmValeOperario"];

            if (formularioAbierto == null)
            {
                using (FrmValeOperario frmApertura = new FrmValeOperario())
                {
                    frmApertura.ShowDialog(this);
                }
            }
            else
            {
                formularioAbierto.BringToFront();
            }
        }

        private void asistenciaOperariosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form formularioAbierto = Application.OpenForms["FrmRegistroAsistencia"];

            if (formularioAbierto == null)
            {
                using (FrmRegistroAsistencia frmApertura = new FrmRegistroAsistencia())
                {
                    frmApertura.ShowDialog(this);
                }
            }
            else
            {
                formularioAbierto.BringToFront();
            }
        }

        private void liquidarNóminaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form formularioAbierto = Application.OpenForms["FrmLiquidacionNominaOperarios"];

            if (formularioAbierto == null)
            {
                using (FrmLiquidacionNominaOperarios frmApertura = new FrmLiquidacionNominaOperarios())
                {
                    frmApertura.ShowDialog(this);
                }
            }
            else
            {
                formularioAbierto.BringToFront();
            }

        }

        private void registrarGastoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form formularioAbierto = Application.OpenForms["FrmGastoCaja"];

            if (formularioAbierto == null)
            {
                using (FrmGastoCaja frmApertura = new FrmGastoCaja())
                {
                    frmApertura.ShowDialog(this);
                }
            }
            else
            {
                formularioAbierto.BringToFront();
            }
        }

        private void reporteDiarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CarWash.Utilidades.clsConnection.EsAdministrador) return;
            Form formularioAbierto = Application.OpenForms["FrmReporteDiarioVentas"];

            if (formularioAbierto == null)
            {
                using (FrmReporteDiarioVentas frmReporte = new FrmReporteDiarioVentas())
                {
                    frmReporte.ShowDialog(this);
                }
            }
            else
            {
                formularioAbierto.BringToFront();
            }
        }
    }
}
