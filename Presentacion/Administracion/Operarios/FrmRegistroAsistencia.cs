using CarWash.Database;
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

            DatabaseQueryLDB.ExecuteNonQuery(
                @"INSERT INTO AsistenciaOperario (idOperario, Fecha, Asistio, Autorizado, Observacion)
                  VALUES (?,?,?,?,?)",
                cmbOperario.SelectedValue,
                dtpFecha.Value.Date,
                asistio,
                autorizado,
                txtObservacion.Text.Trim()
            );

            MessageBox.Show("Asistencia registrada correctamente");
            limpiaCampos();

        }


        public void CargarOperarios()
        {
            var operarios = DatabaseQueryLDB.ExecuteList<Operarios>("SELECT idOperario, COALESCE(opr.Nombres,'') || ' ' || COALESCE(opr.Apellidos,'') AS Nombres FROM Operarios opr WHERE isDelete = 0");
            operarios.Insert(0, new Operarios
            {
                idOperario = 0,
                Nombres = "-- Seleccione --"
            });


            cmbOperario.DataSource = operarios;
            cmbOperario.DisplayMember = "Nombres";
            cmbOperario.ValueMember = "idOperario";
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
