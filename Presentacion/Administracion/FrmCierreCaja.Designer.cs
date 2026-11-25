using System;
using System.Drawing;
using System.Windows.Forms;

namespace CarWash.Presentacion.Administracion
{
    partial class FrmCierreCaja
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private Panel panelHeader;
        private Label lblTitulo;

        private Panel cardResumen;

        private Label lblMontoInicial;
        private Label lblIngresos;
        private Label lblGastos;
        private Label lblTotalCaja;
        private Label lblDiferencia;

        private Label lblMontoInicialValor;
        private Label lblIngresosValor;
        private Label lblGastosValor;
        private Label lblTotalCajaValor;
        private Label lblDiferenciaValor;

        private Button btnCerrarCaja;
        private NumericUpDown contado;
        private TextBox motivo;
        private DataGridView detalle;
        private Label estado;
        private Label lblContado;
        private Label lblMotivo;
        private Button btnCerrar;


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
            this.contado = new NumericUpDown();
            this.motivo = new TextBox();
            this.detalle = new DataGridView();
            this.estado = new Label();
            this.lblContado = new Label();
            this.lblMotivo = new Label();
            this.btnCerrar = new Button();
            ((System.ComponentModel.ISupportInitialize)(this.contado)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.detalle)).BeginInit();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.cardResumen = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.lblIngresosTransferencia = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblIngresosEfectivo = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblIngresosDatafono = new System.Windows.Forms.Label();
            this.lblMontoInicial = new System.Windows.Forms.Label();
            this.lblIngresos = new System.Windows.Forms.Label();
            this.lblGastos = new System.Windows.Forms.Label();
            this.lblTotalCaja = new System.Windows.Forms.Label();
            this.lblDiferencia = new System.Windows.Forms.Label();
            this.lblMontoInicialValor = new System.Windows.Forms.Label();
            this.lblIngresosValor = new System.Windows.Forms.Label();
            this.lblGastosValor = new System.Windows.Forms.Label();
            this.lblTotalCajaValor = new System.Windows.Forms.Label();
            this.lblDiferenciaValor = new System.Windows.Forms.Label();
            this.btnCerrarCaja = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.lblTotalValesOperarios = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblVentasCredito = new System.Windows.Forms.Label();
            this.btnRecalcular = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelHeader.SuspendLayout();
            this.cardResumen.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(76)))), ((int)(((byte)(129)))));
            this.panelHeader.Controls.Add(this.lblTitulo);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1109, 69);
            this.panelHeader.TabIndex = 0;
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(20, 15);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(157, 30);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Cierre de Caja";
            // 
            // cardResumen
            // 
            this.cardResumen.BackColor = System.Drawing.Color.White;
            this.cardResumen.Controls.Add(this.label8);
            this.cardResumen.Controls.Add(this.lblVentasCredito);
            this.cardResumen.Controls.Add(this.label7);
            this.cardResumen.Controls.Add(this.lblTotalValesOperarios);
            this.cardResumen.Controls.Add(this.label5);
            this.cardResumen.Controls.Add(this.lblIngresosTransferencia);
            this.cardResumen.Controls.Add(this.label3);
            this.cardResumen.Controls.Add(this.lblIngresosEfectivo);
            this.cardResumen.Controls.Add(this.label1);
            this.cardResumen.Controls.Add(this.lblIngresosDatafono);
            this.cardResumen.Controls.Add(this.lblMontoInicial);
            this.cardResumen.Controls.Add(this.lblIngresos);
            this.cardResumen.Controls.Add(this.lblGastos);
            this.cardResumen.Controls.Add(this.lblTotalCaja);
            this.cardResumen.Controls.Add(this.lblDiferencia);
            this.cardResumen.Controls.Add(this.lblMontoInicialValor);
            this.cardResumen.Controls.Add(this.lblIngresosValor);
            this.cardResumen.Controls.Add(this.lblGastosValor);
            this.cardResumen.Controls.Add(this.lblTotalCajaValor);
            this.cardResumen.Controls.Add(this.lblDiferenciaValor);
            this.cardResumen.Location = new System.Drawing.Point(14, 90);
            this.cardResumen.Name = "cardResumen";
            this.cardResumen.Size = new System.Drawing.Size(535, 392);
            this.cardResumen.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(30, 98);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(236, 23);
            this.label5.TabIndex = 14;
            this.label5.Text = "Total Ingresos Transferencia:";
            // 
            // lblIngresosTransferencia
            // 
            this.lblIngresosTransferencia.AutoSize = true;
            this.lblIngresosTransferencia.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblIngresosTransferencia.Location = new System.Drawing.Point(324, 98);
            this.lblIngresosTransferencia.Name = "lblIngresosTransferencia";
            this.lblIngresosTransferencia.Size = new System.Drawing.Size(41, 23);
            this.lblIngresosTransferencia.TabIndex = 15;
            this.lblIngresosTransferencia.Text = "0.00";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(30, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(194, 23);
            this.label3.TabIndex = 12;
            this.label3.Text = "Total Ingresos Efectivo:";
            // 
            // lblIngresosEfectivo
            // 
            this.lblIngresosEfectivo.AutoSize = true;
            this.lblIngresosEfectivo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblIngresosEfectivo.Location = new System.Drawing.Point(324, 63);
            this.lblIngresosEfectivo.Name = "lblIngresosEfectivo";
            this.lblIngresosEfectivo.Size = new System.Drawing.Size(41, 23);
            this.lblIngresosEfectivo.TabIndex = 13;
            this.lblIngresosEfectivo.Text = "0.00";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(30, 129);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(205, 23);
            this.label1.TabIndex = 10;
            this.label1.Text = "Total Ingresos Datafono:";
            // 
            // lblIngresosDatafono
            // 
            this.lblIngresosDatafono.AutoSize = true;
            this.lblIngresosDatafono.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblIngresosDatafono.Location = new System.Drawing.Point(324, 129);
            this.lblIngresosDatafono.Name = "lblIngresosDatafono";
            this.lblIngresosDatafono.Size = new System.Drawing.Size(41, 23);
            this.lblIngresosDatafono.TabIndex = 11;
            this.lblIngresosDatafono.Text = "0.00";
            // 
            // lblMontoInicial
            // 
            this.lblMontoInicial.AutoSize = true;
            this.lblMontoInicial.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblMontoInicial.Location = new System.Drawing.Point(30, 30);
            this.lblMontoInicial.Name = "lblMontoInicial";
            this.lblMontoInicial.Size = new System.Drawing.Size(120, 23);
            this.lblMontoInicial.TabIndex = 0;
            this.lblMontoInicial.Text = "Monto Inicial:";
            // 
            // lblIngresos
            // 
            this.lblIngresos.AutoSize = true;
            this.lblIngresos.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblIngresos.Location = new System.Drawing.Point(30, 189);
            this.lblIngresos.Name = "lblIngresos";
            this.lblIngresos.Size = new System.Drawing.Size(125, 23);
            this.lblIngresos.TabIndex = 1;
            this.lblIngresos.Text = "Total Ingresos:";
            // 
            // lblGastos
            // 
            this.lblGastos.AutoSize = true;
            this.lblGastos.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblGastos.Location = new System.Drawing.Point(30, 225);
            this.lblGastos.Name = "lblGastos";
            this.lblGastos.Size = new System.Drawing.Size(111, 23);
            this.lblGastos.TabIndex = 2;
            this.lblGastos.Text = "Total Gastos:";
            // 
            // lblTotalCaja
            // 
            this.lblTotalCaja.AutoSize = true;
            this.lblTotalCaja.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotalCaja.Location = new System.Drawing.Point(30, 286);
            this.lblTotalCaja.Name = "lblTotalCaja";
            this.lblTotalCaja.Size = new System.Drawing.Size(117, 23);
            this.lblTotalCaja.TabIndex = 3;
            this.lblTotalCaja.Text = "Total en Caja:";
            // 
            // lblDiferencia
            // 
            this.lblDiferencia.AutoSize = true;
            this.lblDiferencia.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDiferencia.Location = new System.Drawing.Point(30, 325);
            this.lblDiferencia.Name = "lblDiferencia";
            this.lblDiferencia.Size = new System.Drawing.Size(97, 23);
            this.lblDiferencia.TabIndex = 4;
            this.lblDiferencia.Text = "Diferencia:";
            // 
            // lblMontoInicialValor
            // 
            this.lblMontoInicialValor.AutoSize = true;
            this.lblMontoInicialValor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblMontoInicialValor.Location = new System.Drawing.Point(324, 30);
            this.lblMontoInicialValor.Name = "lblMontoInicialValor";
            this.lblMontoInicialValor.Size = new System.Drawing.Size(41, 23);
            this.lblMontoInicialValor.TabIndex = 5;
            this.lblMontoInicialValor.Text = "0.00";
            // 
            // lblIngresosValor
            // 
            this.lblIngresosValor.AutoSize = true;
            this.lblIngresosValor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblIngresosValor.Location = new System.Drawing.Point(324, 189);
            this.lblIngresosValor.Name = "lblIngresosValor";
            this.lblIngresosValor.Size = new System.Drawing.Size(41, 23);
            this.lblIngresosValor.TabIndex = 6;
            this.lblIngresosValor.Text = "0.00";
            // 
            // lblGastosValor
            // 
            this.lblGastosValor.AutoSize = true;
            this.lblGastosValor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblGastosValor.Location = new System.Drawing.Point(324, 225);
            this.lblGastosValor.Name = "lblGastosValor";
            this.lblGastosValor.Size = new System.Drawing.Size(41, 23);
            this.lblGastosValor.TabIndex = 7;
            this.lblGastosValor.Text = "0.00";
            // 
            // lblTotalCajaValor
            // 
            this.lblTotalCajaValor.AutoSize = true;
            this.lblTotalCajaValor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTotalCajaValor.Location = new System.Drawing.Point(324, 286);
            this.lblTotalCajaValor.Name = "lblTotalCajaValor";
            this.lblTotalCajaValor.Size = new System.Drawing.Size(41, 23);
            this.lblTotalCajaValor.TabIndex = 8;
            this.lblTotalCajaValor.Text = "0.00";
            // 
            // lblDiferenciaValor
            // 
            this.lblDiferenciaValor.AutoSize = true;
            this.lblDiferenciaValor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDiferenciaValor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.lblDiferenciaValor.Location = new System.Drawing.Point(324, 325);
            this.lblDiferenciaValor.Name = "lblDiferenciaValor";
            this.lblDiferenciaValor.Size = new System.Drawing.Size(41, 23);
            this.lblDiferenciaValor.TabIndex = 9;
            this.lblDiferenciaValor.Text = "0.00";
            // 
            // btnCerrarCaja
            // 
            this.btnCerrarCaja.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnCerrarCaja.FlatAppearance.BorderSize = 0;
            this.btnCerrarCaja.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrarCaja.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCerrarCaja.ForeColor = System.Drawing.Color.White;
            this.btnCerrarCaja.Location = new System.Drawing.Point(460, 531);
            this.btnCerrarCaja.Name = "btnCerrarCaja";
            this.btnCerrarCaja.Size = new System.Drawing.Size(180, 45);
            this.btnCerrarCaja.TabIndex = 2;
            this.btnCerrarCaja.Text = "Cerrar Caja";
            this.btnCerrarCaja.UseVisualStyleBackColor = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label7.Location = new System.Drawing.Point(30, 256);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(182, 23);
            this.label7.TabIndex = 16;
            this.label7.Text = "Total Vales Operarios:";
            // 
            // lblTotalValesOperarios
            // 
            this.lblTotalValesOperarios.AutoSize = true;
            this.lblTotalValesOperarios.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTotalValesOperarios.Location = new System.Drawing.Point(324, 255);
            this.lblTotalValesOperarios.Name = "lblTotalValesOperarios";
            this.lblTotalValesOperarios.Size = new System.Drawing.Size(41, 23);
            this.lblTotalValesOperarios.TabIndex = 17;
            this.lblTotalValesOperarios.Text = "0.00";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label8.Location = new System.Drawing.Point(30, 159);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(190, 23);
            this.label8.TabIndex = 18;
            this.label8.Text = "Total Ingresos Crédito:";
            // 
            // lblVentasCredito
            // 
            this.lblVentasCredito.AutoSize = true;
            this.lblVentasCredito.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblVentasCredito.Location = new System.Drawing.Point(324, 159);
            this.lblVentasCredito.Name = "lblVentasCredito";
            this.lblVentasCredito.Size = new System.Drawing.Size(41, 23);
            this.lblVentasCredito.TabIndex = 19;
            this.lblVentasCredito.Text = "0.00";
            // 
            // btnRecalcular
            // 
            this.btnRecalcular.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnRecalcular.FlatAppearance.BorderSize = 0;
            this.btnRecalcular.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRecalcular.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnRecalcular.ForeColor = System.Drawing.Color.White;
            this.btnRecalcular.Location = new System.Drawing.Point(338, 325);
            this.btnRecalcular.Name = "btnRecalcular";
            this.btnRecalcular.Size = new System.Drawing.Size(180, 45);
            this.btnRecalcular.TabIndex = 4;
            this.btnRecalcular.Text = "Actualizar";
            this.btnRecalcular.UseVisualStyleBackColor = false;
            this.btnRecalcular.Click += new System.EventHandler(this.btnRecalcular_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.contado);
            this.panel1.Controls.Add(this.motivo);
            this.panel1.Controls.Add(this.detalle);
            this.panel1.Controls.Add(this.lblContado);
            this.panel1.Controls.Add(this.lblMotivo);
            this.panel1.Controls.Add(this.btnRecalcular);
            this.panel1.Location = new System.Drawing.Point(556, 90);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(535, 392);
            this.panel1.TabIndex = 3;
            // 
            // FrmCierreCaja
            // 
            this.contado.Name = "contado";
            this.contado.Location = new Point(20, 42);
            this.contado.Size = new Size(495, 30);
            this.contado.Font = new Font("Segoe UI", 11F);
            this.contado.Maximum = decimal.MaxValue;
            this.contado.DecimalPlaces = 2;
            this.contado.ThousandsSeparator = true;
            this.contado.TabIndex = 0;
            this.contado.AccessibleName = "Efectivo contado";
            this.lblContado.Text = "Efectivo contado";
            this.lblContado.Location = new Point(20, 15);
            this.lblContado.Size = new Size(300, 24);
            this.motivo.Name = "motivo";
            this.motivo.Location = new Point(20, 112);
            this.motivo.Size = new Size(495, 60);
            this.motivo.Multiline = true;
            this.motivo.MaxLength = 500;
            this.motivo.TabIndex = 1;
            this.motivo.AccessibleName = "Justificacion de diferencia";
            this.lblMotivo.Text = "Justificacion de diferencia";
            this.lblMotivo.Location = new Point(20, 84);
            this.lblMotivo.Size = new Size(400, 24);
            this.detalle.Name = "detalle";
            this.detalle.Location = new Point(20, 185);
            this.detalle.Size = new Size(495, 130);
            this.detalle.ReadOnly = true;
            this.detalle.AllowUserToAddRows = false;
            this.detalle.AllowUserToDeleteRows = false;
            this.detalle.RowHeadersVisible = false;
            this.detalle.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.detalle.Columns.Add("tipo", "Tipo");
            this.detalle.Columns.Add("medio", "Medio / ID");
            this.detalle.Columns.Add("importe", "Importe / diagnostico");
            this.detalle.TabIndex = 2;
            this.estado.Name = "estado";
            this.estado.Location = new Point(14, 490);
            this.estado.Size = new Size(1080, 40);
            this.estado.Text = "Consultando jornada...";
            this.estado.AutoEllipsis = true;
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.Location = new Point(885, 531);
            this.btnCerrar.Size = new Size(180, 45);
            this.btnCerrar.FlatStyle = FlatStyle.Flat;
            this.btnCerrar.BackColor = Color.FromArgb(20, 90, 150);
            this.btnCerrar.ForeColor = Color.White;
            this.btnCerrar.TabIndex = 5;
            this.btnCerrar.Click += new EventHandler(this.btnCerrarVentana_Click);
            this.btnCerrarCaja.Size = new Size(250, 45);
            this.btnCerrarCaja.TabIndex = 4;
            this.btnRecalcular.TabIndex = 3;
            this.CancelButton = this.btnCerrar;
            this.MinimumSize = new Size(1125, 627);
            this.AutoScaleMode = AutoScaleMode.None;
            this.Text = "Cierre de caja";
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.lblTotalCaja.Text = "Efectivo esperado:";
            this.label8.Text = "Credito pendiente:";
            this.Controls.Add(this.estado);
            this.Controls.Add(this.btnCerrar);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1109, 588);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.cardResumen);
            this.Controls.Add(this.btnCerrarCaja);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FrmCierreCaja";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.FrmCierreCaja_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.cardResumen.ResumeLayout(false);
            this.cardResumen.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.contado)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.detalle)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Label label5;
        private Label lblIngresosTransferencia;
        private Label label3;
        private Label lblIngresosEfectivo;
        private Label label1;
        private Label lblIngresosDatafono;
        private Label label7;
        private Label lblTotalValesOperarios;
        private Label label8;
        private Label lblVentasCredito;
        private Button btnRecalcular;
        private Panel panel1;
    }

}