using CarWash.Database;
using CarWash.DTOs;
using CarWash.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarWash.Presentacion.Administracion
{
    public partial class FrmAperturaCaja : Form
    {
        private List<CajaDiaria> cajaDiaria = new List<CajaDiaria>();


        public FrmAperturaCaja()
        {
            InitializeComponent();
        }


        private void FrmAperturaCaja_Load(object sender, EventArgs e)
        {
            txtMontoInicial.Text = "0";
            cargaCajaDiaria();

        }

        private void btnAbrirCaja_Click(object sender, EventArgs e)
        {
            if (validaProcesoGrabar())
            {
                DatabaseQueryLDB.ExecuteNonQuery(
                        "INSERT INTO CajaDiaria (FechaApertura,MontoInicial,TotalIngresosEfectivo,TotalIngresosTransferencias,TotalIngresosDatafono,TotalEgresos,TotalFinal,Estado) values (?,?,?,?,?,?,?,?)",
                        DateTime.Now,
                        txtMontoInicial.Text.Trim(),
                        0,
                        0,
                        0,
                        0,
                        0,
                        true
                    );

                MostrarToast("Ingreso registrado correctamente", Color.FromArgb(40, 167, 69));
            }
        }

        private void cargaCajaDiaria()
        {
            try
            {
                cajaDiaria = DatabaseQueryLDB.ExecuteList<CajaDiaria>(
                @"SELECT idCaja,FechaApertura,FechaCierre,MontoInicial,TotalIngresosEfectivo,TotalIngresosTransferencias,TotalIngresosDatafono,TotalEgresos,TotalFinal,Estado
                  FROM CajaDiaria
                  WHERE  strftime('%Y-%m-%d',FechaApertura / 10000000 - 62135596800,'unixepoch') = date('now');");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando Cajas Diarias: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool validaProcesoGrabar()
        {
            if (cajaDiaria != null)
            {
                MostrarToast("Ya existe una caja abierta.", Color.FromArgb(40, 167, 69));
                return false;
            }
            return true;

        }

        private void MostrarToast(string mensaje, Color color)
        {
            Panel toast = new Panel();
            toast.Size = new Size(300, 50);
            toast.BackColor = color;
            toast.Left = this.Width - 320;
            toast.Top = 20;

            Label lbl = new Label();
            lbl.Text = mensaje;
            lbl.ForeColor = Color.White;
            lbl.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lbl.AutoSize = false;
            lbl.Dock = DockStyle.Fill;
            lbl.TextAlign = ContentAlignment.MiddleCenter;

            toast.Controls.Add(lbl);
            this.Controls.Add(toast);
            toast.BringToFront();

            Timer timer = new Timer();
            timer.Interval = 3000;
            timer.Tick += (s, e) =>
            {
                this.Controls.Remove(toast);
                timer.Stop();
            };
            timer.Start();
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {

        }

        private void txtMontoInicial_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmAperturaCaja_Shown(object sender, EventArgs e)
        {
            this.Activate();
        }
    }
}
