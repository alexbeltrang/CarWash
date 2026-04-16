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
using System.Windows.Forms;

namespace CarWash.Presentacion.Administracion.Nomina
{
    public partial class FrmLiquidacionNominaOperarios : Form
    {
        OperariosController operariosController = new OperariosController();
        OperarioComisionesController operarioComisionesController = new OperarioComisionesController();
        TurnosController turnosController = new TurnosController();

        HashSet<DateTime> festivos = new HashSet<DateTime>();
        List<LiquidacionResultadoDTO> resultadoTodos = new List<LiquidacionResultadoDTO>();
        public FrmLiquidacionNominaOperarios()
        {
            InitializeComponent();
        }


        private void FrmLiquidacionNominaOperarios_Load(object sender, EventArgs e)
        {
            CargarOperarios();

        }


        private void btnCalcular_Click(object sender, EventArgs e)
        {
            festivos = ObtenerFestivos(dtpFechaInicial.Value, dtpFechaFinal.Value);

            if (cmbOperario.SelectedIndex == 0)
            {
                resultadoTodos = LiquidarTodos(dtpFechaInicial.Value, dtpFechaFinal.Value);
                if (resultadoTodos.Count > 0)
                {
                    dtgNominaEmpleados.DataSource = resultadoTodos;
                    FormateaDatagrid();
                }
                else
                {
                    MessageBox.Show("No se encontraron servicios para los operarios en el rango de fechas indicado.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dtgNominaEmpleados.DataSource = null;
                }
            }
            else
            {
                var resultado = LiquidarOperario((int)cmbOperario.SelectedValue,
                    dtpFechaInicial.Value,
                    dtpFechaFinal.Value,
                    festivos);
                if (resultado.IdOperario != 0)
                {
                    resultadoTodos.Add(resultado);
                    dtgNominaEmpleados.DataSource = new List<LiquidacionResultadoDTO> { resultado };
                    FormateaDatagrid();
                }
                else
                {
                    MessageBox.Show("No se encontraron servicios para el operario seleccionado en el rango de fechas indicado.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dtgNominaEmpleados.DataSource = null;

                }


            }


        }

        private void FormateaDatagrid()
        {
            dtgNominaEmpleados.Columns["Comision"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dtgNominaEmpleados.Columns["TotalServicios"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dtgNominaEmpleados.Columns["TotalFacturado"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dtgNominaEmpleados.Columns["Vales"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dtgNominaEmpleados.Columns["TotalPagar"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dtgNominaEmpleados.Columns["Comision"].DefaultCellStyle.Format = "$ #,##0";
            dtgNominaEmpleados.Columns["TotalFacturado"].DefaultCellStyle.Format = "$ #,##0";
            dtgNominaEmpleados.Columns["Vales"].DefaultCellStyle.Format = "$ #,##0";
            dtgNominaEmpleados.Columns["TotalPagar"].DefaultCellStyle.Format = "$ #,##0";

            dtgNominaEmpleados.Columns["IdOperario"].Visible = false;
            dtgNominaEmpleados.Columns["NombreOperario"].HeaderText = "Operario";
            dtgNominaEmpleados.Columns["TotalServicios"].HeaderText = "Total Servicios";
            dtgNominaEmpleados.Columns["TotalFacturado"].HeaderText = "Total Facturado";
            dtgNominaEmpleados.Columns["Comision"].HeaderText = "Valor Comisión";
            dtgNominaEmpleados.Columns["Vales"].HeaderText = "Total Vales";
            dtgNominaEmpleados.Columns["TotalPagar"].HeaderText = "Total a Pagar";
        }


        public void CargarOperarios()
        {
            var operarios = operariosController.GetOperariosActivos();

            var operadoresDisponibles = operarios
                            .Select(o => new OperariosDTO
                            {
                                idOperario = o.idOperario,
                                Nombres = o.Nombres,
                                Apellidos = o.Apellidos
                            })
                            .ToList();

            operadoresDisponibles.Insert(0, new OperariosDTO
            {
                idOperario = 0,
                Nombres = "-- Seleccione --",
                Apellidos = ""
            });

            cmbOperario.DataSource = operadoresDisponibles;
            cmbOperario.DisplayMember = "NombreCompleto";
            cmbOperario.ValueMember = "idOperario";
            cmbOperario.SelectedIndex = 0;
        }

        private decimal ObtenerPorcentaje(DateTime fecha, HashSet<DateTime> festivos, Dictionary<int, decimal> comisiones)
        {
            if (festivos.Contains(fecha.Date))
                return 0.40m;

            int dia = (int)fecha.DayOfWeek;

            if (comisiones.ContainsKey(dia))
                return comisiones[dia];

            return 0.35m;
        }


        public Dictionary<int, decimal> ObtenerComisionesOperario(int idOperario)
        {

            var listaObtenida = operarioComisionesController.ComisionbyOperario(idOperario);


            var lista = listaObtenida
                            .Select(o => new ComisionDTO
                            {
                                DiaSemana = o.DiaSemana,
                                Porcentaje = o.Porcentaje
                            })
                            .ToList();

            return lista.ToDictionary(x => x.DiaSemana, x => x.Porcentaje / 100m);
        }

        private List<ServicioNominaDTO> ObtenerServicios(int idOperario, DateTime fi, DateTime ff)
        {
            return turnosController.PendientesNomina(idOperario, fi, ff);
        }


        private void ActualizarServicio(int idOperario, DateTime fi, DateTime ff)
        {
            string sql = @"
                           UPDATE Turnos SET PagadoNomina = 1
                            WHERE IdOperario = ?
                            AND date(FechaHoraIngreso / 10000000 - 62135596800, 'unixepoch')
                            BETWEEN date(?) AND date(?)";
            DatabaseQueryLDB.ExecuteNonQuery(sql, idOperario, fi.ToString("yyyy-MM-dd"), ff.ToString("yyyy-MM-dd"));
        }


        private decimal ObtenerVales(int idOperario, DateTime fi, DateTime ff)
        {
            string sql = @"SELECT IFNULL(SUM(Valor),0)
                           FROM ValesOperarios
                           WHERE idOperario = ? 
                           AND date(FechaRegsitro / 10000000 - 62135596800, 'unixepoch') BETWEEN date(?) AND date(?)";

            return DatabaseQueryLDB.ExecuteScalar<decimal>(sql, idOperario, fi.ToString("yyyy-MM-dd"), ff.ToString("yyyy-MM-dd"));
        }

        private bool TieneFaltaInjustificada(int idOperario, DateTime fi, DateTime ff)
        {
            string sql = @"
                         SELECT COUNT(*)
                         FROM AsistenciaOperario
                         WHERE idOperario = ?
                         AND Asistio = 0
                         AND Autorizado = 0
                         AND date(Fecha / 10000000 - 62135596800, 'unixepoch') BETWEEN date(?) AND date(?)";

            int faltas = DatabaseQueryLDB.ExecuteScalar<int>(sql, idOperario, fi.ToString("yyyy-MM-dd"), ff.ToString("yyyy-MM-dd"));

            return faltas > 0;
        }


        public LiquidacionResultadoDTO LiquidarOperario(int idOperario, DateTime fechaInicial, DateTime fechaFinal, HashSet<DateTime> festivos)
        {
            LiquidacionResultadoDTO liquidacionResultado = new LiquidacionResultadoDTO();

            var servicios = ObtenerServicios(idOperario, fechaInicial, fechaFinal);
            if (servicios.Count > 0)
            {
                bool faltaInjustificada = TieneFaltaInjustificada(idOperario, fechaInicial, fechaFinal);

                decimal totalFacturado = 0;
                decimal comision = 0;
                var comisiones = ObtenerComisionesOperario(idOperario);

                foreach (var s in servicios)
                {
                    decimal porcentaje = ObtenerPorcentaje(s.Fecha, festivos, comisiones);

                    if (faltaInjustificada)
                        porcentaje = 0.35m;

                    totalFacturado += s.ValorServicio;
                    comision += s.ValorServicio * porcentaje;
                }

                decimal vales = ObtenerVales(idOperario, fechaInicial, fechaFinal);

                decimal totalPagar = comision - vales;

                liquidacionResultado.IdOperario = idOperario;
                liquidacionResultado = new LiquidacionResultadoDTO
                {
                    IdOperario = idOperario,
                    TotalServicios = servicios.Count,
                    TotalFacturado = totalFacturado,
                    Comision = comision,
                    Vales = vales,
                    TotalPagar = totalPagar,
                    NombreOperario = DatabaseQueryLDB.ExecuteScalar<string>("SELECT COALESCE(Nombres,'') || ' ' || COALESCE(Apellidos,'') FROM Operarios WHERE idOperario = ?", idOperario)
                };

            }
            return liquidacionResultado;
        }

        public List<LiquidacionResultadoDTO> LiquidarTodos(DateTime fechaInicial, DateTime fechaFinal)
        {
            string sql = "SELECT IdOperario, COALESCE(Nombres,'') || ' ' || COALESCE(Apellidos,'') AS Nombres FROM Operarios WHERE isDelete = 0";

            var operarios = DatabaseQueryLDB.ExecuteList<OperarioSimpleDTO>(sql).ToList();

            List<LiquidacionResultadoDTO> resultado = new List<LiquidacionResultadoDTO>();

            foreach (var idOperario in operarios)
            {
                var liquidacion = LiquidarOperario(idOperario.IdOperario, fechaInicial, fechaFinal, festivos);
                if (liquidacion.IdOperario != 0)
                {
                    resultado.Add(liquidacion);
                }
            }

            return resultado;
        }

        public HashSet<DateTime> ObtenerFestivos(DateTime fechaInicial, DateTime fechaFinal)
        {
            string sql = @" SELECT Fecha
                            FROM Festivos
                            WHERE date(Fecha) BETWEEN date(?) AND date(?)";

            var lista = DatabaseQueryLDB.ExecuteList<DateTime>(sql, fechaInicial.ToString("yyyy-MM-dd"), fechaFinal.ToString("yyyy-MM-dd"));

            return new HashSet<DateTime>(lista.Select(f => f.Date));
        }


        public void GuardarLiquidacion(LiquidacionResultadoDTO r, DateTime fi, DateTime ff)
        {
            string sql = @"INSERT INTO LiquidacionOperario (IdOperario,FechaInicial,FechaFinal,TotalServicios,TotalFacturado,Comision,Vales,TotalPagado,FechaLiquidacion)
                           VALUES (?,?,?,?,?,?,?,?,?)";

            DatabaseQueryLDB.ExecuteNonQuery(sql,
                r.IdOperario,
                fi,
                ff,
                r.TotalServicios,
                r.TotalFacturado,
                r.Comision,
                r.Vales,
                r.TotalPagar,
                DateTime.Now);
        }

        private void btnLiquidar_Click(object sender, EventArgs e)
        {
            foreach (var r in resultadoTodos)
            {
                GuardarLiquidacion(r, dtpFechaInicial.Value, dtpFechaFinal.Value);
                ActualizarServicio(r.IdOperario, dtpFechaInicial.Value, dtpFechaFinal.Value);
            }
        }
    }
}
