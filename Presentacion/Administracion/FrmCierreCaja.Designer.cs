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
        private Button btnCerrarVentana;

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
            this.btnCerrarVentana = new System.Windows.Forms.Button();
            this.cardResumen = new System.Windows.Forms.Panel();
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
            this.SuspendLayout();

            // FORM
            this.BackColor = System.Drawing.Color.FromArgb(240, 245, 250);
            this.ClientSize = new System.Drawing.Size(550, 430);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;

            // HEADER
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(15, 76, 129);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Height = 55;

            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(20, 15);
            this.lblTitulo.Text = "Cierre de Caja";

            this.btnCerrarVentana.BackColor = System.Drawing.Color.FromArgb(200, 0, 0);
            this.btnCerrarVentana.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrarVentana.FlatAppearance.BorderSize = 0;
            this.btnCerrarVentana.ForeColor = System.Drawing.Color.White;
            this.btnCerrarVentana.Size = new System.Drawing.Size(35, 28);
            this.btnCerrarVentana.Location = new System.Drawing.Point(500, 13);
            this.btnCerrarVentana.Text = "X";

            this.panelHeader.Controls.Add(this.lblTitulo);
            this.panelHeader.Controls.Add(this.btnCerrarVentana);

            // CARD
            this.cardResumen.BackColor = System.Drawing.Color.White;
            this.cardResumen.Size = new System.Drawing.Size(480, 280);
            this.cardResumen.Location = new System.Drawing.Point(35, 90);

            // Labels Texto
            this.lblMontoInicial.AutoSize = true;
            this.lblMontoInicial.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblMontoInicial.Location = new System.Drawing.Point(30, 30);
            this.lblMontoInicial.Text = "Monto Inicial:";

            this.lblIngresos.AutoSize = true;
            this.lblIngresos.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblIngresos.Location = new System.Drawing.Point(30, 70);
            this.lblIngresos.Text = "Total Ingresos:";

            this.lblGastos.AutoSize = true;
            this.lblGastos.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblGastos.Location = new System.Drawing.Point(30, 110);
            this.lblGastos.Text = "Total Gastos:";

            this.lblTotalCaja.AutoSize = true;
            this.lblTotalCaja.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotalCaja.Location = new System.Drawing.Point(30, 150);
            this.lblTotalCaja.Text = "Total en Caja:";

            this.lblDiferencia.AutoSize = true;
            this.lblDiferencia.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDiferencia.Location = new System.Drawing.Point(30, 190);
            this.lblDiferencia.Text = "Diferencia:";

            // Labels Valores
            this.lblMontoInicialValor.AutoSize = true;
            this.lblMontoInicialValor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblMontoInicialValor.Location = new System.Drawing.Point(300, 30);
            this.lblMontoInicialValor.Text = "0.00";

            this.lblIngresosValor.AutoSize = true;
            this.lblIngresosValor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblIngresosValor.Location = new System.Drawing.Point(300, 70);
            this.lblIngresosValor.Text = "0.00";

            this.lblGastosValor.AutoSize = true;
            this.lblGastosValor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblGastosValor.Location = new System.Drawing.Point(300, 110);
            this.lblGastosValor.Text = "0.00";

            this.lblTotalCajaValor.AutoSize = true;
            this.lblTotalCajaValor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTotalCajaValor.Location = new System.Drawing.Point(300, 150);
            this.lblTotalCajaValor.Text = "0.00";

            this.lblDiferenciaValor.AutoSize = true;
            this.lblDiferenciaValor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDiferenciaValor.ForeColor = System.Drawing.Color.FromArgb(40, 167, 69);
            this.lblDiferenciaValor.Location = new System.Drawing.Point(300, 190);
            this.lblDiferenciaValor.Text = "0.00";

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

            // BOTON CERRAR
            this.btnCerrarCaja.BackColor = System.Drawing.Color.FromArgb(220, 53, 69);
            this.btnCerrarCaja.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrarCaja.FlatAppearance.BorderSize = 0;
            this.btnCerrarCaja.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCerrarCaja.ForeColor = System.Drawing.Color.White;
            this.btnCerrarCaja.Size = new System.Drawing.Size(180, 45);
            this.btnCerrarCaja.Location = new System.Drawing.Point(335, 380);
            this.btnCerrarCaja.Text = "Cerrar Caja";

            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.cardResumen);
            this.Controls.Add(this.btnCerrarCaja);

            this.ResumeLayout(false);
        }

        #endregion
    }

}