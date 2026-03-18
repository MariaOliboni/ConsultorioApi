using ConsultorioApi.Data;
using ConsultorioApi.DTOs;
using ConsultorioApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConsultorioApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MedicosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Medico>>> GetMedico()
        {
            var pacientes = await _context.Medicos.Include(m => m.Consultorio).ToListAsync();
            return Ok(pacientes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MedicoResponseDto>> GetMedico(int id)
        {
            var medico = await _context.Medicos.Include(m => m.Consultorio).FirstOrDefaultAsync(m => m.Id == id);
            
            if (medico == null) return NotFound("Médico não encontrado");
            var medicoDto = new MedicoResponseDto
            {
                Id = medico.Id,
                Nome = medico.Nome,
                Crm = medico.Crm,
                ConsultorioId = medico.ConsultorioId,
                ConsultorioNome = medico.Consultorio!= null ? medico.Consultorio.Nome :" Consultorio não existe"
            };


            return Ok(medicoDto);
        }

            [HttpPost]
        public async Task<ActionResult<Medico>> PostMedico(Medico medico)
        {
            var consultorio = await _context.Consultorios.FindAsync(medico.ConsultorioId);
            if (consultorio == null) return NotFound("ID do consultório inválido");

            medico.Consultorio = consultorio;
            await _context.AddAsync(medico);
            await _context.SaveChangesAsync();
            return Ok(medico);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Medico>> DeleteMedico(int id)
        {
            var medico = await _context.Medicos.FindAsync(id);
            if (medico == null) NotFound();
            _context.Remove(medico);
            await _context.SaveChangesAsync();
            return Ok(medico);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Medico>> UpdateMedico(Medico medico, int id)
        {
            var medicoOld = await _context.Medicos.FindAsync(id);
            if (medicoOld == null || medico.Id != medicoOld.Id) return NotFound();

            var consultorio = await _context.Consultorios.FindAsync(medico.ConsultorioId);
            medicoOld.Consultorio = consultorio;

            medicoOld.Nome = medico.Nome;
            medicoOld.Crm = medico.Crm;
            medicoOld.ConsultorioId = medico.ConsultorioId;

            _context.Medicos.Update(medicoOld);
            await _context.SaveChangesAsync();
            return Ok(medicoOld);

        }

    }
}
