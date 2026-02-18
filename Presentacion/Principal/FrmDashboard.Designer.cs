using System.Drawing;
using System.Windows.Forms;

namespace CarWash.Presentacion.Principal
{
    partial class FrmDashboard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private Panel cardVentas;
        private Panel cardServicios;
        private Panel cardClientes;

        private Label lblVentas;
        private Label lblServicios;
        private Label lblClientes;

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

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.cardVentas = new System.Windows.Forms.Panel();
            this.lblVentas = new System.Windows.Forms.Label();
            this.cardServicios = new System.Windows.Forms.Panel();
            this.lblServicios = new System.Windows.Forms.Label();
            this.cardClientes = new System.Windows.Forms.Panel();
            this.lblClientes = new System.Windows.Forms.Label();
            this.dvVehiculosProceso = new System.Windows.Forms.DataGridView();
            this.cardVentas.SuspendLayout();
            this.cardServicios.SuspendLayout();
            this.cardClientes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dvVehiculosProceso)).BeginInit();
            this.SuspendLayout();
            // 
            // cardVentas
            // 
            this.cardVentas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(111)))), ((int)(((byte)(178)))));
            this.cardVentas.Controls.Add(this.lblVentas);
            this.cardVentas.Location = new System.Drawing.Point(50, 50);
            this.cardVentas.Name = "cardVentas";
            this.cardVentas.Size = new System.Drawing.Size(300, 150);
            this.cardVentas.TabIndex = 0;
            // 
            // lblVentas
            // 
            this.lblVentas.AutoSize = true;
            this.lblVentas.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblVentas.ForeColor = System.Drawing.Color.White;
            this.lblVentas.Location = new System.Drawing.Point(20, 60);
            this.lblVentas.Name = "lblVentas";
            this.lblVentas.Size = new System.Drawing.Size(122, 21);
            this.lblVentas.TabIndex = 0;
            this.lblVentas.Text = "Ventas Hoy: $0";
            // 
            // cardServicios
            // 
            this.cardServicios.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(140)))), ((int)(((byte)(210)))));
            this.cardServicios.Controls.Add(this.lblServicios);
            this.cardServicios.Location = new System.Drawing.Point(400, 50);
            this.cardServicios.Name = "cardServicios";
            this.cardServicios.Size = new System.Drawing.Size(300, 150);
            this.cardServicios.TabIndex = 1;
            // 
            // lblServicios
            // 
            this.lblServicios.AutoSize = true;
            this.lblServicios.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblServicios.ForeColor = System.Drawing.Color.White;
            this.lblServicios.Location = new System.Drawing.Point(20, 60);
            this.lblServicios.Name = "lblServicios";
            this.lblServicios.Size = new System.Drawing.Size(156, 21);
            this.lblServicios.TabIndex = 0;
            this.lblServicios.Text = "Servicios Activos: 0";
            // 
            // cardClientes
            // 
            this.cardClientes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(90)))), ((int)(((byte)(150)))));
            this.cardClientes.Controls.Add(this.lblClientes);
            this.cardClientes.Location = new System.Drawing.Point(750, 50);
            this.cardClientes.Name = "cardClientes";
            this.cardClientes.Size = new System.Drawing.Size(300, 150);
            this.cardClientes.TabIndex = 2;
            // 
            // lblClientes
            // 
            this.lblClientes.AutoSize = true;
            this.lblClientes.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblClientes.ForeColor = System.Drawing.Color.White;
            this.lblClientes.Location = new System.Drawing.Point(20, 60);
            this.lblClientes.Name = "lblClientes";
            this.lblClientes.Size = new System.Drawing.Size(181, 21);
            this.lblClientes.TabIndex = 0;
            this.lblClientes.Text = "Clientes Registrados: 0";
            // 
            // dvVehiculosProceso
            // 
            this.dvVehiculosProceso.AllowUserToAddRows = false;
            this.dvVehiculosProceso.AllowUserToDeleteRows = false;
            this.dvVehiculosProceso.AllowUserToResizeColumns = false;
            this.dvVehiculosProceso.AllowUserToResizeRows = false;
            this.dvVehiculosProceso.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dvVehiculosProceso.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dvVehiculosProceso.BackgroundColor = System.Drawing.Color.White;
            this.dvVehiculosProceso.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(76)))), ((int)(((byte)(129)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dvVehiculosProceso.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dvVehiculosProceso.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dvVehiculosProceso.EnableHeadersVisualStyles = false;
            this.dvVehiculosProceso.Location = new System.Drawing.Point(12, 233);
            this.dvVehiculosProceso.Name = "dvVehiculosProceso";
            this.dvVehiculosProceso.ReadOnly = true;
            this.dvVehiculosProceso.RowHeadersVisible = false;
            this.dvVehiculosProceso.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dvVehiculosProceso.Size = new System.Drawing.Size(1137, 444);
            this.dvVehiculosProceso.TabIndex = 3;
            this.dvVehiculosProceso.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dvVehiculosProceso_CellDoubleClick);
            // 
            // FrmDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(52)))), ((int)(((byte)(85)))));
            this.ClientSize = new System.Drawing.Size(1161, 689);
            this.Controls.Add(this.dvVehiculosProceso);
            this.Controls.Add(this.cardVentas);
            this.Controls.Add(this.cardServicios);
            this.Controls.Add(this.cardClientes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmDashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dashboard";
            this.Load += new System.EventHandler(this.FrmDashboard_Load);
            this.cardVentas.ResumeLayout(false);
            this.cardVentas.PerformLayout();
            this.cardServicios.ResumeLayout(false);
            this.cardServicios.PerformLayout();
            this.cardClientes.ResumeLayout(false);
            this.cardClientes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dvVehiculosProceso)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DataGridView dvVehiculosProceso;
    }
}
