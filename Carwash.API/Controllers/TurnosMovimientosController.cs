using Carwash.API.Context;
using Carwash.API.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Carwash.API.Controllers
{
    public class TurnosMovimientosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TurnosMovimientosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/CajaDiaria
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.TurnosMovimientos.ToListAsync();
            return Ok(data);
        }

        // GET: api/Turnos/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var turno = await _context.TurnosMovimientos.FindAsync(id);

            if (turno == null)
                return NotFound();

            return Ok(turno);
        }

        // POST: api/Turnos
        [HttpPost("RegistrarTurnoMovimiento")]
        public async Task<IActionResult> Create([FromBody] TurnosMovimientos turnoMovimiento)
        {
            try
            {
                _context.TurnosMovimientos.Add(turnoMovimiento);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = turnoMovimiento.IdTurnoMovimientos }, turnoMovimiento);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("ActualizarTurnoMovimiento/{id}")]
        public async Task<IActionResult> ActualizarTurnoMovimiento(int id, [FromBody] TurnosMovimientos turnoMovimiento)
        {
            if (id != turnoMovimiento.IdTurnoMovimientos)
                return BadRequest();

            _context.Entry(turnoMovimiento).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }


        // DELETE: api/turno/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var turno = await _context.TurnosMovimientos.FindAsync(id);

            if (turno == null)
                return NotFound();

            _context.TurnosMovimientos.Remove(turno);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
