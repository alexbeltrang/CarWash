using CarWash.Presentacion.Operacion;
using System.Windows.Forms;

namespace CarWash.Presentacion.Consultas
{
    partial class FrmConsultaMovimientosCaja
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
            if (disposing && !vida.IsCancellationRequested) vida.Cancel();
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblTipoVehiculo = new System.Windows.Forms.Label();
            this.cmbFormaPago = new System.Windows.Forms.ComboBox();
            this.lblHistorico = new System.Windows.Forms.Label();
            this.dtgMovimientos = new System.Windows.Forms.DataGridView();
            this.dtpFechaInicial = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpFechaFinal = new System.Windows.Forms.DateTimePicker();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblOperario = new System.Windows.Forms.Label();
            this.cmbOperario = new System.Windows.Forms.ComboBox();
            this.accionesAdmin = new TableLayoutPanel();
            this.cajaConsulta = new NumericUpDown();
            this.paginaConsulta = new NumericUpDown();
            this.cargarCaja = new Button();
            this.alcance = new ComboBox();
            this.verImpacto = new Button();
            this.impactoDetalle = new TextBox();
            this.importeCorreccion = new NumericUpDown();
            this.medioCorreccion = new ComboBox();
            this.motivoAdmin = new TextBox();
            this.corregir = new Button();
            this.borrar = new Button();
            this.estadoAdmin = new Label();
            this.cargarCierres = new Button();
            this.verCierre = new Button();
            this.reabrir = new Button();
            this.recerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)(this.dtgMovimientos)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(210, 9);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(355, 37);
            this.lblTitulo.TabIndex = 20;
            this.lblTitulo.Text = "CONSULTA MOVIMIENTOS";
            // 
            // lblTipoVehiculo
            // 
            this.lblTipoVehiculo.AutoSize = true;
            this.lblTipoVehiculo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblTipoVehiculo.ForeColor = System.Drawing.Color.White;
            this.lblTipoVehiculo.Location = new System.Drawing.Point(3, 88);
            this.lblTipoVehiculo.Name = "lblTipoVehiculo";
            this.lblTipoVehiculo.Size = new System.Drawing.Size(114, 20);
            this.lblTipoVehiculo.TabIndex = 25;
            this.lblTipoVehiculo.Text = "Forma de Pago";
            // 
            // cmbFormaPago
            // 
            this.cmbFormaPago.BackColor = System.Drawing.Color.White;
            this.cmbFormaPago.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFormaPago.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbFormaPago.Location = new System.Drawing.Point(6, 111);
            this.cmbFormaPago.MaxDropDownItems = 10;
            this.cmbFormaPago.Name = "cmbFormaPago";
            this.cmbFormaPago.Size = new System.Drawing.Size(210, 31);
            this.cmbFormaPago.TabIndex = 4;
            // 
            // lblHistorico
            // 
            this.lblHistorico.AutoSize = true;
            this.lblHistorico.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblHistorico.ForeColor = System.Drawing.Color.White;
            this.lblHistorico.Location = new System.Drawing.Point(10, 253);
            this.lblHistorico.Name = "lblHistorico";
            this.lblHistorico.Size = new System.Drawing.Size(189, 23);
            this.lblHistorico.TabIndex = 37;
            this.lblHistorico.Text = "Movimientos / versiones de cierre:";
            // 
            // dtgMovimientos
            // 
            this.dtgMovimientos.AllowUserToAddRows = false;
            this.dtgMovimientos.AllowUserToDeleteRows = false;
            this.dtgMovimientos.AllowUserToResizeColumns = false;
            this.dtgMovimientos.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            this.dtgMovimientos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dtgMovimientos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtgMovimientos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dtgMovimientos.BackgroundColor = System.Drawing.Color.White;
            this.dtgMovimientos.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(76)))), ((int)(((byte)(129)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtgMovimientos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dtgMovimientos.ColumnHeadersHeight = 29;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtgMovimientos.DefaultCellStyle = dataGridViewCellStyle3;
            this.dtgMovimientos.EnableHeadersVisualStyles = false;
            this.dtgMovimientos.Location = new System.Drawing.Point(14, 279);
            this.dtgMovimientos.Name = "dtgMovimientos";
            this.dtgMovimientos.ReadOnly = true;
            this.dtgMovimientos.RowHeadersVisible = false;
            this.dtgMovimientos.RowHeadersWidth = 51;
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            this.dtgMovimientos.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dtgMovimientos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dtgMovimientos.Size = new System.Drawing.Size(1145, 230);
            this.dtgMovimientos.TabIndex = 38;
            // 
            // dtpFechaInicial
            // 
            this.dtpFechaInicial.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpFechaInicial.Location = new System.Drawing.Point(231, 111);
            this.dtpFechaInicial.Name = "dtpFechaInicial";
            this.dtpFechaInicial.Size = new System.Drawing.Size(302, 30);
            this.dtpFechaInicial.TabIndex = 39;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(227, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 20);
            this.label1.TabIndex = 40;
            this.label1.Text = "Fecha Inicial";
            // 
            // btnBuscar
            // 
            this.btnBuscar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnBuscar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBuscar.FlatAppearance.BorderSize = 0;
            this.btnBuscar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBuscar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnBuscar.ForeColor = System.Drawing.Color.White;
            this.btnBuscar.Location = new System.Drawing.Point(493, 167);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(100, 35);
            this.btnBuscar.TabIndex = 41;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = false;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(545, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 20);
            this.label2.TabIndex = 43;
            this.label2.Text = "Fecha Final";
            // 
            // dtpFechaFinal
            // 
            this.dtpFechaFinal.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpFechaFinal.Location = new System.Drawing.Point(549, 112);
            this.dtpFechaFinal.Name = "dtpFechaFinal";
            this.dtpFechaFinal.Size = new System.Drawing.Size(302, 30);
            this.dtpFechaFinal.TabIndex = 42;
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotal.ForeColor = System.Drawing.Color.White;
            this.lblTotal.Location = new System.Drawing.Point(12, 210);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(197, 23);
            this.lblTotal.TabIndex = 44;
            this.lblTotal.Text = "Valor Total Movimiento";
            // 
            // lblOperario
            // 
            this.lblOperario.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblOperario.ForeColor = System.Drawing.Color.White;
            this.lblOperario.Location = new System.Drawing.Point(857, 89);
            this.lblOperario.Name = "lblOperario";
            this.lblOperario.Size = new System.Drawing.Size(100, 23);
            this.lblOperario.TabIndex = 45;
            this.lblOperario.Text = "Operario";
            // 
            // cmbOperario
            // 
            this.cmbOperario.BackColor = System.Drawing.Color.White;
            this.cmbOperario.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOperario.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbOperario.Location = new System.Drawing.Point(861, 115);
            this.cmbOperario.MaxDropDownItems = 10;
            this.cmbOperario.Name = "cmbOperario";
            this.cmbOperario.Size = new System.Drawing.Size(298, 31);
            this.cmbOperario.TabIndex = 46;
            // 
            // FrmConsultaMovimientosCaja
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(90)))), ((int)(((byte)(150)))));
            this.ClientSize = new System.Drawing.Size(1171, 900);
            this.MinimumSize = new System.Drawing.Size(1187, 840);
            this.accionesAdmin.Name = "accionesAdmin";
            this.accionesAdmin.Location = new System.Drawing.Point(14, 535);
            this.accionesAdmin.Size = new System.Drawing.Size(1145, 345);
            this.accionesAdmin.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.accionesAdmin.ColumnCount = 1;
            this.accionesAdmin.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.accionesAdmin.RowCount = 6;
            this.accionesAdmin.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            this.accionesAdmin.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.accionesAdmin.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            this.accionesAdmin.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            this.accionesAdmin.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            this.accionesAdmin.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            var consultaAdmin = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false, Margin = Padding.Empty };
            this.cajaConsulta.Name = "cajaConsulta";
            this.cajaConsulta.Minimum = 1;
            this.cajaConsulta.Maximum = int.MaxValue;
            this.cajaConsulta.Width = 100;
            this.paginaConsulta.Name = "paginaConsulta";
            this.paginaConsulta.Minimum = 1;
            this.paginaConsulta.Maximum = 1000000;
            this.paginaConsulta.Width = 70;
            this.cargarCaja.Name = "cargarCaja";
            this.cargarCaja.Text = "Consultar jornada";
            this.cargarCaja.AutoSize = true;
            this.cargarCaja.Click += new System.EventHandler(this.cargarCaja_Click);
            this.alcance.Name = "alcance";
            this.alcance.DropDownStyle = ComboBoxStyle.DropDownList;
            this.alcance.Width = 190;
            this.alcance.Items.AddRange(new object[] { "Cobro seleccionado", "Turno del cobro" });
            this.verImpacto.Name = "verImpacto";
            this.verImpacto.Text = "Consultar impacto";
            this.verImpacto.AutoSize = true;
            this.verImpacto.Click += new System.EventHandler(this.verImpacto_Click);
            consultaAdmin.Controls.AddRange(new Control[] { new Label { Text = "Jornada", AutoSize = true }, this.cajaConsulta, new Label { Text = "Pagina", AutoSize = true }, this.paginaConsulta, this.cargarCaja, this.alcance, this.verImpacto });
            this.impactoDetalle.Name = "impactoDetalle";
            this.impactoDetalle.Multiline = true;
            this.impactoDetalle.ReadOnly = true;
            this.impactoDetalle.ScrollBars = ScrollBars.Vertical;
            this.impactoDetalle.Dock = DockStyle.Fill;
            var valoresAdmin = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false, Margin = Padding.Empty };
            this.importeCorreccion.Name = "importeCorreccion";
            this.importeCorreccion.DecimalPlaces = 2;
            this.importeCorreccion.Maximum = 999999999999m;
            this.importeCorreccion.Width = 180;
            this.medioCorreccion.Name = "medioCorreccion";
            this.medioCorreccion.DropDownStyle = ComboBoxStyle.DropDownList;
            this.medioCorreccion.Width = 230;
            valoresAdmin.Controls.AddRange(new Control[] { new Label { Text = "Nuevo importe", AutoSize = true }, this.importeCorreccion, new Label { Text = "Nuevo medio", AutoSize = true }, this.medioCorreccion });
            var razonAdmin = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false, Margin = Padding.Empty };
            this.motivoAdmin.Name = "motivoAdmin";
            this.motivoAdmin.MaxLength = 500;
            this.motivoAdmin.Width = 950;
            razonAdmin.Controls.AddRange(new Control[] { new Label { Text = "Motivo", AutoSize = true }, this.motivoAdmin });
            var botonesAdmin = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false, Margin = Padding.Empty };
            this.corregir.Name = "corregir";
            this.corregir.Text = "Confirmar correccion";
            this.corregir.AutoSize = true;
            this.corregir.Click += new System.EventHandler(this.corregir_Click);
            this.borrar.Name = "borrar";
            this.borrar.Text = "Borrar definitivamente";
            this.borrar.AutoSize = true;
            this.borrar.Click += new System.EventHandler(this.borrar_Click);
            this.cargarCierres.Name = "cargarCierres";
            this.cargarCierres.Text = "Cierres por rango";
            this.cargarCierres.AutoSize = true;
            this.cargarCierres.Click += new System.EventHandler(this.cargarCierres_Click);
            this.verCierre.Name = "verCierre";
            this.verCierre.Text = "Ver version";
            this.verCierre.AutoSize = true;
            this.verCierre.Click += new System.EventHandler(this.verCierre_Click);
            this.reabrir.Name = "reabrir";
            this.reabrir.Text = "Reabrir en revision";
            this.reabrir.AutoSize = true;
            this.reabrir.Click += new System.EventHandler(this.reabrir_Click);
            this.recerrar.Name = "recerrar";
            this.recerrar.Text = "Recierre historico";
            this.recerrar.AutoSize = true;
            this.recerrar.Click += new System.EventHandler(this.recerrar_Click);
            botonesAdmin.Controls.AddRange(new Control[] { this.corregir, this.borrar, this.cargarCierres, this.verCierre, this.reabrir, this.recerrar });
            this.estadoAdmin.Name = "estadoAdmin";
            this.estadoAdmin.Dock = DockStyle.Fill;
            this.estadoAdmin.AutoEllipsis = true;
            this.estadoAdmin.Text = "Sin impacto confirmado.";
            foreach (var button in new[] { this.cargarCaja, this.verImpacto, this.corregir, this.borrar, this.cargarCierres, this.verCierre, this.reabrir, this.recerrar })
            {
                button.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
                button.ForeColor = System.Drawing.Color.White;
                button.FlatStyle = FlatStyle.Flat;
                button.Height = 32;
                button.MinimumSize = new System.Drawing.Size(150, 32);
            }
            this.borrar.BackColor = System.Drawing.Color.Firebrick;
            this.accionesAdmin.Controls.Add(consultaAdmin, 0, 0);
            this.accionesAdmin.Controls.Add(this.impactoDetalle, 0, 1);
            this.accionesAdmin.Controls.Add(valoresAdmin, 0, 2);
            this.accionesAdmin.Controls.Add(razonAdmin, 0, 3);
            this.accionesAdmin.Controls.Add(botonesAdmin, 0, 4);
            this.accionesAdmin.Controls.Add(this.estadoAdmin, 0, 5);
            this.Controls.Add(this.accionesAdmin);
            this.Controls.Add(this.cmbOperario);
            this.Controls.Add(this.lblOperario);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dtpFechaFinal);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtpFechaInicial);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.lblTipoVehiculo);
            this.Controls.Add(this.cmbFormaPago);
            this.Controls.Add(this.lblHistorico);
            this.Controls.Add(this.dtgMovimientos);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.MinimizeBox = false;
            this.Name = "FrmConsultaMovimientosCaja";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Movimientos de caja";
            this.Load += new System.EventHandler(this.FrmConsultaMovimientosCaja_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtgMovimientos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private Label lblTitulo;
        private Label lblTipoVehiculo;
        private ComboBox cmbFormaPago;
        private Label lblHistorico;
        private DataGridView dtgMovimientos;
        private DateTimePicker dtpFechaInicial;
        private Label label1;
        private Button btnBuscar;
        private Label label2;
        private DateTimePicker dtpFechaFinal;
        private Label lblTotal;
        private Label lblOperario;
        private ComboBox cmbOperario;
        private TableLayoutPanel accionesAdmin;
        private NumericUpDown cajaConsulta;
        private NumericUpDown paginaConsulta;
        private Button cargarCaja;
        private ComboBox alcance;
        private Button verImpacto;
        private TextBox impactoDetalle;
        private NumericUpDown importeCorreccion;
        private ComboBox medioCorreccion;
        private TextBox motivoAdmin;
        private Button corregir;
        private Button borrar;
        private Label estadoAdmin;
        private Button cargarCierres;
        private Button verCierre;
        private Button reabrir;
        private Button recerrar;
    }
}