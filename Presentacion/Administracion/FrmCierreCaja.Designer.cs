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
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtValorEfectivo = new System.Windows.Forms.TextBox();
            this.txtValorTransferencia = new System.Windows.Forms.TextBox();
            this.txtValorDatafono = new System.Windows.Forms.TextBox();
            this.btnRecalcular = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtValorCredito = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtValorGastos = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtValorVales = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(28, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(188, 23);
            this.label2.TabIndex = 13;
            this.label2.Text = "Ingrese Valor Efectivo:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(34, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(230, 23);
            this.label4.TabIndex = 14;
            this.label4.Text = "Ingrese Valor Transferencia:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(34, 120);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(199, 23);
            this.label6.TabIndex = 15;
            this.label6.Text = "Ingrese Valor Datafono:";
            // 
            // txtValorEfectivo
            // 
            this.txtValorEfectivo.Location = new System.Drawing.Point(290, 45);
            this.txtValorEfectivo.MaxLength = 20;
            this.txtValorEfectivo.Name = "txtValorEfectivo";
            this.txtValorEfectivo.Size = new System.Drawing.Size(159, 22);
            this.txtValorEfectivo.TabIndex = 16;
            this.txtValorEfectivo.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtValorEfectivo_MouseClick);
            this.txtValorEfectivo.Enter += new System.EventHandler(this.TextBox_SelectAll);
            this.txtValorEfectivo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtValorEfectivo_KeyDown);
            this.txtValorEfectivo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtValorEfectivo_KeyPress);
            // 
            // txtValorTransferencia
            // 
            this.txtValorTransferencia.Location = new System.Drawing.Point(290, 83);
            this.txtValorTransferencia.MaxLength = 20;
            this.txtValorTransferencia.Name = "txtValorTransferencia";
            this.txtValorTransferencia.Size = new System.Drawing.Size(159, 22);
            this.txtValorTransferencia.TabIndex = 17;
            this.txtValorTransferencia.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtValorTransferencia_MouseClick);
            this.txtValorTransferencia.Enter += new System.EventHandler(this.TextBox_SelectAll);
            this.txtValorTransferencia.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtValorTransferencia_KeyDown);
            this.txtValorTransferencia.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtValorTransferencia_KeyPress);
            // 
            // txtValorDatafono
            // 
            this.txtValorDatafono.Location = new System.Drawing.Point(291, 120);
            this.txtValorDatafono.MaxLength = 20;
            this.txtValorDatafono.Name = "txtValorDatafono";
            this.txtValorDatafono.Size = new System.Drawing.Size(159, 22);
            this.txtValorDatafono.TabIndex = 18;
            this.txtValorDatafono.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtValorDatafono_MouseClick);
            this.txtValorDatafono.Enter += new System.EventHandler(this.TextBox_SelectAll);
            this.txtValorDatafono.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtValorDatafono_KeyDown);
            this.txtValorDatafono.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtValorDatafono_KeyPress);
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
            this.btnRecalcular.Text = "Cuadrar Caja";
            this.btnRecalcular.UseVisualStyleBackColor = false;
            this.btnRecalcular.Click += new System.EventHandler(this.btnRecalcular_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.txtValorVales);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.txtValorGastos);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.txtValorCredito);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.btnRecalcular);
            this.panel1.Controls.Add(this.txtValorDatafono);
            this.panel1.Controls.Add(this.txtValorTransferencia);
            this.panel1.Controls.Add(this.txtValorEfectivo);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(556, 90);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(535, 392);
            this.panel1.TabIndex = 3;
            // 
            // txtValorCredito
            // 
            this.txtValorCredito.Location = new System.Drawing.Point(291, 159);
            this.txtValorCredito.MaxLength = 20;
            this.txtValorCredito.Name = "txtValorCredito";
            this.txtValorCredito.Size = new System.Drawing.Size(159, 22);
            this.txtValorCredito.TabIndex = 20;
            this.txtValorCredito.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtValorCredito_MouseClick);
            this.txtValorCredito.Enter += new System.EventHandler(this.TextBox_SelectAll);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label9.Location = new System.Drawing.Point(34, 159);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(184, 23);
            this.label9.TabIndex = 19;
            this.label9.Text = "Ingrese Valor Crédito:";
            // 
            // txtValorGastos
            // 
            this.txtValorGastos.Location = new System.Drawing.Point(290, 191);
            this.txtValorGastos.MaxLength = 20;
            this.txtValorGastos.Name = "txtValorGastos";
            this.txtValorGastos.Size = new System.Drawing.Size(159, 22);
            this.txtValorGastos.TabIndex = 22;
            this.txtValorGastos.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtValorGastos_MouseClick);
            this.txtValorGastos.Enter += new System.EventHandler(this.TextBox_SelectAll);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label10.Location = new System.Drawing.Point(33, 191);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(176, 23);
            this.label10.TabIndex = 21;
            this.label10.Text = "Ingrese Valor Gastos:";
            // 
            // txtValorVales
            // 
            this.txtValorVales.Location = new System.Drawing.Point(291, 225);
            this.txtValorVales.MaxLength = 20;
            this.txtValorVales.Name = "txtValorVales";
            this.txtValorVales.Size = new System.Drawing.Size(159, 22);
            this.txtValorVales.TabIndex = 24;
            this.txtValorVales.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtValorVales_MouseClick);
            this.txtValorVales.Enter += new System.EventHandler(this.TextBox_SelectAll);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label11.Location = new System.Drawing.Point(34, 225);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(164, 23);
            this.label11.TabIndex = 23;
            this.label11.Text = "Ingrese Valor Vales:";
            // 
            // FrmCierreCaja
            // 
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
        private Label label2;
        private Label label4;
        private Label label6;
        private TextBox txtValorEfectivo;
        private TextBox txtValorTransferencia;
        private TextBox txtValorDatafono;
        private Button btnRecalcular;
        private Panel panel1;
        private TextBox txtValorVales;
        private Label label11;
        private TextBox txtValorGastos;
        private Label label10;
        private TextBox txtValorCredito;
        private Label label9;
    }

}