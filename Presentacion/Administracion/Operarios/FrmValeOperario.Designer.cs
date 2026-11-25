using System.Windows.Forms;

namespace CarWash.Presentacion.Operacion
{
    partial class FrmValeOperario
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private Panel panelCard;
        private Label lblTitulo;
        private Label lblOperario;
        private Label lblValor;
        private Label lblMotivo;

        private ComboBox cmbOperario;
        private TextBox txtValor;
        private TextBox txtMotivo;

        private Button btnGuardar;
        private Button btnCancelar;
        private ComboBox medio;
        private Label lblMedio;
        private Label estado;
        private Button btnConsultar;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing) { if (!lifetime.IsCancellationRequested) lifetime.Cancel(); lifetime.Dispose(); }
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
            this.medio = new ComboBox();
            this.lblMedio = new Label();
            this.estado = new Label();
            this.btnConsultar = new Button();
            this.panelCard = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblOperario = new System.Windows.Forms.Label();
            this.cmbOperario = new System.Windows.Forms.ComboBox();
            this.lblValor = new System.Windows.Forms.Label();
            this.txtValor = new System.Windows.Forms.TextBox();
            this.lblMotivo = new System.Windows.Forms.Label();
            this.txtMotivo = new System.Windows.Forms.TextBox();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.panelCard.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelCard
            // 
            this.panelCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(76)))), ((int)(((byte)(129)))));
            this.panelCard.Controls.Add(this.lblTitulo);
            this.panelCard.Controls.Add(this.lblOperario);
            this.panelCard.Controls.Add(this.cmbOperario);
            this.panelCard.Controls.Add(this.lblValor);
            this.panelCard.Controls.Add(this.txtValor);
            this.panelCard.Controls.Add(this.lblMotivo);
            this.panelCard.Controls.Add(this.txtMotivo);
            this.panelCard.Controls.Add(this.btnGuardar);
            this.panelCard.Controls.Add(this.btnCancelar);
            this.panelCard.Location = new System.Drawing.Point(40, 40);
            this.panelCard.Name = "panelCard";
            this.panelCard.Size = new System.Drawing.Size(380, 380);
            this.panelCard.TabIndex = 0;
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(90, 20);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(234, 37);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "REGISTRAR VALE";
            // 
            // lblOperario
            // 
            this.lblOperario.ForeColor = System.Drawing.Color.White;
            this.lblOperario.Location = new System.Drawing.Point(40, 90);
            this.lblOperario.Name = "lblOperario";
            this.lblOperario.Size = new System.Drawing.Size(100, 23);
            this.lblOperario.TabIndex = 1;
            this.lblOperario.Text = "Operario";
            // 
            // cmbOperario
            // 
            this.cmbOperario.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOperario.Location = new System.Drawing.Point(40, 115);
            this.cmbOperario.Name = "cmbOperario";
            this.cmbOperario.Size = new System.Drawing.Size(300, 24);
            this.cmbOperario.TabIndex = 2;
            // 
            // lblValor
            // 
            this.lblValor.ForeColor = System.Drawing.Color.White;
            this.lblValor.Location = new System.Drawing.Point(40, 150);
            this.lblValor.Name = "lblValor";
            this.lblValor.Size = new System.Drawing.Size(100, 23);
            this.lblValor.TabIndex = 3;
            this.lblValor.Text = "Valor";
            // 
            // txtValor
            // 
            this.txtValor.Location = new System.Drawing.Point(40, 177);
            this.txtValor.MaxLength = 10;
            this.txtValor.Name = "txtValor";
            this.txtValor.Size = new System.Drawing.Size(300, 22);
            this.txtValor.TabIndex = 4;
            this.txtValor.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtValor_MouseClick);
            this.txtValor.Enter += new System.EventHandler(this.txtValor_Enter);
            this.txtValor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtValor_KeyDown);
            this.txtValor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtValor_KeyPress);
            // 
            // lblMotivo
            // 
            this.lblMotivo.ForeColor = System.Drawing.Color.White;
            this.lblMotivo.Location = new System.Drawing.Point(40, 210);
            this.lblMotivo.Name = "lblMotivo";
            this.lblMotivo.Size = new System.Drawing.Size(100, 23);
            this.lblMotivo.TabIndex = 5;
            this.lblMotivo.Text = "Motivo";
            // 
            // txtMotivo
            // 
            this.txtMotivo.Location = new System.Drawing.Point(40, 234);
            this.txtMotivo.MaxLength = 200;
            this.txtMotivo.Multiline = true;
            this.txtMotivo.Name = "txtMotivo";
            this.txtMotivo.Size = new System.Drawing.Size(300, 60);
            this.txtMotivo.TabIndex = 6;
            // 
            // btnGuardar
            // 
            this.btnGuardar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(111)))), ((int)(((byte)(178)))));
            this.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardar.ForeColor = System.Drawing.Color.White;
            this.btnGuardar.Location = new System.Drawing.Point(40, 310);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(120, 35);
            this.btnGuardar.TabIndex = 7;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = false;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.ForeColor = System.Drawing.Color.White;
            this.btnCancelar.Location = new System.Drawing.Point(220, 310);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(120, 35);
            this.btnCancelar.TabIndex = 8;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // FrmValeOperario
            // 
            this.medio.Name = "medio";
            this.medio.DropDownStyle = ComboBoxStyle.DropDownList;
            this.medio.Location = new System.Drawing.Point(40, 328);
            this.medio.Size = new System.Drawing.Size(300, 24);
            this.medio.TabIndex = 7;
            this.lblMedio.Text = "Medio de salida";
            this.lblMedio.ForeColor = System.Drawing.Color.White;
            this.lblMedio.Location = new System.Drawing.Point(40, 303);
            this.lblMedio.Size = new System.Drawing.Size(300, 23);
            this.estado.Name = "estado";
            this.estado.ForeColor = System.Drawing.Color.White;
            this.estado.Location = new System.Drawing.Point(40, 363);
            this.estado.Size = new System.Drawing.Size(300, 65);
            this.estado.Text = "Consultando jornada...";
            this.btnGuardar.Location = new System.Drawing.Point(20, 440);
            this.btnGuardar.Size = new System.Drawing.Size(200, 40);
            this.btnGuardar.TabIndex = 8;
            this.btnCancelar.Location = new System.Drawing.Point(230, 440);
            this.btnCancelar.Size = new System.Drawing.Size(130, 40);
            this.btnCancelar.TabIndex = 9;
            this.btnConsultar.Name = "btnConsultar";
            this.btnConsultar.Text = "Consultar";
            this.btnConsultar.Location = new System.Drawing.Point(40, 492);
            this.btnConsultar.Size = new System.Drawing.Size(300, 35);
            this.btnConsultar.TabIndex = 10;
            this.btnConsultar.Click += new System.EventHandler(this.btnConsultar_Click);
            this.panelCard.Controls.Add(this.medio);
            this.panelCard.Controls.Add(this.lblMedio);
            this.panelCard.Controls.Add(this.estado);
            this.panelCard.Controls.Add(this.btnConsultar);
            this.panelCard.Size = new System.Drawing.Size(380, 545);
            this.CancelButton = this.btnCancelar;
            this.AcceptButton = this.btnGuardar;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(35)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(460, 625);
            this.MinimumSize = new System.Drawing.Size(476, 664);
            this.Controls.Add(this.panelCard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FrmValeOperario";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Vale Operario";
            this.Load += new System.EventHandler(this.FrmValeOperario_Load);
            this.panelCard.ResumeLayout(false);
            this.panelCard.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
    }
}