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
        private Panel cardServicios;
        private Label lblServicios;
        private System.Windows.Forms.FlowLayoutPanel flpOperadores;
        private System.Windows.Forms.Panel cardProceso;
        private System.Windows.Forms.Panel cardPromedio;
        private System.Windows.Forms.Panel cardMasLento;

        private System.Windows.Forms.Label lblTotalProceso;
        private System.Windows.Forms.Label lblPromedioTiempo;
        private System.Windows.Forms.Label lblMasLento;

        private System.Windows.Forms.DataGridView dvVehiculosProceso;

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
            this.cardServicios = new System.Windows.Forms.Panel();
            this.lblServicios = new System.Windows.Forms.Label();
            this.cardProceso = new System.Windows.Forms.Panel();
            this.lblTotalProceso = new System.Windows.Forms.Label();
            this.cardPromedio = new System.Windows.Forms.Panel();
            this.lblPromedioTiempo = new System.Windows.Forms.Label();
            this.cardMasLento = new System.Windows.Forms.Panel();
            this.lblMasLento = new System.Windows.Forms.Label();
            this.flpOperadores = new System.Windows.Forms.FlowLayoutPanel();
            this.dvVehiculosProceso = new System.Windows.Forms.DataGridView();
            this.txtBuscar = new System.Windows.Forms.TextBox();
            this.lblBuscaPlaca = new System.Windows.Forms.Label();
            this.cardServicios.SuspendLayout();
            this.cardProceso.SuspendLayout();
            this.cardPromedio.SuspendLayout();
            this.cardMasLento.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dvVehiculosProceso)).BeginInit();
            this.SuspendLayout();
            // 
            // cardServicios
            // 
            this.cardServicios.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(140)))), ((int)(((byte)(210)))));
            this.cardServicios.Controls.Add(this.lblServicios);
            this.cardServicios.Location = new System.Drawing.Point(20, 20);
            this.cardServicios.Name = "cardServicios";
            this.cardServicios.Size = new System.Drawing.Size(300, 63);
            this.cardServicios.TabIndex = 0;
            // 
            // lblServicios
            // 
            this.lblServicios.AutoSize = true;
            this.lblServicios.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblServicios.ForeColor = System.Drawing.Color.White;
            this.lblServicios.Location = new System.Drawing.Point(20, 14);
            this.lblServicios.Name = "lblServicios";
            this.lblServicios.Size = new System.Drawing.Size(196, 28);
            this.lblServicios.TabIndex = 0;
            this.lblServicios.Text = "Servicios Activos: 0";
            // 
            // cardProceso
            // 
            this.cardProceso.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.cardProceso.Controls.Add(this.lblTotalProceso);
            this.cardProceso.Location = new System.Drawing.Point(340, 20);
            this.cardProceso.Name = "cardProceso";
            this.cardProceso.Size = new System.Drawing.Size(300, 63);
            this.cardProceso.TabIndex = 1;
            // 
            // lblTotalProceso
            // 
            this.lblTotalProceso.AutoSize = true;
            this.lblTotalProceso.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTotalProceso.ForeColor = System.Drawing.Color.White;
            this.lblTotalProceso.Location = new System.Drawing.Point(20, 14);
            this.lblTotalProceso.Name = "lblTotalProceso";
            this.lblTotalProceso.Size = new System.Drawing.Size(138, 28);
            this.lblTotalProceso.TabIndex = 0;
            this.lblTotalProceso.Text = "En proceso: 0";
            // 
            // cardPromedio
            // 
            this.cardPromedio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(89)))), ((int)(((byte)(182)))));
            this.cardPromedio.Controls.Add(this.lblPromedioTiempo);
            this.cardPromedio.Location = new System.Drawing.Point(660, 20);
            this.cardPromedio.Name = "cardPromedio";
            this.cardPromedio.Size = new System.Drawing.Size(300, 63);
            this.cardPromedio.TabIndex = 2;
            // 
            // lblPromedioTiempo
            // 
            this.lblPromedioTiempo.AutoSize = true;
            this.lblPromedioTiempo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblPromedioTiempo.ForeColor = System.Drawing.Color.White;
            this.lblPromedioTiempo.Location = new System.Drawing.Point(20, 14);
            this.lblPromedioTiempo.Name = "lblPromedioTiempo";
            this.lblPromedioTiempo.Size = new System.Drawing.Size(168, 28);
            this.lblPromedioTiempo.TabIndex = 0;
            this.lblPromedioTiempo.Text = "Promedio: 0 min";
            // 
            // cardMasLento
            // 
            this.cardMasLento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.cardMasLento.Controls.Add(this.lblMasLento);
            this.cardMasLento.Location = new System.Drawing.Point(383, 432);
            this.cardMasLento.Name = "cardMasLento";
            this.cardMasLento.Size = new System.Drawing.Size(300, 63);
            this.cardMasLento.TabIndex = 3;
            this.cardMasLento.Visible = false;
            // 
            // lblMasLento
            // 
            this.lblMasLento.AutoSize = true;
            this.lblMasLento.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblMasLento.ForeColor = System.Drawing.Color.White;
            this.lblMasLento.Location = new System.Drawing.Point(20, 14);
            this.lblMasLento.Name = "lblMasLento";
            this.lblMasLento.Size = new System.Drawing.Size(125, 28);
            this.lblMasLento.TabIndex = 0;
            this.lblMasLento.Text = "Más lento: -";
            // 
            // flpOperadores
            // 
            this.flpOperadores.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpOperadores.AutoScroll = true;
            this.flpOperadores.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.flpOperadores.Location = new System.Drawing.Point(20, 90);
            this.flpOperadores.Name = "flpOperadores";
            this.flpOperadores.Padding = new System.Windows.Forms.Padding(15);
            this.flpOperadores.Size = new System.Drawing.Size(947, 336);
            this.flpOperadores.TabIndex = 4;
            this.flpOperadores.WrapContents = false;
            // 
            // dvVehiculosProceso
            // 
            this.dvVehiculosProceso.AllowUserToAddRows = false;
            this.dvVehiculosProceso.AllowUserToDeleteRows = false;
            this.dvVehiculosProceso.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dvVehiculosProceso.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dvVehiculosProceso.BackgroundColor = System.Drawing.Color.White;
            this.dvVehiculosProceso.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(76)))), ((int)(((byte)(129)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dvVehiculosProceso.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dvVehiculosProceso.ColumnHeadersHeight = 29;
            this.dvVehiculosProceso.EnableHeadersVisualStyles = false;
            this.dvVehiculosProceso.Location = new System.Drawing.Point(20, 459);
            this.dvVehiculosProceso.Name = "dvVehiculosProceso";
            this.dvVehiculosProceso.ReadOnly = true;
            this.dvVehiculosProceso.RowHeadersVisible = false;
            this.dvVehiculosProceso.RowHeadersWidth = 51;
            this.dvVehiculosProceso.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dvVehiculosProceso.Size = new System.Drawing.Size(947, 452);
            this.dvVehiculosProceso.TabIndex = 5;
            this.dvVehiculosProceso.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dvVehiculosProceso_CellDoubleClick);
            // 
            // txtBuscar
            // 
            this.txtBuscar.Location = new System.Drawing.Point(133, 433);
            this.txtBuscar.MaxLength = 7;
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.Size = new System.Drawing.Size(100, 22);
            this.txtBuscar.TabIndex = 6;
            this.txtBuscar.TextChanged += new System.EventHandler(this.txtBuscar_TextChanged);
            // 
            // lblBuscaPlaca
            // 
            this.lblBuscaPlaca.AutoSize = true;
            this.lblBuscaPlaca.BackColor = System.Drawing.Color.White;
            this.lblBuscaPlaca.Location = new System.Drawing.Point(20, 435);
            this.lblBuscaPlaca.Name = "lblBuscaPlaca";
            this.lblBuscaPlaca.Size = new System.Drawing.Size(99, 16);
            this.lblBuscaPlaca.TabIndex = 7;
            this.lblBuscaPlaca.Text = "Buscar x Placa:";
            // 
            // FrmDashboard
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.ClientSize = new System.Drawing.Size(979, 920);
            this.Controls.Add(this.lblBuscaPlaca);
            this.Controls.Add(this.txtBuscar);
            this.Controls.Add(this.cardServicios);
            this.Controls.Add(this.cardProceso);
            this.Controls.Add(this.cardPromedio);
            this.Controls.Add(this.flpOperadores);
            this.Controls.Add(this.dvVehiculosProceso);
            this.Controls.Add(this.cardMasLento);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmDashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dashboard";
            this.Load += new System.EventHandler(this.FrmDashboard_Load);
            this.cardServicios.ResumeLayout(false);
            this.cardServicios.PerformLayout();
            this.cardProceso.ResumeLayout(false);
            this.cardProceso.PerformLayout();
            this.cardPromedio.ResumeLayout(false);
            this.cardPromedio.PerformLayout();
            this.cardMasLento.ResumeLayout(false);
            this.cardMasLento.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dvVehiculosProceso)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        #endregion

        private TextBox txtBuscar;
        private Label lblBuscaPlaca;
    }
}
