using CarWash.Controladores;
using CarWash.Presentacion.Principal;
using CarWash.Utilidades;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CarWash.Presentacion.Login
{
    public partial class FrmLogin : Form
    {
        private readonly LoginController _controller = new LoginController();

        public FrmLogin()
        {
            InitializeComponent();
            btnLogin.MouseEnter += (s, e) =>
            {
                btnLogin.BackColor = Color.FromArgb(45, 140, 210);
            };

            btnLogin.MouseLeave += (s, e) =>
            {
                btnLogin.BackColor = Color.FromArgb(31, 111, 178);
            };

            btnLogin.MouseDown += (s, e) =>
            {
                btnLogin.BackColor = Color.FromArgb(20, 90, 150);
            };

            UIHelper.RedondearControl(this, 25);
            UIHelper.RedondearControl(panelCard, 30);

        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {

            using (var ms = new System.IO.MemoryStream(Properties.Resources.splash_car_icon))
            {
                this.Icon = new Icon(ms);
            }
            //            var UserName = FunctionsEncrip.Cifrado(1, "Pipe0825*");
            this.Opacity = 0;
            Timer t = new Timer();
            t.Interval = 20;
            t.Tick += (s, ev) =>
            {
                if (this.Opacity < 1)
                    this.Opacity += 0.05;
                else
                    t.Stop();
            };
            t.Start();
            _controller.InicializarBaseDeDatos();
        }

        private void chkMostrar_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chkMostrar.Checked;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string mensaje;
            if (!_controller.ValidarCampos(txtUsuario.Text, txtPassword.Text, out mensaje))
            {
                MessageBox.Show(mensaje, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
                return;
            }

            var resp = _controller.Login(txtUsuario.Text, txtPassword.Text);
            if (!resp.esValido)
            {
                MessageBox.Show(resp.respuesta, "Error Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsConnection.idUser = resp.Usuario.idUser;
            clsConnection.intCodigoPerfil = resp.Usuario.PerfilId;
            clsConnection.strNombreUsuario = resp.Usuario.DisplayName;
            clsConnection.strEmailUsuario = resp.Usuario.Email.ToLower();
            this.Visible = false;
            FrmPrincipal frmPrincipalForm = new FrmPrincipal();
            frmPrincipalForm.Show();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                this.ClientRectangle,
                Color.FromArgb(5, 25, 45),
                Color.FromArgb(20, 90, 150),
                45F))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= 0x20000; // Sombra
                return cp;
            }
        }


    }
}
