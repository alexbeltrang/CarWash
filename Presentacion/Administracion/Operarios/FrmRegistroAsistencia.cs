using CarWash.Controladores;
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
    public partial class FrmRegistroAsistencia : Form
    {
        OperariosController operariosController = new OperariosController();
        AsistenciaOperarioController asistenciaOperarioController = new AsistenciaOperarioController();
        public FrmRegistroAsistencia()
        {
            InitializeComponent();
        }
        private void FrmRegistroAsistencia_Load(object sender, EventArgs e)
        {
            CargarOperarios();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            bool asistio = rbAsistio.Checked;
            bool autorizado = rbFaltaAutorizada.Checked;

            asistenciaOperarioController.RegistrarAsistenciaOperario(new AsistenciaOperario
            {
                idOperario = (int)cmbOperario.SelectedValue,
                Fecha = dtpFecha.Value.Date,
                Asistio = asistio,
                Autorizado = autorizado,
                Observacion = txtObservacion.Text.Trim()
            });

            MessageBox.Show("Asistencia registrada correctamente");
            limpiaCampos();

        }


        public void CargarOperarios()
        {

            var operarios = operariosController.GetOperariosActivos();

            var operadoresDisponibles = operarios
                            .Select(o => new OperariosDTO
                            {
                                idOperario = o.idOperario,
                                Nombres = o.Nombres,
                                Apellidos = o.Apellidos
                            })
                            .ToList();

            operadoresDisponibles.Insert(0, new OperariosDTO
            {
                idOperario = 0,
                Nombres = "-- Seleccione --",
                Apellidos = ""
            });

            cmbOperario.DataSource = operadoresDisponibles;
            cmbOperario.DisplayMember = "NombreCompleto";
            cmbOperario.ValueMember = "idOperario";
            cmbOperario.SelectedIndex = 0;
        }

        public void limpiaCampos()
        {
            cmbOperario.SelectedIndex = -1;
            dtpFecha.Value = DateTime.Now;
            rbAsistio.Checked = false;
            rbFaltaAutorizada.Checked = false;
            txtObservacion.Clear();
        }


    }
}
