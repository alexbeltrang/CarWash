using System.Windows.Forms;

namespace CarWash.Presentacion.Administracion
{
    partial class FrmRegistroAsistencia
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblOperario = new System.Windows.Forms.Label();
            this.cmbOperario = new System.Windows.Forms.ComboBox();
            this.lblFecha = new System.Windows.Forms.Label();
            this.dtpFecha = new System.Windows.Forms.DateTimePicker();
            this.grpEstado = new System.Windows.Forms.GroupBox();
            this.rbFaltaInjustificada = new System.Windows.Forms.RadioButton();
            this.rbFaltaAutorizada = new System.Windows.Forms.RadioButton();
            this.rbAsistio = new System.Windows.Forms.RadioButton();
            this.lblObservacion = new System.Windows.Forms.Label();
            this.txtObservacion = new System.Windows.Forms.TextBox();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.grpEstado.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(26, 9);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(348, 37);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "REGISTRO DE ASISTENCIA";
            // 
            // lblOperario
            // 
            this.lblOperario.AutoSize = true;
            this.lblOperario.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblOperario.ForeColor = System.Drawing.Color.White;
            this.lblOperario.Location = new System.Drawing.Point(20, 70);
            this.lblOperario.Name = "lblOperario";
            this.lblOperario.Size = new System.Drawing.Size(81, 23);
            this.lblOperario.TabIndex = 1;
            this.lblOperario.Text = "Operario";
            // 
            // cmbOperario
            // 
            this.cmbOperario.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOperario.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbOperario.Location = new System.Drawing.Point(24, 95);
            this.cmbOperario.Name = "cmbOperario";
            this.cmbOperario.Size = new System.Drawing.Size(350, 31);
            this.cmbOperario.TabIndex = 2;
            // 
            // lblFecha
            // 
            this.lblFecha.AutoSize = true;
            this.lblFecha.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblFecha.ForeColor = System.Drawing.Color.White;
            this.lblFecha.Location = new System.Drawing.Point(20, 140);
            this.lblFecha.Name = "lblFecha";
            this.lblFecha.Size = new System.Drawing.Size(55, 23);
            this.lblFecha.TabIndex = 3;
            this.lblFecha.Text = "Fecha";
            // 
            // dtpFecha
            // 
            this.dtpFecha.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpFecha.Location = new System.Drawing.Point(24, 165);
            this.dtpFecha.Name = "dtpFecha";
            this.dtpFecha.Size = new System.Drawing.Size(350, 30);
            this.dtpFecha.TabIndex = 4;
            // 
            // grpEstado
            // 
            this.grpEstado.Controls.Add(this.rbFaltaInjustificada);
            this.grpEstado.Controls.Add(this.rbFaltaAutorizada);
            this.grpEstado.Controls.Add(this.rbAsistio);
            this.grpEstado.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpEstado.ForeColor = System.Drawing.Color.White;
            this.grpEstado.Location = new System.Drawing.Point(24, 215);
            this.grpEstado.Name = "grpEstado";
            this.grpEstado.Size = new System.Drawing.Size(350, 120);
            this.grpEstado.TabIndex = 5;
            this.grpEstado.TabStop = false;
            this.grpEstado.Text = "Estado";
            // 
            // rbFaltaInjustificada
            // 
            this.rbFaltaInjustificada.AutoSize = true;
            this.rbFaltaInjustificada.Location = new System.Drawing.Point(20, 80);
            this.rbFaltaInjustificada.Name = "rbFaltaInjustificada";
            this.rbFaltaInjustificada.Size = new System.Drawing.Size(172, 27);
            this.rbFaltaInjustificada.TabIndex = 0;
            this.rbFaltaInjustificada.Text = "Falta Injustificada";
            // 
            // rbFaltaAutorizada
            // 
            this.rbFaltaAutorizada.AutoSize = true;
            this.rbFaltaAutorizada.Location = new System.Drawing.Point(20, 55);
            this.rbFaltaAutorizada.Name = "rbFaltaAutorizada";
            this.rbFaltaAutorizada.Size = new System.Drawing.Size(162, 27);
            this.rbFaltaAutorizada.TabIndex = 1;
            this.rbFaltaAutorizada.Text = "Falta Autorizada";
            // 
            // rbAsistio
            // 
            this.rbAsistio.AutoSize = true;
            this.rbAsistio.Location = new System.Drawing.Point(20, 30);
            this.rbAsistio.Name = "rbAsistio";
            this.rbAsistio.Size = new System.Drawing.Size(84, 27);
            this.rbAsistio.TabIndex = 2;
            this.rbAsistio.Text = "Asistió";
            // 
            // lblObservacion
            // 
            this.lblObservacion.AutoSize = true;
            this.lblObservacion.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblObservacion.ForeColor = System.Drawing.Color.White;
            this.lblObservacion.Location = new System.Drawing.Point(20, 350);
            this.lblObservacion.Name = "lblObservacion";
            this.lblObservacion.Size = new System.Drawing.Size(109, 23);
            this.lblObservacion.TabIndex = 6;
            this.lblObservacion.Text = "Observación";
            // 
            // txtObservacion
            // 
            this.txtObservacion.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtObservacion.Location = new System.Drawing.Point(24, 375);
            this.txtObservacion.Multiline = true;
            this.txtObservacion.Name = "txtObservacion";
            this.txtObservacion.Size = new System.Drawing.Size(350, 70);
            this.txtObservacion.TabIndex = 7;
            // 
            // btnGuardar
            // 
            this.btnGuardar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnGuardar.ForeColor = System.Drawing.Color.White;
            this.btnGuardar.Location = new System.Drawing.Point(120, 460);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(150, 40);
            this.btnGuardar.TabIndex = 8;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = false;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // FrmRegistroAsistencia
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(90)))), ((int)(((byte)(150)))));
            this.ClientSize = new System.Drawing.Size(400, 520);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.lblOperario);
            this.Controls.Add(this.cmbOperario);
            this.Controls.Add(this.lblFecha);
            this.Controls.Add(this.dtpFecha);
            this.Controls.Add(this.grpEstado);
            this.Controls.Add(this.lblObservacion);
            this.Controls.Add(this.txtObservacion);
            this.Controls.Add(this.btnGuardar);
            this.Name = "FrmRegistroAsistencia";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Registro de Asistencia";
            this.Load += new System.EventHandler(this.FrmRegistroAsistencia_Load);
            this.grpEstado.ResumeLayout(false);
            this.grpEstado.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lblTitulo;
        private Label lblOperario;
        private ComboBox cmbOperario;
        private Label lblFecha;
        private DateTimePicker dtpFecha;
        private GroupBox grpEstado;
        private RadioButton rbAsistio;
        private RadioButton rbFaltaAutorizada;
        private RadioButton rbFaltaInjustificada;
        private Label lblObservacion;
        private TextBox txtObservacion;
        private Button btnGuardar;
    }
}