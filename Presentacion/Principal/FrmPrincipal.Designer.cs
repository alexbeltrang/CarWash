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


        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPrincipal));
            this.panelContent = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.sistemaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.abrirCajaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cerrarCajaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jornadaOperariosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reporteDiarioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.operaciónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ingresoVehículoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.administraciónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.regsitrarValesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.registrarGastoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelContent
            // 
            this.panelContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(90)))), ((int)(((byte)(150)))));
            this.panelContent.Controls.Add(this.splitContainer1);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(0, 28);
            this.panelContent.Margin = new System.Windows.Forms.Padding(4);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(1924, 1147);
            this.panelContent.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Size = new System.Drawing.Size(1924, 1147);
            this.splitContainer1.SplitterDistance = 650;
            this.splitContainer1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sistemaToolStripMenuItem,
            this.operaciónToolStripMenuItem,
            this.administraciónToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1924, 28);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // sistemaToolStripMenuItem
            // 
            this.sistemaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.abrirCajaToolStripMenuItem,
            this.cerrarCajaToolStripMenuItem,
            this.jornadaOperariosToolStripMenuItem,
            this.reporteDiarioToolStripMenuItem});
            this.sistemaToolStripMenuItem.Name = "sistemaToolStripMenuItem";
            this.sistemaToolStripMenuItem.Size = new System.Drawing.Size(75, 24);
            this.sistemaToolStripMenuItem.Text = "&Sistema";
            // 
            // abrirCajaToolStripMenuItem
            // 
            this.abrirCajaToolStripMenuItem.Name = "abrirCajaToolStripMenuItem";
            this.abrirCajaToolStripMenuItem.Size = new System.Drawing.Size(213, 26);
            this.abrirCajaToolStripMenuItem.Text = "&Abrir Caja";
            this.abrirCajaToolStripMenuItem.Click += new System.EventHandler(this.abrirCajaToolStripMenuItem_Click);
            // 
            // cerrarCajaToolStripMenuItem
            // 
            this.cerrarCajaToolStripMenuItem.Name = "cerrarCajaToolStripMenuItem";
            this.cerrarCajaToolStripMenuItem.Size = new System.Drawing.Size(213, 26);
            this.cerrarCajaToolStripMenuItem.Text = "&Cerrar Caja";
            this.cerrarCajaToolStripMenuItem.Click += new System.EventHandler(this.cerrarCajaToolStripMenuItem_Click);
            // 
            // jornadaOperariosToolStripMenuItem
            // 
            this.jornadaOperariosToolStripMenuItem.Name = "jornadaOperariosToolStripMenuItem";
            this.jornadaOperariosToolStripMenuItem.Size = new System.Drawing.Size(213, 26);
            this.jornadaOperariosToolStripMenuItem.Text = "&Jornada Operarios";
            // 
            // reporteDiarioToolStripMenuItem
            // 
            this.reporteDiarioToolStripMenuItem.Name = "reporteDiarioToolStripMenuItem";
            this.reporteDiarioToolStripMenuItem.Size = new System.Drawing.Size(213, 26);
            this.reporteDiarioToolStripMenuItem.Text = "&Reporte Diario";
            // 
            // operaciónToolStripMenuItem
            // 
            this.operaciónToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ingresoVehículoToolStripMenuItem});
            this.operaciónToolStripMenuItem.Name = "operaciónToolStripMenuItem";
            this.operaciónToolStripMenuItem.Size = new System.Drawing.Size(92, 24);
            this.operaciónToolStripMenuItem.Text = "&Operación";
            // 
            // ingresoVehículoToolStripMenuItem
            // 
            this.ingresoVehículoToolStripMenuItem.Name = "ingresoVehículoToolStripMenuItem";
            this.ingresoVehículoToolStripMenuItem.Size = new System.Drawing.Size(201, 26);
            this.ingresoVehículoToolStripMenuItem.Text = "&Ingreso Vehículo";
            // 
            // administraciónToolStripMenuItem
            // 
            this.administraciónToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.regsitrarValesToolStripMenuItem,
            this.registrarGastoToolStripMenuItem});
            this.administraciónToolStripMenuItem.Name = "administraciónToolStripMenuItem";
            this.administraciónToolStripMenuItem.Size = new System.Drawing.Size(123, 24);
            this.administraciónToolStripMenuItem.Text = "Administración";
            // 
            // regsitrarValesToolStripMenuItem
            // 
            this.regsitrarValesToolStripMenuItem.Name = "regsitrarValesToolStripMenuItem";
            this.regsitrarValesToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.regsitrarValesToolStripMenuItem.Text = "&Registrar Vales";
            this.regsitrarValesToolStripMenuItem.Click += new System.EventHandler(this.regsitrarValesToolStripMenuItem_Click);
            // 
            // registrarGastoToolStripMenuItem
            // 
            this.registrarGastoToolStripMenuItem.Name = "registrarGastoToolStripMenuItem";
            this.registrarGastoToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.registrarGastoToolStripMenuItem.Text = "R&egistrar Gasto";
            this.registrarGastoToolStripMenuItem.Click += new System.EventHandler(this.registrarGastoToolStripMenuItem_Click);
            // 
            // FrmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(52)))), ((int)(((byte)(85)))));
            this.ClientSize = new System.Drawing.Size(1924, 1175);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CarWash Pro";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmPrincipal_FormClosed);
            this.Load += new System.EventHandler(this.FrmPrincipal_Load);
            this.panelContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private Panel panelContent;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem sistemaToolStripMenuItem;
        private ToolStripMenuItem abrirCajaToolStripMenuItem;
        private ToolStripMenuItem cerrarCajaToolStripMenuItem;
        private ToolStripMenuItem jornadaOperariosToolStripMenuItem;
        private ToolStripMenuItem reporteDiarioToolStripMenuItem;
        private ToolStripMenuItem operaciónToolStripMenuItem;
        private ToolStripMenuItem ingresoVehículoToolStripMenuItem;
        private SplitContainer splitContainer1;
        private ToolStripMenuItem administraciónToolStripMenuItem;
        private ToolStripMenuItem regsitrarValesToolStripMenuItem;
        private ToolStripMenuItem registrarGastoToolStripMenuItem;
    }
}