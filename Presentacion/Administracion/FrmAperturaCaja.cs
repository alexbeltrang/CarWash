using CarWash.Controladores;
using CarWash.Database;
using CarWash.DTOs;
using CarWash.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CancellationTokenSource = System.Threading.CancellationTokenSource;
using System.Windows.Forms;

namespace CarWash.Presentacion.Administracion
{
    public partial class FrmAperturaCaja : Form
    {
        private readonly CajaComandosClient comandos = new CajaComandosClient();
        private readonly CancellationTokenSource lifetime = new CancellationTokenSource();
        private CajaAperturaDTO? pendiente;
        private bool ocupado;
        private bool disponible;
        private bool confirmado;

        public FrmAperturaCaja()
        {
            InitializeComponent();
        }


        private async void FrmAperturaCaja_Load(object sender, EventArgs e)
        {
            if (UIHelper.EnModoDiseno(this)) return;
            if (txtMontoInicial.Text.Length == 0) txtMontoInicial.Text = "0";
            await CargarAsync();
        }

        private async void btnAbrirCaja_Click(object sender, EventArgs e)
        {
            var answer = MessageBox.Show(this, "Confirmar apertura con el monto indicado?", "Apertura de caja", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            await ConfirmarAperturaAsync(answer == DialogResult.Yes);
        }

        private async Task ConfirmarAperturaAsync(bool confirmacion)
        {
            if (!confirmacion || ocupado || confirmado || (!disponible && pendiente == null)) return;
            decimal amount;
            if (pendiente == null && (!decimal.TryParse(txtMontoInicial.Text, out amount) || amount < 0 || decimal.Round(amount, 2) != amount))
            { estadoApertura.Text = "Ingrese un monto no negativo con hasta dos decimales."; return; }
            pendiente = pendiente ?? new CajaAperturaDTO { MontoInicial = CajaComandosClient.Dinero(decimal.Parse(txtMontoInicial.Text)) };
            ocupado = true;
            ActualizarControles();
            try
            {
                var result = await comandos.AbrirAsync(pendiente, lifetime.Token);
                if (IsDisposed) return;
                if ((int?)result.Resultado["idCaja"] is not int id || id <= 0) throw new System.IO.IOException();
                confirmado = true;
                pendiente = null;
                estadoApertura.Text = "Apertura confirmada. Caja " + id;
            }
            catch (CajaHttpException error) when (error.EscrituraRechazada)
            { if (!IsDisposed) { pendiente = null; estadoApertura.Text = error.Message; } }
            catch (Exception error) when (error is System.IO.IOException || error is System.Net.Http.HttpRequestException || error is OperationCanceledException || error is UnauthorizedAccessException)
            { if (!IsDisposed) estadoApertura.Text = "Resultado desconocido. UUID: " + pendiente?.IdOperacion; }
            finally { ocupado = false; if (!IsDisposed) ActualizarControles(); }
        }

        private async Task CargarAsync()
        {
            if (ocupado || IsDisposed || pendiente != null || confirmado) return;
            ocupado = true;
            disponible = false;
            ActualizarControles();
            try
            {
                var active = await comandos.ActivaAsync(lifetime.Token);
                if (IsDisposed) return;
                disponible = active == null;
                estadoApertura.Text = disponible ? "No hay caja abierta." : "Ya existe una caja abierta.";
            }
            catch (Exception error) when (error is System.IO.IOException || error is System.Net.Http.HttpRequestException || error is OperationCanceledException || error is UnauthorizedAccessException)
            { if (!IsDisposed) estadoApertura.Text = "No se pudo consultar la caja. Vuelva a consultar."; }
            finally { ocupado = false; if (!IsDisposed) ActualizarControles(); }
        }

        private void ActualizarControles()
        {
            txtMontoInicial.Enabled = !ocupado && pendiente == null && !confirmado;
            btnAbrirCaja.Enabled = !ocupado && !confirmado && (disponible || pendiente != null);
            btnAbrirCaja.Text = pendiente == null ? "Abrir caja" : "Consultar / reintentar";
            btnConsultar.Enabled = !ocupado && pendiente == null && !confirmado;
        }

        private async void btnConsultar_Click(object sender, EventArgs e) => await CargarAsync();

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if ((ocupado || pendiente != null) && e.CloseReason == CloseReason.UserClosing)
            { e.Cancel = true; estadoApertura.Text = "Operacion pendiente. Consulte el UUID: " + pendiente?.IdOperacion; }
            base.OnFormClosing(e);
        }

        private void MostrarToast(string mensaje, Color color)
        {
            Panel toast = new Panel();
            toast.Size = new Size(300, 50);
            toast.BackColor = color;
            toast.Left = this.Width - 320;
            toast.Top = 20;

            Label lbl = new Label();
            lbl.Text = mensaje;
            lbl.ForeColor = Color.White;
            lbl.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lbl.AutoSize = false;
            lbl.Dock = DockStyle.Fill;
            lbl.TextAlign = ContentAlignment.MiddleCenter;

            toast.Controls.Add(lbl);
            this.Controls.Add(toast);
            toast.BringToFront();

            Timer timer = new Timer();
            timer.Interval = 3000;
            timer.Tick += (s, e) =>
            {
                this.Controls.Remove(toast);
                timer.Stop();
            };
            timer.Start();
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {

        }

        private void txtMontoInicial_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmAperturaCaja_Shown(object sender, EventArgs e)
        {
            this.Activate();
        }
    }
}
