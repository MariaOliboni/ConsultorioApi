using ConsultorioApi.Data;
using ConsultorioApi.Models;
using ConsultorioApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Consultorio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacientesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly CpfService _cpfService;
        private readonly EmailService _emailService;

        public PacientesController(AppDbContext context, CpfService cpfService, EmailService emailService)
        {
            _context = context;
            _cpfService = cpfService;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Paciente>>> GetPaciente()
        {
            var pacientes = await _context.Pacientes.ToListAsync();
            return Ok(pacientes);
        }

        [HttpPost]
        public async Task<ActionResult<Paciente>> PostPaciente(Paciente paciente)
        {

            if (_emailService.ValidarEmail(paciente.Email) == false) throw new Exception("Email Inválido");
            if (_cpfService.ValidarCpf(paciente.Cpf) == false) throw new Exception("CPF Inválido");

            _context.Pacientes.Add(paciente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPaciente), new { id = paciente.Id }, paciente);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Paciente>> GetPaciente(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null) return NotFound();
            return paciente;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Paciente>> DeletePaciente(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null) return NotFound();
            _context.Remove(paciente);
            await _context.SaveChangesAsync();
            return Ok(paciente);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Paciente>> UpdatePaciente(int id, Paciente paciente)
        {
            var pacienteOld = await _context.Pacientes.FindAsync(id);
            if (pacienteOld == null || paciente.Id != pacienteOld.Id) return NotFound();

            if (_emailService.ValidarEmail(paciente.Email) == false) throw new Exception("Email Inválido");
            if (_cpfService.ValidarCpf(paciente.Cpf) == false) throw new Exception("CPF Inválido");

            pacienteOld.Nome = paciente.Nome;
            pacienteOld.Email = paciente.Email;
            pacienteOld.Cpf = paciente.Cpf;


            _context.Update(pacienteOld);
            await _context.SaveChangesAsync();
            return Ok(pacienteOld);
        }

    }
}