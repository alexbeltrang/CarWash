using CarWash.Controladores;
using CarWash.Database;
using CarWash.Entidades;
using CarWash.DTOs;
using CancellationTokenSource = System.Threading.CancellationTokenSource;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarWash.Presentacion.Operacion
{
    public partial class FrmGastoCaja : Form
    {
        private readonly CajaComandosClient comandos = new CajaComandosClient();
        private readonly CancellationTokenSource lifetime = new CancellationTokenSource();
        private CajaResumenDTO? resumen;
        private CajaGastoDTO? pendiente;
        private bool ocupado;
        private bool confirmado;
        public FrmGastoCaja()
        {
            InitializeComponent();
        }

        private async void FrmGastoCaja_Load(object sender, EventArgs e)
        {
            if (!UIHelper.EnModoDiseno(this)) await CargarAsync();
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            var answer = MessageBox.Show(this, "Confirmar gasto y medio de salida?", "Gasto de caja", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            await GuardarAsync(answer == DialogResult.Yes);
        }

        private async Task GuardarAsync(bool confirmacion)
        {
            if (!confirmacion || ocupado || confirmado || resumen == null) return;
            if (pendiente == null && !validaCampos()) return;
            pendiente = pendiente ?? new CajaGastoDTO { RevisionEsperada = resumen.Revision,
                Importe = CajaComandosClient.Dinero(decimal.Parse(txtValor.Text)), IdFormaPagoSalida = ((CajaMedioDTO)medio.SelectedItem).IdFormaPago,
                Concepto = txtConcepto.Text.Trim(), Observaciones = txtObservacion.Text.Trim() };
            ocupado = true;
            ActualizarControles();
            try
            {
                var result = await comandos.GastoAsync(resumen.IdCaja, pendiente, lifetime.Token);
                if (IsDisposed) return;
                if ((int?)result.Resultado["idCaja"] != resumen.IdCaja || (int?)result.Resultado["idGasto"] is not int id || id <= 0) throw new System.IO.IOException();
                pendiente = null;
                confirmado = true;
                estado.Text = "Gasto confirmado. ID " + id;
            }
            catch (CajaHttpException error) when (error.EscrituraRechazada)
            { if (!IsDisposed) { pendiente = null; resumen = null; estado.Text = error.Message + " Consulte nuevamente."; } }
            catch (Exception error) when (error is System.IO.IOException || error is System.Net.Http.HttpRequestException || error is OperationCanceledException || error is UnauthorizedAccessException)
            { if (!IsDisposed) estado.Text = "Resultado desconocido. UUID: " + pendiente?.IdOperacion; }
            finally { ocupado = false; if (!IsDisposed) ActualizarControles(); }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void limpiacampos()
        {
            txtConcepto.Text = string.Empty;
            txtValor.Text = string.Empty;
            txtObservacion.Text = string.Empty;
        }

        private void txtValor_Enter(object sender, EventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void txtValor_KeyDown(object sender, KeyEventArgs e)
        {
            // Detecta Ctrl+V (pegar)
            if (e.Control && e.KeyCode == Keys.V)
            {
                e.SuppressKeyPress = true; // Bloquea el pegado
            }
        }

        private void txtValor_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = sender as TextBox;

            // Permitir teclas de control (backspace, delete, tab, etc.)
            if (char.IsControl(e.KeyChar))
                return;

            // Permitir solo dígitos
            if (char.IsDigit(e.KeyChar))
                return;

            // Permitir un solo punto decimal
            if (e.KeyChar == '.' && !tb.Text.Contains("."))
                return;

            // Si llega aquí, se bloquea la tecla
            e.Handled = true;
        }


        private bool validaCampos()
        {
            decimal amount;
            var valid = !string.IsNullOrWhiteSpace(txtConcepto.Text) && txtConcepto.Text.Trim().Length <= 500
                && decimal.TryParse(txtValor.Text, out amount) && amount > 0 && decimal.Round(amount, 2) == amount
                && medio.SelectedItem is CajaMedioDTO && resumen?.Estado == "ABIERTA";
            if (!valid) estado.Text = "Revise concepto, importe positivo y medio de salida.";
            return valid;
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

        private async Task CargarAsync()
        {
            if (ocupado || pendiente != null || confirmado || IsDisposed) return;
            ocupado = true;
            resumen = null;
            ActualizarControles();
            try
            {
                var active = await comandos.ActivaAsync(lifetime.Token);
                if (IsDisposed) return;
                if (active == null) { estado.Text = "No hay caja abierta."; return; }
                var loaded = await comandos.ResumenAsync(active.idCaja, lifetime.Token);
                var media = await comandos.MediosAsync(lifetime.Token);
                if (IsDisposed) return;
                var previous = (medio.SelectedItem as CajaMedioDTO)?.IdFormaPago;
                medio.DisplayMember = "Nombre";
                medio.DataSource = media.Where(row => !string.Equals(row.Nombre, "Credito", StringComparison.OrdinalIgnoreCase) && !string.Equals(row.Nombre, "Crédito", StringComparison.OrdinalIgnoreCase)).ToList();
                medio.SelectedIndex = -1;
                if (previous != null) medio.SelectedItem = ((IEnumerable<CajaMedioDTO>)medio.DataSource).FirstOrDefault(row => row.IdFormaPago == previous);
                resumen = loaded;
                estado.Text = "Jornada abierta. Seleccione el medio de salida.";
            }
            catch (Exception error) when (error is System.IO.IOException || error is System.Net.Http.HttpRequestException || error is OperationCanceledException || error is UnauthorizedAccessException)
            { if (!IsDisposed) estado.Text = "No se pudo consultar la caja. No se confirma ausencia."; }
            finally { ocupado = false; if (!IsDisposed) ActualizarControles(); }
        }

        private void ActualizarControles()
        {
            txtConcepto.Enabled = txtValor.Enabled = txtObservacion.Enabled = medio.Enabled = !ocupado && pendiente == null && !confirmado;
            btnGuardar.Enabled = !ocupado && !confirmado && resumen != null;
            btnGuardar.Text = pendiente == null ? "Guardar" : "Consultar / reintentar";
            btnConsultar.Enabled = !ocupado && pendiente == null && !confirmado;
        }

        private async void btnConsultar_Click(object sender, EventArgs e) => await CargarAsync();

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if ((ocupado || pendiente != null) && e.CloseReason == CloseReason.UserClosing)
            { e.Cancel = true; estado.Text = "Operacion pendiente. Consulte el UUID: " + pendiente?.IdOperacion; }
            base.OnFormClosing(e);
        }

    }
}
