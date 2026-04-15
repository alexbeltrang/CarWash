using Carwash.API.Context;
using Carwash.API.DTOs;
using Carwash.API.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Carwash.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TurnosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TurnosController(AppDbContext context)
        {
            _context = context;
        }
        // GET: api/CajaDiaria
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.Turnos.ToListAsync();
            return Ok(data);
        }

        // GET: api/Turnos/GetById?id=5
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            try
            {
                var turno = await _context.Turnos.FindAsync(id);

                if (turno == null)
                    return NotFound();

                return Ok(turno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/Turnos
        [HttpPost("RegistrarTurno")]
        public async Task<IActionResult> Create([FromBody] Turnos turno)
        {

            _context.Turnos.Add(turno);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = turno.IdTurno }, turno);
        }

        // PUT: api/Turnos/5
        [HttpPut("ActualizarTurno/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Turnos turno)
        {
            if (id != turno.IdTurno)
                return BadRequest();


            _context.Entry(turno).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/turno/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var turno = await _context.Turnos.FindAsync(id);

            if (turno == null)
                return NotFound();

            _context.Turnos.Remove(turno);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("BuscarPorPlaca")]
        public async Task<IActionResult> BuscarPorPlaca([FromQuery] string placa)
        {
            try
            {
                if (string.IsNullOrEmpty(placa))
                    return BadRequest("La placa es obligatoria");

                var placaNormalizada = placa.Trim().ToUpper();

                var vehiculo = await _context.Turnos
                    .Where(t => t.Placa.ToUpper() == placaNormalizada)
                    .OrderByDescending(t => t.FechaHoraIngreso)
                    .Select(t => new
                    {
                        t.IdTurno,
                        t.NombreCliente,
                        t.NumeroCelular,
                        t.Placa,
                        t.IdTipoVehiculo,
                        t.Estado,
                        t.Marca
                    })
                    .FirstOrDefaultAsync();

                if (vehiculo == null)
                    return NotFound();

                return Ok(vehiculo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al buscar por placa: {ex.Message}");
            }
        }


        [HttpGet("HistorialPorPlaca")]
        public async Task<IActionResult> HistorialPorPlaca([FromQuery] string placa)
        {
            try
            {
                if (string.IsNullOrEmpty(placa))
                    return BadRequest("La placa es obligatoria");

                var placaNormalizada = placa.Trim().ToUpper();

                var historial = await (
                    from tur in _context.Turnos
                    join tvh in _context.TipoVehiculo
                        on tur.IdTipoVehiculo equals tvh.IdTipoVehiculo
                    where tur.Placa.ToUpper() == placaNormalizada
                    orderby tur.FechaHoraIngreso descending
                    select new IngresoVehiculoDTO
                    {
                        IdTurno = tur.IdTurno,
                        NumeroTurno = tur.NumeroTurno,

                        // Formato de fecha
                        FechaHoraIngreso = tur.FechaHoraIngreso
                            .ToString("yyyy-MM-dd HH:mm"),

                        NombreCliente = tur.NombreCliente,
                        Placa = tur.Placa,

                        TipoVehiculo = tvh.Nombre,

                        // Formato de dinero
                        ValorPagado = "$ " + tur.Valor.ToString("N2"),

                        Marca = tur.Marca
                    }
                ).ToListAsync();

                if (historial == null || historial.Count == 0)
                    return NotFound();

                return Ok(historial);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener historial: {ex.Message}");
            }
        }

        [HttpGet("VehiculoEnProceso")]
        public async Task<IActionResult> VehiculoEnProceso([FromQuery] int idTurno)
        {
            try
            {
                var data = await (
                    from tur in _context.Turnos
                    join tvh in _context.TipoVehiculo
                        on tur.IdTipoVehiculo equals tvh.IdTipoVehiculo

                    // LEFT JOIN Operarios
                    join ope in _context.Operarios
                        on tur.idOperario equals ope.idOperario into operariosGroup
                    from ope in operariosGroup.DefaultIfEmpty()

                    where tur.IdTurno == idTurno

                    select new
                    {
                        tur.IdTurno,
                        tur.Placa,
                        tur.NombreCliente,
                        tur.NumeroCelular,
                        tur.NumeroTurno,
                        tur.FechaHoraIngreso,
                        TipoVehiculo = tvh.Nombre,

                        OperadorAsignado =
                            ((ope != null ? ope.Nombres : "") ?? "") + " " +
                            ((ope != null ? ope.Apellidos : "") ?? ""),

                        tur.Valor,
                        tur.idOperario,
                        tur.Observaciones,
                        tur.ValorBaseComision,
                        tur.ValorComision
                    }
                ).FirstOrDefaultAsync();

                if (data == null)
                    return NotFound();

                var result = new GestionVehiculosDTO
                {
                    IdTurno = data.IdTurno,
                    Placa = data.Placa,
                    NombreCliente = data.NombreCliente,
                    NumeroCelular = data.NumeroCelular,
                    NumeroTurno = data.NumeroTurno,
                    FechaHoraIngreso = data.FechaHoraIngreso.ToString("yyyy-MM-dd HH:mm"),
                    TipoVehiculo = data.TipoVehiculo,
                    OperadorAsignado = data.OperadorAsignado.Trim(),
                    Valor = data.Valor,
                    idOperario = data.idOperario,
                    Observaciones = data.Observaciones,
                    ValorBaseComision = data.ValorBaseComision
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener vehículo en proceso: {ex.Message}");
            }
        }


    }
}
