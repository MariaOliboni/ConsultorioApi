using ConsultorioApi.Data;
using ConsultorioApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Consultorio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ConsultasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Consulta>>> GetConsultas()
        {
            var consultas = await _context.Consultas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .ToListAsync();

            return Ok(consultas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Consulta>> GetConsulta(int id)
        {
            var consulta = await _context.Consultas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (consulta == null)
                return NotFound("Consulta não encontrada");

            return Ok(consulta);
        }

        [HttpPost]
        public async Task<ActionResult<Consulta>> PostConsulta(Consulta consulta)
        {
            var paciente = await _context.Pacientes.FindAsync(consulta.PacienteId);
            if (paciente == null) return NotFound("Paciente não encontrado");

            var medico = await _context.Medicos.FindAsync(consulta.MedicoId);
            if (medico == null) return NotFound("Médico não encontrado");

            var consultorioExists = await _context.Consultorios.AnyAsync(c => c.Id == medico.ConsultorioId);
            if (!consultorioExists) return BadRequest("Médico vinculado a consultório inexistente");

            consulta.Paciente = paciente;
            consulta.Medico = medico;

            consulta.DataHora = DateTime.Now;

            _context.Consultas.Add(consulta);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetConsulta), new { id = consulta.Id }, consulta);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Consulta>> UpdateConsulta(int id, Consulta consulta)
        {
            var consultaAntiga = await _context.Consultas.FindAsync(id);

            if (consultaAntiga == null || consultaAntiga.Id != consulta.Id)
                return NotFound();

            var paciente = await _context.Pacientes.FindAsync(consulta.PacienteId);
            var medico = await _context.Medicos.FindAsync(consulta.MedicoId);

            if (paciente == null) return NotFound("Paciente não encontrado");
            if (medico == null) return NotFound("Médico não encontrado");

            consultaAntiga.PacienteId = consulta.PacienteId;
            consultaAntiga.MedicoId = consulta.MedicoId;
            consultaAntiga.DataHora = consulta.DataHora;
            consultaAntiga.Observacoes = consulta.Observacoes;

            _context.Consultas.Update(consultaAntiga);
            await _context.SaveChangesAsync();

            return Ok(consultaAntiga);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteConsulta(int id)
        {
            var consulta = await _context.Consultas.FindAsync(id);

            if (consulta == null)
                return NotFound();

            _context.Consultas.Remove(consulta);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}