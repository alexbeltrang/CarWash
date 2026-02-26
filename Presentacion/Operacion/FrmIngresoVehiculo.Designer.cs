using System.Drawing;
using System.Windows.Forms;

namespace CarWash.Presentacion.Operacion
{
    partial class FrmIngresoVehiculo
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle41 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle42 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle43 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle44 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmIngresoVehiculo));
            this.lblCelular = new System.Windows.Forms.Label();
            this.txtCelular = new System.Windows.Forms.TextBox();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblPlaca = new System.Windows.Forms.Label();
            this.txtPlaca = new System.Windows.Forms.TextBox();
            this.lblCliente = new System.Windows.Forms.Label();
            this.txtCliente = new System.Windows.Forms.TextBox();
            this.lblTipoVehiculo = new System.Windows.Forms.Label();
            this.cmbTipoVehiculo = new System.Windows.Forms.ComboBox();
            this.lblValor = new System.Windows.Forms.Label();
            this.txtValor = new System.Windows.Forms.TextBox();
            this.lblObservaciones = new System.Windows.Forms.Label();
            this.txtObservaciones = new System.Windows.Forms.TextBox();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.lblHistorico = new System.Windows.Forms.Label();
            this.dgvHistorico = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.txtValorBase = new System.Windows.Forms.TextBox();
            this.lstServiciosVehiculo = new System.Windows.Forms.CheckedListBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorico)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCelular
            // 
            this.lblCelular.AutoSize = true;
            this.lblCelular.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCelular.ForeColor = System.Drawing.Color.White;
            this.lblCelular.Location = new System.Drawing.Point(20, 132);
            this.lblCelular.Name = "lblCelular";
            this.lblCelular.Size = new System.Drawing.Size(62, 20);
            this.lblCelular.TabIndex = 36;
            this.lblCelular.Text = "Celular:";
            // 
            // txtCelular
            // 
            this.txtCelular.BackColor = System.Drawing.Color.White;
            this.txtCelular.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCelular.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtCelular.Location = new System.Drawing.Point(170, 132);
            this.txtCelular.Name = "txtCelular";
            this.txtCelular.Size = new System.Drawing.Size(264, 30);
            this.txtCelular.TabIndex = 3;
            this.txtCelular.Tag = "opcional";
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(249, 10);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(310, 37);
            this.lblTitulo.TabIndex = 20;
            this.lblTitulo.Text = "REGISTRO DE INGRESO";
            // 
            // lblPlaca
            // 
            this.lblPlaca.AutoSize = true;
            this.lblPlaca.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblPlaca.ForeColor = System.Drawing.Color.White;
            this.lblPlaca.Location = new System.Drawing.Point(20, 61);
            this.lblPlaca.Name = "lblPlaca";
            this.lblPlaca.Size = new System.Drawing.Size(49, 20);
            this.lblPlaca.TabIndex = 21;
            this.lblPlaca.Text = "Placa:";
            // 
            // txtPlaca
            // 
            this.txtPlaca.BackColor = System.Drawing.Color.White;
            this.txtPlaca.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPlaca.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtPlaca.Location = new System.Drawing.Point(170, 61);
            this.txtPlaca.Name = "txtPlaca";
            this.txtPlaca.Size = new System.Drawing.Size(264, 30);
            this.txtPlaca.TabIndex = 1;
            this.txtPlaca.Enter += new System.EventHandler(this.txtPlaca_Enter);
            this.txtPlaca.Leave += new System.EventHandler(this.txtPlaca_Leave);
            // 
            // lblCliente
            // 
            this.lblCliente.AutoSize = true;
            this.lblCliente.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCliente.ForeColor = System.Drawing.Color.White;
            this.lblCliente.Location = new System.Drawing.Point(20, 97);
            this.lblCliente.Name = "lblCliente";
            this.lblCliente.Size = new System.Drawing.Size(123, 20);
            this.lblCliente.TabIndex = 23;
            this.lblCliente.Text = "Nombre Cliente:";
            // 
            // txtCliente
            // 
            this.txtCliente.BackColor = System.Drawing.Color.White;
            this.txtCliente.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCliente.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtCliente.Location = new System.Drawing.Point(170, 97);
            this.txtCliente.Name = "txtCliente";
            this.txtCliente.Size = new System.Drawing.Size(264, 30);
            this.txtCliente.TabIndex = 2;
            this.txtCliente.Tag = "opcional";
            // 
            // lblTipoVehiculo
            // 
            this.lblTipoVehiculo.AutoSize = true;
            this.lblTipoVehiculo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblTipoVehiculo.ForeColor = System.Drawing.Color.White;
            this.lblTipoVehiculo.Location = new System.Drawing.Point(20, 168);
            this.lblTipoVehiculo.Name = "lblTipoVehiculo";
            this.lblTipoVehiculo.Size = new System.Drawing.Size(107, 20);
            this.lblTipoVehiculo.TabIndex = 25;
            this.lblTipoVehiculo.Text = "Tipo Vehículo:";
            // 
            // cmbTipoVehiculo
            // 
            this.cmbTipoVehiculo.BackColor = System.Drawing.Color.White;
            this.cmbTipoVehiculo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTipoVehiculo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbTipoVehiculo.Location = new System.Drawing.Point(170, 168);
            this.cmbTipoVehiculo.MaxDropDownItems = 10;
            this.cmbTipoVehiculo.Name = "cmbTipoVehiculo";
            this.cmbTipoVehiculo.Size = new System.Drawing.Size(264, 31);
            this.cmbTipoVehiculo.TabIndex = 4;
            this.cmbTipoVehiculo.SelectionChangeCommitted += new System.EventHandler(this.cmbTipoVehiculo_SelectionChangeCommitted);
            // 
            // lblValor
            // 
            this.lblValor.AutoSize = true;
            this.lblValor.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblValor.ForeColor = System.Drawing.Color.White;
            this.lblValor.Location = new System.Drawing.Point(19, 205);
            this.lblValor.Name = "lblValor";
            this.lblValor.Size = new System.Drawing.Size(50, 20);
            this.lblValor.TabIndex = 29;
            this.lblValor.Text = "Valor:";
            // 
            // txtValor
            // 
            this.txtValor.BackColor = System.Drawing.Color.White;
            this.txtValor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtValor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtValor.Location = new System.Drawing.Point(169, 205);
            this.txtValor.Name = "txtValor";
            this.txtValor.Size = new System.Drawing.Size(264, 30);
            this.txtValor.TabIndex = 6;
            this.txtValor.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtValor_MouseClick);
            this.txtValor.Enter += new System.EventHandler(this.TextBox_SelectAll);
            this.txtValor.Leave += new System.EventHandler(this.txtValor_Leave);
            // 
            // lblObservaciones
            // 
            this.lblObservaciones.AutoSize = true;
            this.lblObservaciones.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblObservaciones.ForeColor = System.Drawing.Color.White;
            this.lblObservaciones.Location = new System.Drawing.Point(19, 276);
            this.lblObservaciones.Name = "lblObservaciones";
            this.lblObservaciones.Size = new System.Drawing.Size(115, 20);
            this.lblObservaciones.TabIndex = 32;
            this.lblObservaciones.Text = "Observaciones:";
            // 
            // txtObservaciones
            // 
            this.txtObservaciones.BackColor = System.Drawing.Color.White;
            this.txtObservaciones.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtObservaciones.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtObservaciones.Location = new System.Drawing.Point(169, 276);
            this.txtObservaciones.MaxLength = 800;
            this.txtObservaciones.Multiline = true;
            this.txtObservaciones.Name = "txtObservaciones";
            this.txtObservaciones.Size = new System.Drawing.Size(264, 95);
            this.txtObservaciones.TabIndex = 8;
            this.txtObservaciones.Tag = "opcional";
            // 
            // btnGuardar
            // 
            this.btnGuardar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnGuardar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGuardar.FlatAppearance.BorderSize = 0;
            this.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnGuardar.ForeColor = System.Drawing.Color.White;
            this.btnGuardar.Location = new System.Drawing.Point(258, 426);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(100, 35);
            this.btnGuardar.TabIndex = 9;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = false;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelar.FlatAppearance.BorderSize = 0;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancelar.ForeColor = System.Drawing.Color.White;
            this.btnCancelar.Location = new System.Drawing.Point(388, 426);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(100, 35);
            this.btnCancelar.TabIndex = 10;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // lblHistorico
            // 
            this.lblHistorico.AutoSize = true;
            this.lblHistorico.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblHistorico.ForeColor = System.Drawing.Color.White;
            this.lblHistorico.Location = new System.Drawing.Point(13, 438);
            this.lblHistorico.Name = "lblHistorico";
            this.lblHistorico.Size = new System.Drawing.Size(189, 23);
            this.lblHistorico.TabIndex = 37;
            this.lblHistorico.Text = "Histórico del Vehículo:";
            // 
            // dgvHistorico
            // 
            this.dgvHistorico.AllowUserToAddRows = false;
            this.dgvHistorico.AllowUserToDeleteRows = false;
            this.dgvHistorico.AllowUserToResizeColumns = false;
            this.dgvHistorico.AllowUserToResizeRows = false;
            dataGridViewCellStyle41.ForeColor = System.Drawing.Color.Black;
            this.dgvHistorico.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle41;
            this.dgvHistorico.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvHistorico.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvHistorico.BackgroundColor = System.Drawing.Color.White;
            this.dgvHistorico.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle42.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle42.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(76)))), ((int)(((byte)(129)))));
            dataGridViewCellStyle42.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle42.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle42.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle42.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle42.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvHistorico.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle42;
            this.dgvHistorico.ColumnHeadersHeight = 29;
            dataGridViewCellStyle43.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle43.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle43.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle43.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle43.SelectionBackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle43.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle43.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvHistorico.DefaultCellStyle = dataGridViewCellStyle43;
            this.dgvHistorico.EnableHeadersVisualStyles = false;
            this.dgvHistorico.Location = new System.Drawing.Point(14, 482);
            this.dgvHistorico.Name = "dgvHistorico";
            this.dgvHistorico.ReadOnly = true;
            this.dgvHistorico.RowHeadersVisible = false;
            this.dgvHistorico.RowHeadersWidth = 51;
            dataGridViewCellStyle44.ForeColor = System.Drawing.Color.Black;
            this.dgvHistorico.RowsDefaultCellStyle = dataGridViewCellStyle44;
            this.dgvHistorico.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvHistorico.Size = new System.Drawing.Size(675, 178);
            this.dgvHistorico.TabIndex = 38;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(19, 240);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 20);
            this.label1.TabIndex = 39;
            this.label1.Text = "Valor Base:";
            // 
            // txtValorBase
            // 
            this.txtValorBase.BackColor = System.Drawing.Color.White;
            this.txtValorBase.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtValorBase.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtValorBase.Location = new System.Drawing.Point(169, 240);
            this.txtValorBase.Name = "txtValorBase";
            this.txtValorBase.Size = new System.Drawing.Size(264, 30);
            this.txtValorBase.TabIndex = 7;
            this.txtValorBase.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtValorBase_MouseClick);
            this.txtValorBase.Enter += new System.EventHandler(this.TextBox_SelectAll);
            this.txtValorBase.Leave += new System.EventHandler(this.txtValorBase_Leave);
            // 
            // lstServiciosVehiculo
            // 
            this.lstServiciosVehiculo.CheckOnClick = true;
            this.lstServiciosVehiculo.FormattingEnabled = true;
            this.lstServiciosVehiculo.Location = new System.Drawing.Point(441, 61);
            this.lstServiciosVehiculo.Name = "lstServiciosVehiculo";
            this.lstServiciosVehiculo.Size = new System.Drawing.Size(248, 310);
            this.lstServiciosVehiculo.TabIndex = 5;
            this.lstServiciosVehiculo.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lstServiciosVehiculo_ItemCheck);
            // 
            // FrmIngresoVehiculo
            // 
            this.AcceptButton = this.btnGuardar;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(90)))), ((int)(((byte)(150)))));
            this.ClientSize = new System.Drawing.Size(806, 670);
            this.Controls.Add(this.lstServiciosVehiculo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtValorBase);
            this.Controls.Add(this.lblCelular);
            this.Controls.Add(this.txtCelular);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.lblPlaca);
            this.Controls.Add(this.txtPlaca);
            this.Controls.Add(this.lblCliente);
            this.Controls.Add(this.txtCliente);
            this.Controls.Add(this.lblTipoVehiculo);
            this.Controls.Add(this.cmbTipoVehiculo);
            this.Controls.Add(this.lblValor);
            this.Controls.Add(this.txtValor);
            this.Controls.Add(this.lblObservaciones);
            this.Controls.Add(this.txtObservaciones);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.lblHistorico);
            this.Controls.Add(this.dgvHistorico);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmIngresoVehiculo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Registrar Ingreso de Vehículo";
            this.Load += new System.EventHandler(this.FrmIngresoVehiculo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorico)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private Label lblCelular;
        private TextBox txtCelular;
        private Label lblTitulo;
        private Label lblPlaca;
        private TextBox txtPlaca;
        private Label lblCliente;
        private TextBox txtCliente;
        private Label lblTipoVehiculo;
        private ComboBox cmbTipoVehiculo;
        private Label lblValor;
        private TextBox txtValor;
        private Label lblObservaciones;
        private TextBox txtObservaciones;
        private Button btnGuardar;
        private Button btnCancelar;
        private Label lblHistorico;
        private DataGridView dgvHistorico;
        private Label label1;
        private TextBox txtValorBase;
        private CheckedListBox lstServiciosVehiculo;
    }
}