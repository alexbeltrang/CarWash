using CarWash.Database;
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
            cargaDashBoard();
            
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
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

        private void btnIngresoVehiculo_Click(object sender, EventArgs e)
        {
            ActivarBoton(btnIngresoVehiculo);
            //AbrirFormulario(new FrmIngresoVehiculo());
            FrmIngresoVehiculo frmIngreso = new FrmIngresoVehiculo();
            frmIngreso.Show();
        }

        private void ActivarBoton(Button btn)
        {
            foreach (Control c in panelMenu.Controls)
            {
                if (c is Button)
                {
                    c.BackColor = Color.FromArgb(8, 35, 64);
                }
            }

            btn.BackColor = Color.FromArgb(15, 76, 129);
        }


        private void btnDashboard_Click(object sender, EventArgs e)
        {
            cargaDashBoard();
        }

        private void cargaDashBoard()
        {
            AbrirFormulario(new FrmDashboard());
        }

        
    }
}
