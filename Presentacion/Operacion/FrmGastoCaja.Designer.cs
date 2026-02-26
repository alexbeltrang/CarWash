using System.Windows.Forms;

namespace CarWash.Presentacion.Operacion
{
    partial class FrmGastoCaja
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private Panel panelCard;
        private Label lblTitulo;
        private Label lblConcepto;
        private Label lblValor;
        private Label lblObservacion;

        private TextBox txtConcepto;
        private TextBox txtValor;
        private TextBox txtObservacion;

        private Button btnGuardar;
        private Button btnCancelar;

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
            this.panelCard = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblConcepto = new System.Windows.Forms.Label();
            this.txtConcepto = new System.Windows.Forms.TextBox();
            this.lblValor = new System.Windows.Forms.Label();
            this.txtValor = new System.Windows.Forms.TextBox();
            this.lblObservacion = new System.Windows.Forms.Label();
            this.txtObservacion = new System.Windows.Forms.TextBox();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.panelCard.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelCard
            // 
            this.panelCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(76)))), ((int)(((byte)(129)))));
            this.panelCard.Controls.Add(this.lblTitulo);
            this.panelCard.Controls.Add(this.lblConcepto);
            this.panelCard.Controls.Add(this.txtConcepto);
            this.panelCard.Controls.Add(this.lblValor);
            this.panelCard.Controls.Add(this.txtValor);
            this.panelCard.Controls.Add(this.lblObservacion);
            this.panelCard.Controls.Add(this.txtObservacion);
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
            this.lblTitulo.Location = new System.Drawing.Point(80, 20);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(258, 37);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "REGISTRAR GASTO";
            // 
            // lblConcepto
            // 
            this.lblConcepto.ForeColor = System.Drawing.Color.White;
            this.lblConcepto.Location = new System.Drawing.Point(40, 90);
            this.lblConcepto.Name = "lblConcepto";
            this.lblConcepto.Size = new System.Drawing.Size(100, 23);
            this.lblConcepto.TabIndex = 1;
            this.lblConcepto.Text = "Concepto";
            // 
            // txtConcepto
            // 
            this.txtConcepto.Location = new System.Drawing.Point(40, 115);
            this.txtConcepto.MaxLength = 50;
            this.txtConcepto.Name = "txtConcepto";
            this.txtConcepto.Size = new System.Drawing.Size(300, 22);
            this.txtConcepto.TabIndex = 2;
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
            this.txtValor.Location = new System.Drawing.Point(40, 175);
            this.txtValor.MaxLength = 10;
            this.txtValor.Name = "txtValor";
            this.txtValor.Size = new System.Drawing.Size(300, 22);
            this.txtValor.TabIndex = 4;
            this.txtValor.Enter += new System.EventHandler(this.txtValor_Enter);
            this.txtValor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtValor_KeyDown);
            this.txtValor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtValor_KeyPress);
            // 
            // lblObservacion
            // 
            this.lblObservacion.ForeColor = System.Drawing.Color.White;
            this.lblObservacion.Location = new System.Drawing.Point(40, 210);
            this.lblObservacion.Name = "lblObservacion";
            this.lblObservacion.Size = new System.Drawing.Size(100, 23);
            this.lblObservacion.TabIndex = 5;
            this.lblObservacion.Text = "Observaciones";
            // 
            // txtObservacion
            // 
            this.txtObservacion.Location = new System.Drawing.Point(40, 234);
            this.txtObservacion.MaxLength = 200;
            this.txtObservacion.Multiline = true;
            this.txtObservacion.Name = "txtObservacion";
            this.txtObservacion.Size = new System.Drawing.Size(300, 60);
            this.txtObservacion.TabIndex = 6;
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
            // FrmGastoCaja
            // 
            this.AcceptButton = this.btnGuardar;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(35)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(460, 460);
            this.Controls.Add(this.panelCard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FrmGastoCaja";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gasto Caja";
            this.panelCard.ResumeLayout(false);
            this.panelCard.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
    }
}