using System.Drawing;
using System.Windows.Forms;

namespace CarWash.Presentacion.Operacion
{
    partial class FrmGestionDetalleVehiculo
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


        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Button btnCerrar;

        private System.Windows.Forms.Panel cardInfo;
        private System.Windows.Forms.Label lblPlaca;
        private System.Windows.Forms.Label lblCliente;
        private System.Windows.Forms.Label lblEstado;

        private System.Windows.Forms.Button btnFinalizar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.GroupBox gbFormaPago;
        private System.Windows.Forms.RadioButton rbEfectivo;
        private System.Windows.Forms.RadioButton rbTransferencia;
        private System.Windows.Forms.RadioButton rbDatafono;
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.cardInfo = new System.Windows.Forms.Panel();
            this.lblObservaciones = new System.Windows.Forms.Label();
            this.txtObservaciones = new System.Windows.Forms.TextBox();
            this.lblOperadorAsignado = new System.Windows.Forms.Label();
            this.txtValorPagar = new System.Windows.Forms.TextBox();
            this.lblValorPagar = new System.Windows.Forms.Label();
            this.lblServicio = new System.Windows.Forms.Label();
            this.lblTipoVehiculo = new System.Windows.Forms.Label();
            this.lblFechaIngreso = new System.Windows.Forms.Label();
            this.lblTurno = new System.Windows.Forms.Label();
            this.lblCelular = new System.Windows.Forms.Label();
            this.lblPlaca = new System.Windows.Forms.Label();
            this.lblCliente = new System.Windows.Forms.Label();
            this.lblEstado = new System.Windows.Forms.Label();
            this.gbFormaPago = new System.Windows.Forms.GroupBox();
            this.rbEfectivo = new System.Windows.Forms.RadioButton();
            this.rbTransferencia = new System.Windows.Forms.RadioButton();
            this.rbDatafono = new System.Windows.Forms.RadioButton();
            this.btnFinalizar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnAsignarOperador = new System.Windows.Forms.Button();
            this.btnLiberarOperador = new System.Windows.Forms.Button();
            this.panelHeader.SuspendLayout();
            this.cardInfo.SuspendLayout();
            this.gbFormaPago.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(76)))), ((int)(((byte)(129)))));
            this.panelHeader.Controls.Add(this.lblTitulo);
            this.panelHeader.Controls.Add(this.btnCerrar);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(823, 60);
            this.panelHeader.TabIndex = 0;
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(20, 15);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(334, 32);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Gestión Detalle del Vehículo";
            // 
            // btnCerrar
            // 
            this.btnCerrar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnCerrar.FlatAppearance.BorderSize = 0;
            this.btnCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrar.ForeColor = System.Drawing.Color.White;
            this.btnCerrar.Location = new System.Drawing.Point(780, 3);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(40, 30);
            this.btnCerrar.TabIndex = 1;
            this.btnCerrar.Text = "X";
            this.btnCerrar.UseVisualStyleBackColor = false;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // cardInfo
            // 
            this.cardInfo.BackColor = System.Drawing.Color.White;
            this.cardInfo.Controls.Add(this.lblObservaciones);
            this.cardInfo.Controls.Add(this.txtObservaciones);
            this.cardInfo.Controls.Add(this.lblOperadorAsignado);
            this.cardInfo.Controls.Add(this.txtValorPagar);
            this.cardInfo.Controls.Add(this.lblValorPagar);
            this.cardInfo.Controls.Add(this.lblServicio);
            this.cardInfo.Controls.Add(this.lblTipoVehiculo);
            this.cardInfo.Controls.Add(this.lblFechaIngreso);
            this.cardInfo.Controls.Add(this.lblTurno);
            this.cardInfo.Controls.Add(this.lblCelular);
            this.cardInfo.Controls.Add(this.lblPlaca);
            this.cardInfo.Controls.Add(this.lblCliente);
            this.cardInfo.Controls.Add(this.lblEstado);
            this.cardInfo.Controls.Add(this.gbFormaPago);
            this.cardInfo.Location = new System.Drawing.Point(30, 90);
            this.cardInfo.Name = "cardInfo";
            this.cardInfo.Size = new System.Drawing.Size(760, 517);
            this.cardInfo.TabIndex = 1;
            // 
            // lblObservaciones
            // 
            this.lblObservaciones.AutoSize = true;
            this.lblObservaciones.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblObservaciones.Location = new System.Drawing.Point(24, 373);
            this.lblObservaciones.Name = "lblObservaciones";
            this.lblObservaciones.Size = new System.Drawing.Size(120, 23);
            this.lblObservaciones.TabIndex = 13;
            this.lblObservaciones.Text = "Observaciones";
            // 
            // txtObservaciones
            // 
            this.txtObservaciones.Location = new System.Drawing.Point(26, 399);
            this.txtObservaciones.Multiline = true;
            this.txtObservaciones.Name = "txtObservaciones";
            this.txtObservaciones.Size = new System.Drawing.Size(714, 99);
            this.txtObservaciones.TabIndex = 6;
            // 
            // lblOperadorAsignado
            // 
            this.lblOperadorAsignado.AutoSize = true;
            this.lblOperadorAsignado.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblOperadorAsignado.Location = new System.Drawing.Point(24, 221);
            this.lblOperadorAsignado.Name = "lblOperadorAsignado";
            this.lblOperadorAsignado.Size = new System.Drawing.Size(163, 23);
            this.lblOperadorAsignado.TabIndex = 10;
            this.lblOperadorAsignado.Text = "Operador Asignado:";
            // 
            // txtValorPagar
            // 
            this.txtValorPagar.Location = new System.Drawing.Point(145, 324);
            this.txtValorPagar.Name = "txtValorPagar";
            this.txtValorPagar.Size = new System.Drawing.Size(158, 22);
            this.txtValorPagar.TabIndex = 5;
            this.txtValorPagar.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtValorPagar_KeyDown);
            this.txtValorPagar.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtValorPagar_KeyPress);
            this.txtValorPagar.Leave += new System.EventHandler(this.txtValorPagar_Leave);
            // 
            // lblValorPagar
            // 
            this.lblValorPagar.AutoSize = true;
            this.lblValorPagar.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblValorPagar.Location = new System.Drawing.Point(24, 323);
            this.lblValorPagar.Name = "lblValorPagar";
            this.lblValorPagar.Size = new System.Drawing.Size(115, 23);
            this.lblValorPagar.TabIndex = 8;
            this.lblValorPagar.Text = "Valor a Pagar:";
            // 
            // lblServicio
            // 
            this.lblServicio.AutoSize = true;
            this.lblServicio.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblServicio.Location = new System.Drawing.Point(24, 192);
            this.lblServicio.Name = "lblServicio";
            this.lblServicio.Size = new System.Drawing.Size(223, 23);
            this.lblServicio.TabIndex = 7;
            this.lblServicio.Text = "Servicio Atendido: Enjuague";
            // 
            // lblTipoVehiculo
            // 
            this.lblTipoVehiculo.AutoSize = true;
            this.lblTipoVehiculo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTipoVehiculo.Location = new System.Drawing.Point(23, 163);
            this.lblTipoVehiculo.Name = "lblTipoVehiculo";
            this.lblTipoVehiculo.Size = new System.Drawing.Size(181, 23);
            this.lblTipoVehiculo.TabIndex = 6;
            this.lblTipoVehiculo.Text = "Tipo Vehiculo: Camión";
            // 
            // lblFechaIngreso
            // 
            this.lblFechaIngreso.AutoSize = true;
            this.lblFechaIngreso.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblFechaIngreso.Location = new System.Drawing.Point(22, 134);
            this.lblFechaIngreso.Name = "lblFechaIngreso";
            this.lblFechaIngreso.Size = new System.Drawing.Size(257, 23);
            this.lblFechaIngreso.TabIndex = 5;
            this.lblFechaIngreso.Text = "Fecha Hora Ingreso: 3010000000";
            // 
            // lblTurno
            // 
            this.lblTurno.AutoSize = true;
            this.lblTurno.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTurno.Location = new System.Drawing.Point(22, 20);
            this.lblTurno.Name = "lblTurno";
            this.lblTurno.Size = new System.Drawing.Size(210, 25);
            this.lblTurno.TabIndex = 4;
            this.lblTurno.Text = "Turno N°: T-20260216";
            // 
            // lblCelular
            // 
            this.lblCelular.AutoSize = true;
            this.lblCelular.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblCelular.Location = new System.Drawing.Point(22, 106);
            this.lblCelular.Name = "lblCelular";
            this.lblCelular.Size = new System.Drawing.Size(230, 23);
            this.lblCelular.TabIndex = 3;
            this.lblCelular.Text = "Número Celular: 3010000000";
            // 
            // lblPlaca
            // 
            this.lblPlaca.AutoSize = true;
            this.lblPlaca.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblPlaca.Location = new System.Drawing.Point(22, 48);
            this.lblPlaca.Name = "lblPlaca";
            this.lblPlaca.Size = new System.Drawing.Size(146, 25);
            this.lblPlaca.TabIndex = 0;
            this.lblPlaca.Text = "Placa: ABC-123";
            // 
            // lblCliente
            // 
            this.lblCliente.AutoSize = true;
            this.lblCliente.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblCliente.Location = new System.Drawing.Point(22, 77);
            this.lblCliente.Name = "lblCliente";
            this.lblCliente.Size = new System.Drawing.Size(221, 23);
            this.lblCliente.TabIndex = 1;
            this.lblCliente.Text = "Nombre Cliente: Juan Pérez";
            // 
            // lblEstado
            // 
            this.lblEstado.AutoSize = true;
            this.lblEstado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.lblEstado.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblEstado.ForeColor = System.Drawing.Color.White;
            this.lblEstado.Location = new System.Drawing.Point(627, 20);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.lblEstado.Size = new System.Drawing.Size(134, 33);
            this.lblEstado.TabIndex = 2;
            this.lblEstado.Text = "EN PROCESO";
            // 
            // gbFormaPago
            // 
            this.gbFormaPago.Controls.Add(this.rbEfectivo);
            this.gbFormaPago.Controls.Add(this.rbTransferencia);
            this.gbFormaPago.Controls.Add(this.rbDatafono);
            this.gbFormaPago.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.gbFormaPago.Location = new System.Drawing.Point(26, 247);
            this.gbFormaPago.Name = "gbFormaPago";
            this.gbFormaPago.Size = new System.Drawing.Size(714, 70);
            this.gbFormaPago.TabIndex = 1;
            this.gbFormaPago.TabStop = false;
            this.gbFormaPago.Text = "Forma de Pago";
            // 
            // rbEfectivo
            // 
            this.rbEfectivo.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbEfectivo.Checked = true;
            this.rbEfectivo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbEfectivo.FlatAppearance.BorderSize = 0;
            this.rbEfectivo.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.rbEfectivo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.rbEfectivo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbEfectivo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.rbEfectivo.Location = new System.Drawing.Point(20, 25);
            this.rbEfectivo.Name = "rbEfectivo";
            this.rbEfectivo.Size = new System.Drawing.Size(150, 30);
            this.rbEfectivo.TabIndex = 2;
            this.rbEfectivo.TabStop = true;
            this.rbEfectivo.Text = "💵 EFECTIVO";
            this.rbEfectivo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbEfectivo.UseVisualStyleBackColor = false;
            // 
            // rbTransferencia
            // 
            this.rbTransferencia.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbTransferencia.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbTransferencia.FlatAppearance.BorderSize = 0;
            this.rbTransferencia.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.rbTransferencia.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.rbTransferencia.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbTransferencia.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.rbTransferencia.Location = new System.Drawing.Point(200, 25);
            this.rbTransferencia.Name = "rbTransferencia";
            this.rbTransferencia.Size = new System.Drawing.Size(170, 30);
            this.rbTransferencia.TabIndex = 3;
            this.rbTransferencia.Text = "🏦 TRANSFERENCIA";
            this.rbTransferencia.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbTransferencia.UseVisualStyleBackColor = false;
            // 
            // rbDatafono
            // 
            this.rbDatafono.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbDatafono.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbDatafono.FlatAppearance.BorderSize = 0;
            this.rbDatafono.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(140)))), ((int)(((byte)(0)))));
            this.rbDatafono.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(140)))), ((int)(((byte)(0)))));
            this.rbDatafono.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbDatafono.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.rbDatafono.Location = new System.Drawing.Point(400, 25);
            this.rbDatafono.Name = "rbDatafono";
            this.rbDatafono.Size = new System.Drawing.Size(150, 30);
            this.rbDatafono.TabIndex = 4;
            this.rbDatafono.Text = "💳 DATAFONO";
            this.rbDatafono.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbDatafono.UseVisualStyleBackColor = false;
            // 
            // btnFinalizar
            // 
            this.btnFinalizar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnFinalizar.FlatAppearance.BorderSize = 0;
            this.btnFinalizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFinalizar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnFinalizar.ForeColor = System.Drawing.Color.White;
            this.btnFinalizar.Location = new System.Drawing.Point(457, 613);
            this.btnFinalizar.Name = "btnFinalizar";
            this.btnFinalizar.Size = new System.Drawing.Size(164, 45);
            this.btnFinalizar.TabIndex = 9;
            this.btnFinalizar.Text = "Finalizar Servicio";
            this.btnFinalizar.UseVisualStyleBackColor = false;
            this.btnFinalizar.Click += new System.EventHandler(this.btnFinalizar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnCancelar.FlatAppearance.BorderSize = 0;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancelar.ForeColor = System.Drawing.Color.White;
            this.btnCancelar.Location = new System.Drawing.Point(627, 613);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(164, 45);
            this.btnCancelar.TabIndex = 10;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnAsignarOperador
            // 
            this.btnAsignarOperador.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnAsignarOperador.FlatAppearance.BorderSize = 0;
            this.btnAsignarOperador.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAsignarOperador.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAsignarOperador.ForeColor = System.Drawing.Color.White;
            this.btnAsignarOperador.Location = new System.Drawing.Point(287, 613);
            this.btnAsignarOperador.Name = "btnAsignarOperador";
            this.btnAsignarOperador.Size = new System.Drawing.Size(164, 45);
            this.btnAsignarOperador.TabIndex = 8;
            this.btnAsignarOperador.Text = "Asignar Operador";
            this.btnAsignarOperador.UseVisualStyleBackColor = false;
            this.btnAsignarOperador.Click += new System.EventHandler(this.btnAsignarOperador_Click);
            // 
            // btnLiberarOperador
            // 
            this.btnLiberarOperador.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnLiberarOperador.FlatAppearance.BorderSize = 0;
            this.btnLiberarOperador.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLiberarOperador.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLiberarOperador.ForeColor = System.Drawing.Color.White;
            this.btnLiberarOperador.Location = new System.Drawing.Point(26, 613);
            this.btnLiberarOperador.Name = "btnLiberarOperador";
            this.btnLiberarOperador.Size = new System.Drawing.Size(164, 45);
            this.btnLiberarOperador.TabIndex = 7;
            this.btnLiberarOperador.Text = "Liberar Operador";
            this.btnLiberarOperador.UseVisualStyleBackColor = false;
            this.btnLiberarOperador.Click += new System.EventHandler(this.btnLiberarOperador_Click);
            // 
            // FrmGestionDetalleVehiculo
            // 
            this.AcceptButton = this.btnFinalizar;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(823, 676);
            this.Controls.Add(this.btnLiberarOperador);
            this.Controls.Add(this.btnAsignarOperador);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.cardInfo);
            this.Controls.Add(this.btnFinalizar);
            this.Controls.Add(this.btnCancelar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmGestionDetalleVehiculo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.FrmGestionDetalleVehiculo_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.cardInfo.ResumeLayout(false);
            this.cardInfo.PerformLayout();
            this.gbFormaPago.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblFechaIngreso;
        private System.Windows.Forms.Label lblTurno;
        private System.Windows.Forms.Label lblCelular;
        private System.Windows.Forms.Label lblServicio;
        private System.Windows.Forms.Label lblTipoVehiculo;
        private System.Windows.Forms.Label lblOperadorAsignado;
        private System.Windows.Forms.TextBox txtValorPagar;
        private System.Windows.Forms.Label lblValorPagar;
        private System.Windows.Forms.Label lblObservaciones;
        private System.Windows.Forms.TextBox txtObservaciones;
        private System.Windows.Forms.Button btnAsignarOperador;
        private System.Windows.Forms.Button btnLiberarOperador;
    }
}