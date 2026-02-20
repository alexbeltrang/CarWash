using CarWash.Database;
using CarWash.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarWash.Presentacion.Operacion
{
    public partial class FrmAsignarOperador : Form
    {
        public int? OperadorSeleccionadoId { get; private set; }

        public FrmAsignarOperador(string placa, string cliente)
        {
            InitializeComponent();

            lblPlaca.Text = "Placa: " + placa;
            lblCliente.Text = "Cliente: " + cliente;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnAsignar_Click(object sender, EventArgs e)
        {
            if (dvOperadoresDisponibles.CurrentRow == null)
            {
                MessageBox.Show("Debe seleccionar un operador.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            OperariosDTO operadorSeleccionado = dvOperadoresDisponibles.CurrentRow.DataBoundItem as OperariosDTO;

            if (operadorSeleccionado == null)
            {
                MessageBox.Show("No se pudo obtener el operador seleccionado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            OperadorSeleccionadoId = operadorSeleccionado.idOperario;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // Bordes redondeados
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect,
            int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.Region = System.Drawing.Region.FromHrgn(
                CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        private void FrmAsignarOperador_Load(object sender, EventArgs e)
        {
            CargaOperadoresDisponibles();
        }


        private void CargaOperadoresDisponibles()
        {
            try
            {
                var operadoresDisponibles = DatabaseQueryLDB.ExecuteList<OperariosDTO>(
                @"SELECT idOperario,Nombres,Apellidos  
                  FROM Operarios 
                  WHERE idOperario NOT IN (SELECT idOperario FROM Turnos WHERE Estado = 0 AND idOperario IS NOT NULL and OperadorOcupado = 0) ");

                dvOperadoresDisponibles.DataSource = operadoresDisponibles;

                dvOperadoresDisponibles.Columns["idOperario"].Visible = false;
                dvOperadoresDisponibles.Columns["NombreCompleto"].HeaderText = "Nombre Operador";
                dvOperadoresDisponibles.Columns["Nombres"].Visible = false;
                dvOperadoresDisponibles.Columns["Apellidos"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando operadores: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
