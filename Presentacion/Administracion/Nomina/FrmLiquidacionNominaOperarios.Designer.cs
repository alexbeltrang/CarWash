using System.Windows.Forms;

namespace CarWash.Presentacion.Administracion.Nomina
{
    partial class FrmLiquidacionNominaOperarios
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblOperario = new System.Windows.Forms.Label();
            this.cmbOperario = new System.Windows.Forms.ComboBox();
            this.lblFechaInicial = new System.Windows.Forms.Label();
            this.dtpFechaInicial = new System.Windows.Forms.DateTimePicker();
            this.lblFechaFinal = new System.Windows.Forms.Label();
            this.dtpFechaFinal = new System.Windows.Forms.DateTimePicker();
            this.btnCalcular = new System.Windows.Forms.Button();
            this.dtgNominaEmpleados = new System.Windows.Forms.DataGridView();
            this.btnLiquidar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dtgNominaEmpleados)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(350, 10);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(359, 37);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "LIQUIDACIÓN DE NÓMINA";
            // 
            // lblOperario
            // 
            this.lblOperario.AutoSize = true;
            this.lblOperario.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblOperario.ForeColor = System.Drawing.Color.White;
            this.lblOperario.Location = new System.Drawing.Point(20, 80);
            this.lblOperario.Name = "lblOperario";
            this.lblOperario.Size = new System.Drawing.Size(70, 20);
            this.lblOperario.TabIndex = 1;
            this.lblOperario.Text = "Operario";
            // 
            // cmbOperario
            // 
            this.cmbOperario.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOperario.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbOperario.Location = new System.Drawing.Point(20, 105);
            this.cmbOperario.Name = "cmbOperario";
            this.cmbOperario.Size = new System.Drawing.Size(250, 31);
            this.cmbOperario.TabIndex = 2;
            // 
            // lblFechaInicial
            // 
            this.lblFechaInicial.AutoSize = true;
            this.lblFechaInicial.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblFechaInicial.ForeColor = System.Drawing.Color.White;
            this.lblFechaInicial.Location = new System.Drawing.Point(300, 80);
            this.lblFechaInicial.Name = "lblFechaInicial";
            this.lblFechaInicial.Size = new System.Drawing.Size(94, 20);
            this.lblFechaInicial.TabIndex = 3;
            this.lblFechaInicial.Text = "Fecha Inicial";
            // 
            // dtpFechaInicial
            // 
            this.dtpFechaInicial.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpFechaInicial.Location = new System.Drawing.Point(300, 105);
            this.dtpFechaInicial.Name = "dtpFechaInicial";
            this.dtpFechaInicial.Size = new System.Drawing.Size(250, 30);
            this.dtpFechaInicial.TabIndex = 4;
            // 
            // lblFechaFinal
            // 
            this.lblFechaFinal.AutoSize = true;
            this.lblFechaFinal.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblFechaFinal.ForeColor = System.Drawing.Color.White;
            this.lblFechaFinal.Location = new System.Drawing.Point(580, 80);
            this.lblFechaFinal.Name = "lblFechaFinal";
            this.lblFechaFinal.Size = new System.Drawing.Size(86, 20);
            this.lblFechaFinal.TabIndex = 5;
            this.lblFechaFinal.Text = "Fecha Final";
            // 
            // dtpFechaFinal
            // 
            this.dtpFechaFinal.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpFechaFinal.Location = new System.Drawing.Point(580, 105);
            this.dtpFechaFinal.Name = "dtpFechaFinal";
            this.dtpFechaFinal.Size = new System.Drawing.Size(250, 30);
            this.dtpFechaFinal.TabIndex = 6;
            // 
            // btnCalcular
            // 
            this.btnCalcular.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnCalcular.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalcular.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCalcular.ForeColor = System.Drawing.Color.White;
            this.btnCalcular.Location = new System.Drawing.Point(860, 100);
            this.btnCalcular.Name = "btnCalcular";
            this.btnCalcular.Size = new System.Drawing.Size(120, 35);
            this.btnCalcular.TabIndex = 7;
            this.btnCalcular.Text = "Calcular";
            this.btnCalcular.UseVisualStyleBackColor = false;
            this.btnCalcular.Click += new System.EventHandler(this.btnCalcular_Click);
            // 
            // dtgNominaEmpleados
            // 
            this.dtgNominaEmpleados.AllowUserToAddRows = false;
            this.dtgNominaEmpleados.AllowUserToDeleteRows = false;
            this.dtgNominaEmpleados.BackgroundColor = System.Drawing.Color.White;
            this.dtgNominaEmpleados.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(76)))), ((int)(((byte)(129)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            this.dtgNominaEmpleados.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dtgNominaEmpleados.ColumnHeadersHeight = 29;
            this.dtgNominaEmpleados.Location = new System.Drawing.Point(20, 158);
            this.dtgNominaEmpleados.Name = "dtgNominaEmpleados";
            this.dtgNominaEmpleados.ReadOnly = true;
            this.dtgNominaEmpleados.RowHeadersVisible = false;
            this.dtgNominaEmpleados.RowHeadersWidth = 51;
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            this.dtgNominaEmpleados.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dtgNominaEmpleados.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dtgNominaEmpleados.Size = new System.Drawing.Size(960, 395);
            this.dtgNominaEmpleados.TabIndex = 8;
            // 
            // btnLiquidar
            // 
            this.btnLiquidar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnLiquidar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLiquidar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLiquidar.ForeColor = System.Drawing.Color.White;
            this.btnLiquidar.Location = new System.Drawing.Point(392, 559);
            this.btnLiquidar.Name = "btnLiquidar";
            this.btnLiquidar.Size = new System.Drawing.Size(120, 35);
            this.btnLiquidar.TabIndex = 9;
            this.btnLiquidar.Text = "Liquidar";
            this.btnLiquidar.UseVisualStyleBackColor = false;
            this.btnLiquidar.Click += new System.EventHandler(this.btnLiquidar_Click);
            // 
            // FrmLiquidacionNominaOperarios
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(90)))), ((int)(((byte)(150)))));
            this.ClientSize = new System.Drawing.Size(1000, 623);
            this.Controls.Add(this.btnLiquidar);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.lblOperario);
            this.Controls.Add(this.cmbOperario);
            this.Controls.Add(this.lblFechaInicial);
            this.Controls.Add(this.dtpFechaInicial);
            this.Controls.Add(this.lblFechaFinal);
            this.Controls.Add(this.dtpFechaFinal);
            this.Controls.Add(this.btnCalcular);
            this.Controls.Add(this.dtgNominaEmpleados);
            this.Name = "FrmLiquidacionNominaOperarios";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Liquidación de Nómina";
            this.Load += new System.EventHandler(this.FrmLiquidacionNominaOperarios_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtgNominaEmpleados)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lblTitulo;
        private Label lblOperario;
        private ComboBox cmbOperario;
        private Label lblFechaInicial;
        private DateTimePicker dtpFechaInicial;
        private Label lblFechaFinal;
        private DateTimePicker dtpFechaFinal;
        private Button btnCalcular;
        private DataGridView dtgNominaEmpleados;
        private Button btnLiquidar;
    }
}