using CarWash.Database;
using CarWash.Presentacion.Administracion;
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

        public FrmPrincipal()
        {
            InitializeComponent();
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
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

        private void FrmPrincipal_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
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
            formulario.Dock = DockStyle.Fill;

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

        }
    }
}
