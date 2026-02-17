using System.Drawing;
using System.Windows.Forms;

namespace CarWash.Presentacion.Principal
{
    partial class FrmPrincipal
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
        /// 
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Label lblBienvenido;
        private System.Windows.Forms.Button btnCerrar;

        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Button btnClientes;
        private System.Windows.Forms.Button btnServicios;
        private System.Windows.Forms.Button btnIngresoVehiculo;
        private System.Windows.Forms.Button btnReportes;

        private System.Windows.Forms.Panel cardVentas;
        private System.Windows.Forms.Panel cardServicios;
        private System.Windows.Forms.Panel cardClientes;

        private System.Windows.Forms.Label lblVentas;
        private System.Windows.Forms.Label lblServicios;
        private System.Windows.Forms.Label lblClientes;
        private System.Windows.Forms.Panel panelCard;
        private System.Windows.Forms.Label lblTitulo;


        private void InitializeComponent()
        {
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblBienvenido = new System.Windows.Forms.Label();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.panelMenu = new System.Windows.Forms.Panel();
            this.btnDashboard = new System.Windows.Forms.Button();
            this.btnClientes = new System.Windows.Forms.Button();
            this.btnServicios = new System.Windows.Forms.Button();
            this.btnReportes = new System.Windows.Forms.Button();
            this.btnIngresoVehiculo = new System.Windows.Forms.Button();
            this.panelContent = new System.Windows.Forms.Panel();
            this.cardVentas = new System.Windows.Forms.Panel();
            this.lblVentas = new System.Windows.Forms.Label();
            this.cardServicios = new System.Windows.Forms.Panel();
            this.lblServicios = new System.Windows.Forms.Label();
            this.cardClientes = new System.Windows.Forms.Panel();
            this.lblClientes = new System.Windows.Forms.Label();
            this.panelTop.SuspendLayout();
            this.panelMenu.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.cardVentas.SuspendLayout();
            this.cardServicios.SuspendLayout();
            this.cardClientes.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(76)))), ((int)(((byte)(129)))));
            this.panelTop.Controls.Add(this.lblBienvenido);
            this.panelTop.Controls.Add(this.btnCerrar);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1300, 60);
            this.panelTop.TabIndex = 2;
            // 
            // lblBienvenido
            // 
            this.lblBienvenido.AutoSize = true;
            this.lblBienvenido.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblBienvenido.ForeColor = System.Drawing.Color.White;
            this.lblBienvenido.Location = new System.Drawing.Point(20, 15);
            this.lblBienvenido.Name = "lblBienvenido";
            this.lblBienvenido.Size = new System.Drawing.Size(241, 25);
            this.lblBienvenido.TabIndex = 0;
            this.lblBienvenido.Text = "Dashboard - CarWash Pro";
            // 
            // btnCerrar
            // 
            this.btnCerrar.FlatAppearance.BorderSize = 0;
            this.btnCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrar.ForeColor = System.Drawing.Color.White;
            this.btnCerrar.Location = new System.Drawing.Point(1271, -2);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(30, 30);
            this.btnCerrar.TabIndex = 1;
            this.btnCerrar.Text = "X";
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // panelMenu
            // 
            this.panelMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(35)))), ((int)(((byte)(64)))));
            this.panelMenu.Controls.Add(this.btnDashboard);
            this.panelMenu.Controls.Add(this.btnClientes);
            this.panelMenu.Controls.Add(this.btnServicios);
            this.panelMenu.Controls.Add(this.btnReportes);
            this.panelMenu.Controls.Add(this.btnIngresoVehiculo);
            this.panelMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelMenu.Location = new System.Drawing.Point(0, 60);
            this.panelMenu.Name = "panelMenu";
            this.panelMenu.Size = new System.Drawing.Size(220, 840);
            this.panelMenu.TabIndex = 1;
            // 
            // btnDashboard
            // 
            this.btnDashboard.FlatAppearance.BorderSize = 0;
            this.btnDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDashboard.ForeColor = System.Drawing.Color.White;
            this.btnDashboard.Location = new System.Drawing.Point(10, 100);
            this.btnDashboard.Name = "btnDashboard";
            this.btnDashboard.Size = new System.Drawing.Size(200, 45);
            this.btnDashboard.TabIndex = 0;
            this.btnDashboard.Text = "Dashboard";
            // 
            // btnClientes
            // 
            this.btnClientes.FlatAppearance.BorderSize = 0;
            this.btnClientes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClientes.ForeColor = System.Drawing.Color.White;
            this.btnClientes.Location = new System.Drawing.Point(10, 160);
            this.btnClientes.Name = "btnClientes";
            this.btnClientes.Size = new System.Drawing.Size(200, 45);
            this.btnClientes.TabIndex = 1;
            this.btnClientes.Text = "Clientes";
            // 
            // btnServicios
            // 
            this.btnServicios.FlatAppearance.BorderSize = 0;
            this.btnServicios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnServicios.ForeColor = System.Drawing.Color.White;
            this.btnServicios.Location = new System.Drawing.Point(10, 220);
            this.btnServicios.Name = "btnServicios";
            this.btnServicios.Size = new System.Drawing.Size(200, 45);
            this.btnServicios.TabIndex = 2;
            this.btnServicios.Text = "Servicios";
            // 
            // btnReportes
            // 
            this.btnReportes.FlatAppearance.BorderSize = 0;
            this.btnReportes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReportes.ForeColor = System.Drawing.Color.White;
            this.btnReportes.Location = new System.Drawing.Point(10, 331);
            this.btnReportes.Name = "btnReportes";
            this.btnReportes.Size = new System.Drawing.Size(200, 45);
            this.btnReportes.TabIndex = 3;
            this.btnReportes.Text = "Reportes";
            // 
            // btnIngresoVehiculo
            // 
            this.btnIngresoVehiculo.FlatAppearance.BorderSize = 0;
            this.btnIngresoVehiculo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIngresoVehiculo.ForeColor = System.Drawing.Color.White;
            this.btnIngresoVehiculo.Location = new System.Drawing.Point(10, 280);
            this.btnIngresoVehiculo.Name = "btnIngresoVehiculo";
            this.btnIngresoVehiculo.Size = new System.Drawing.Size(200, 45);
            this.btnIngresoVehiculo.TabIndex = 4;
            this.btnIngresoVehiculo.Text = "Ingreso Vehículo";
            this.btnIngresoVehiculo.Click += new System.EventHandler(this.btnIngresoVehiculo_Click);
            // 
            // panelContent
            // 
            this.panelContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(90)))), ((int)(((byte)(150)))));
            this.panelContent.Controls.Add(this.cardVentas);
            this.panelContent.Controls.Add(this.cardServicios);
            this.panelContent.Controls.Add(this.cardClientes);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(220, 60);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(1080, 840);
            this.panelContent.TabIndex = 0;
            // 
            // cardVentas
            // 
            this.cardVentas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(111)))), ((int)(((byte)(178)))));
            this.cardVentas.Controls.Add(this.lblVentas);
            this.cardVentas.Location = new System.Drawing.Point(50, 80);
            this.cardVentas.Name = "cardVentas";
            this.cardVentas.Size = new System.Drawing.Size(300, 150);
            this.cardVentas.TabIndex = 0;
            // 
            // lblVentas
            // 
            this.lblVentas.AutoSize = true;
            this.lblVentas.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblVentas.ForeColor = System.Drawing.Color.White;
            this.lblVentas.Location = new System.Drawing.Point(30, 60);
            this.lblVentas.Name = "lblVentas";
            this.lblVentas.Size = new System.Drawing.Size(122, 21);
            this.lblVentas.TabIndex = 0;
            this.lblVentas.Text = "Ventas Hoy: $0";
            // 
            // cardServicios
            // 
            this.cardServicios.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(140)))), ((int)(((byte)(210)))));
            this.cardServicios.Controls.Add(this.lblServicios);
            this.cardServicios.Location = new System.Drawing.Point(400, 80);
            this.cardServicios.Name = "cardServicios";
            this.cardServicios.Size = new System.Drawing.Size(300, 150);
            this.cardServicios.TabIndex = 1;
            // 
            // lblServicios
            // 
            this.lblServicios.AutoSize = true;
            this.lblServicios.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblServicios.ForeColor = System.Drawing.Color.White;
            this.lblServicios.Location = new System.Drawing.Point(30, 60);
            this.lblServicios.Name = "lblServicios";
            this.lblServicios.Size = new System.Drawing.Size(156, 21);
            this.lblServicios.TabIndex = 0;
            this.lblServicios.Text = "Servicios Activos: 0";
            // 
            // cardClientes
            // 
            this.cardClientes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(90)))), ((int)(((byte)(150)))));
            this.cardClientes.Controls.Add(this.lblClientes);
            this.cardClientes.Location = new System.Drawing.Point(750, 80);
            this.cardClientes.Name = "cardClientes";
            this.cardClientes.Size = new System.Drawing.Size(300, 150);
            this.cardClientes.TabIndex = 2;
            // 
            // lblClientes
            // 
            this.lblClientes.AutoSize = true;
            this.lblClientes.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblClientes.ForeColor = System.Drawing.Color.White;
            this.lblClientes.Location = new System.Drawing.Point(30, 60);
            this.lblClientes.Name = "lblClientes";
            this.lblClientes.Size = new System.Drawing.Size(181, 21);
            this.lblClientes.TabIndex = 0;
            this.lblClientes.Text = "Clientes Registrados: 0";
            // 
            // FrmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(52)))), ((int)(((byte)(85)))));
            this.ClientSize = new System.Drawing.Size(1300, 900);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelMenu);
            this.Controls.Add(this.panelTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CarWash Pro";
            this.Load += new System.EventHandler(this.FrmPrincipal_Load);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelMenu.ResumeLayout(false);
            this.panelContent.ResumeLayout(false);
            this.cardVentas.ResumeLayout(false);
            this.cardVentas.PerformLayout();
            this.cardServicios.ResumeLayout(false);
            this.cardServicios.PerformLayout();
            this.cardClientes.ResumeLayout(false);
            this.cardClientes.PerformLayout();
            this.ResumeLayout(false);

        }


        #endregion
    }
}