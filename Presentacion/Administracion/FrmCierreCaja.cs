using CarWash.Controladores;
using CarWash.Database;
using CarWash.Entidades;
using CarWash.DTOs;
using System.Threading;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarWash.Presentacion.Administracion
{
    public partial class FrmCierreCaja : Form
    {
        private decimal diferencia = 0;
        private readonly CajaComandosClient comandos = new CajaComandosClient();
        private readonly CancellationTokenSource lifetime = new CancellationTokenSource();
        private CajaResumenDTO? resumen;
        private CajaCierreDTO? pendiente;
        private bool ocupado;
        private bool confirmado;
        private readonly int? idHistorico;
        public FrmCierreCaja() : this((int?)null) { }
        public FrmCierreCaja(int idCaja) : this((int?)idCaja) { }
        private FrmCierreCaja(int? idCaja)
        {
            if (idCaja <= 0) throw new ArgumentOutOfRangeException(nameof(idCaja));
            idHistorico = idCaja;
            InitializeComponent();
            if (idHistorico.HasValue) Text = "Recierre historico - Jornada #" + idHistorico;
            btnCerrarCaja.Click += BtnCerrarCaja_Click;
            contado.ValueChanged += (sender, args) => calculaDiferencia();
            motivo.TextChanged += (sender, args) => ActualizarControles();
        }



        private async void FrmCierreCaja_Load(object sender, EventArgs e)
        {
            if (!UIHelper.EnModoDiseno(this)) await CargarAsync();
        }


        private async Task CargarAsync()
        {
            if (ocupado || IsDisposed || pendiente != null || confirmado) return;
            ocupado = true;
            ActualizarControles();
            try
            {
                resumen = null;
                estado.Text = "Consultando jornada...";
                if (idHistorico.HasValue && !CarWash.Utilidades.clsConnection.EsAdministrador)
                { estado.Text = "Revision historica requiere Administrador."; return; }
                var id = idHistorico;
                if (!id.HasValue)
                {
                    var active = await comandos.ActivaAsync(lifetime.Token);
                    if (IsDisposed) return;
                    if (active == null) { estado.Text = "No hay caja abierta."; return; }
                    id = active.idCaja;
                }
                var loaded = await comandos.ResumenAsync(id.Value, lifetime.Token);
                if (IsDisposed) return;
                if (loaded.Estado != (idHistorico.HasValue ? "EN_REVISION" : "ABIERTA"))
                { estado.Text = "La jornada no esta en el estado requerido. No se cambiara de caja."; return; }
                resumen = loaded;
                lblMontoInicialValor.Text = resumen.Apertura.ToString("N2");
                lblIngresosValor.Text = resumen.CobrosPorMedio.Sum(row => row.Importe).ToString("N2");
                lblGastosValor.Text = resumen.GastosPorMedio.Sum(row => row.Importe).ToString("N2");
                lblTotalCajaValor.Text = resumen.EfectivoEsperado.ToString("N2");
                lblIngresosEfectivo.Text = resumen.CobrosPorMedio.Where(row => row.Categoria == "EFECTIVO").Sum(row => row.Importe).ToString("N2");
                lblIngresosTransferencia.Text = resumen.CobrosPorMedio.Where(row => row.Categoria == "TRANSFERENCIA").Sum(row => row.Importe).ToString("N2");
                lblIngresosDatafono.Text = resumen.CobrosPorMedio.Where(row => row.Categoria == "DATAFONO").Sum(row => row.Importe).ToString("N2");
                lblVentasCredito.Text = resumen.CreditoPendiente.ToString("N2");
                lblTotalValesOperarios.Text = resumen.ValesPorMedio.Sum(row => row.Importe).ToString("N2");
                detalle.Rows.Clear();
                foreach (var row in resumen.CobrosPorMedio) detalle.Rows.Add("Cobro", row.Nombre, row.Importe.ToString("N2"));
                foreach (var row in resumen.GastosPorMedio) detalle.Rows.Add("Gasto", row.Nombre, row.Importe.ToString("N2"));
                foreach (var row in resumen.ValesPorMedio) detalle.Rows.Add("Vale", row.Nombre, row.Importe.ToString("N2"));
                foreach (var issue in resumen.FuentesInvalidas) detalle.Rows.Add(issue.Fuente, issue.Id, issue.Codigo);
                estado.Text = idHistorico.HasValue ? "Revision historica #" + idHistorico + ". " + (resumen.PuedeCerrar ? "Resumen verificado para recierre." : "Recierre bloqueado por diagnosticos.")
                    : !resumen.PuedeCerrar ? "Cierre bloqueado: revise los diagnosticos."
                    : resumen.DiscrepanciasAcumulados.Count > 0 ? "Fuentes verificadas; acumulados anteriores distintos." : "Jornada abierta. Resumen verificado.";
                calculaDiferencia();
            }
            catch (Exception error) when (error is System.IO.IOException || error is System.Net.Http.HttpRequestException || error is OperationCanceledException || error is UnauthorizedAccessException)
            {
                if (!IsDisposed) { resumen = null; estado.Text = "No se pudo consultar la caja. No se confirma ausencia ni cierre."; }
            }
            finally { ocupado = false; if (!IsDisposed) ActualizarControles(); }
        }

        private void ActualizarControles()
        {
            contado.Enabled = motivo.Enabled = !ocupado && pendiente == null && !confirmado;
            btnRecalcular.Enabled = !ocupado && pendiente == null && !confirmado;
            btnCerrarCaja.Text = pendiente == null ? idHistorico.HasValue ? "Confirmar recierre" : "Confirmar cierre" : "Consultar / reintentar";
            btnCerrarCaja.Enabled = !ocupado && !confirmado && (pendiente != null || (resumen?.PuedeCerrar == true && (diferencia == 0 || !string.IsNullOrWhiteSpace(motivo.Text))));
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if ((ocupado || pendiente != null) && e.CloseReason == CloseReason.UserClosing)
            { e.Cancel = true; estado.Text = "Operacion pendiente. Consulte el UUID antes de salir: " + pendiente?.IdOperacion; }
            base.OnFormClosing(e);
        }

        private void calculaDiferencia()
        {
            diferencia = contado.Value - (resumen?.EfectivoEsperado ?? 0m);
            if (diferencia < 0)
            {
                lblDiferenciaValor.ForeColor = Color.Red;
                lblDiferenciaValor.Text = diferencia.ToString("N2");
            }
            else
            {
                lblDiferenciaValor.ForeColor = Color.FromArgb(40, 167, 69);
                lblDiferenciaValor.Text = diferencia.ToString("N2");
            }
            ActualizarControles();
        }


        private async Task ConfirmarAsync(bool confirmacion)
        {
            if (!confirmacion || ocupado || confirmado || resumen == null || (!resumen.PuedeCerrar && pendiente == null)) return;
            if (pendiente == null && diferencia != 0 && string.IsNullOrWhiteSpace(motivo.Text)) return;
            ocupado = true;
            pendiente = pendiente ?? new CajaCierreDTO { RevisionEsperada = resumen.Revision, ResumenToken = resumen.ResumenToken,
                EfectivoContado = CajaComandosClient.Dinero(contado.Value), JustificacionDiferencia = motivo.Text.Trim() };
            ActualizarControles();
            try
            {
                var result = idHistorico.HasValue ? await comandos.RecerrarAsync(idHistorico.Value, pendiente, lifetime.Token)
                    : await comandos.CerrarAsync(resumen.IdCaja, pendiente, lifetime.Token);
                if (IsDisposed) return;
                if ((int?)result.Resultado["idCaja"] != resumen.IdCaja || result.Resultado["snapshot"] == null) throw new System.IO.IOException();
                confirmado = true;
                pendiente = null;
                estado.Text = "Cierre confirmado. Version " + result.Resultado["version"] + ". Diferencia: " + result.Resultado["diferencia"];
            }
            catch (CajaHttpException error) when (error.EscrituraRechazada)
            {
                if (!IsDisposed)
                {
                    pendiente = null;
                    resumen = null;
                    estado.Text = error.Message;
                }
            }
            catch (Exception error) when (error is System.IO.IOException || error is System.Net.Http.HttpRequestException || error is OperationCanceledException || error is UnauthorizedAccessException)
            { if (!IsDisposed) estado.Text = "Resultado desconocido. Conserve conteo y motivo. UUID: " + pendiente?.IdOperacion; }
            finally { ocupado = false; if (!IsDisposed) ActualizarControles(); }
        }

        private void btnCerrarVentana_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void BtnCerrarCaja_Click(object sender, EventArgs e)
        {
            var answer = MessageBox.Show(this, idHistorico.HasValue ? "Confirmar recierre de jornada historica #" + idHistorico + " con este conteo y resumen?" : "Confirmar el conteo y el resumen revisado?", "Cierre de caja",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            await ConfirmarAsync(answer == DialogResult.Yes);
        }

        private async void btnRecalcular_Click(object sender, EventArgs e)
        {
            await CargarAsync();
        }
    }
}
