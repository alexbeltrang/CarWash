using CarWash.Database;
using CarWash.Entidades;
using CarWash.Presentacion;
using CarWash.Presentacion.Principal;
using CarWash.Utilidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarWash.Presentacion.Login
{
    public partial class FrmLogin : Form
    {
        private static string NameDatabase = ConfigurationManager.AppSettings["NameLDB"];
        private static string dbFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, NameDatabase);
        string strMensaje = string.Empty;
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
            creaBaseLocal();
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
            if (validaCampos())
            {
                DataTable dttConexion = new DataTable();
                ingresaAplicativo(txtUsuario.Text, txtPassword.Text);
            }
        }


        private bool validaCampos()
        {
            if (txtUsuario.Text == "" | txtUsuario.Text == null)
            {
                strMensaje = "Digite el " + lblUsuario.Text;
                MessageBox.Show(strMensaje, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
                return false;
            }
            else if (txtPassword.Text == "" | txtPassword.Text == null)
            {
                strMensaje = "Digite la " + lblPassword.Text;
                MessageBox.Show(strMensaje, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
                return false;
            }
            else
                return true;
        }



        private void ingresaAplicativo(string strUser, string strPassword)
        {
            string usrCif = FunctionsEncrip.Cifrado(1, strUser);
            string pwdCif = FunctionsEncrip.Cifrado(1, strPassword);

            var resp = DatabaseQueryLDB.Login(usrCif, pwdCif);
            if (!resp.esValido)
            {
                MessageBox.Show(resp.respuesta, "Error Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                clsConnection.idUser = resp.Usuario.idUser;
                clsConnection.intCodigoPerfil = resp.Usuario.PerfilId;
                clsConnection.strNombreUsuario = resp.Usuario.DisplayName;
                clsConnection.strEmailUsuario = resp.Usuario.Email.ToLower();
                this.Visible = false;
                FrmPrincipal frmPrincipalForm = new FrmPrincipal();
                frmPrincipalForm.Show();
            }
        }

        private void creaBaseLocal()
        {
            DatabaseHelper.CreateOrUpdateTable<Modulo>();
            DatabaseHelper.CreateOrUpdateTable<TurnosDiarios>();
            DatabaseHelper.CreateOrUpdateTable<Usuario>();
            DatabaseHelper.CreateOrUpdateTable<CajaDiaria>();
            DatabaseHelper.CreateOrUpdateTable<Operarios>();
            DatabaseHelper.CreateOrUpdateTable<Perfil>();
            DatabaseHelper.CreateOrUpdateTable<PerfilModulo>();
            DatabaseHelper.CreateOrUpdateTable<Servicios>();
            DatabaseHelper.CreateOrUpdateTable<TipoVehiculo>();
            DatabaseHelper.CreateOrUpdateTable<Turnos>();
            DatabaseHelper.CreateOrUpdateTable<PrecioServicioVehiculo>();
            DatabaseHelper.CreateOrUpdateTable<JornadaOperario>();

            //creacion del indice para las tablas 

            //using (var conn = new SQLite.SQLiteConnection(dbFile))
            //{
            //    conn.Execute(@"CREATE INDEX IF NOT EXISTS idx_Bills_Document_Id_Status ON Bills (Document_Id, Document_Status, Accounting_Document_No, Supplier,FechaCargaInformacion);");
            //    conn.Execute(@"CREATE INDEX IF NOT EXISTS idx_CodingDetails_Document_Number ON CodingDetails (Document_Number, GL_Account,FechaCargaInformacion);");
            //}
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
