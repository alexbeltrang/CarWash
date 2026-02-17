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
            CargarEstadisticas();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void CargarEstadisticas()
        {
            try
            {
                // Clientes
                //int totalClientes = DatabaseQueryLDB.ExecuteScalar<int>(
                //    "SELECT COUNT(*) FROM Cliente");
                int totalClientes = 0;
                lblClientes.Text = "Clientes Registrados: " + totalClientes;

                // Servicios activos (ejemplo campo Estado = 1)
                int serviciosActivos = DatabaseQueryLDB.ExecuteScalar<int>(
                    "SELECT COUNT(*) FROM Servicios WHERE IsDelete = 0");

                lblServicios.Text = "Servicios Activos: " + serviciosActivos;

                // Ventas del día (ejemplo con fecha actual)
                decimal ventasHoy = DatabaseQueryLDB.ExecuteScalar<decimal>(
                    "SELECT IFNULL(SUM(Valor),0) FROM Turnos WHERE DATE(FechaHoraIngreso) = DATE('now')");

                lblVentas.Text = "Ventas Hoy: $" + ventasHoy.ToString("N0");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando estadísticas: " + ex.Message);
            }
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
            AbrirFormulario(new FrmIngresoVehiculo());
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
    }
}
