    using ConsultorioApi.Data;
    using ConsultorioApi.Models;
    using ConsultorioApi.Services;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    namespace ConsultorioApi.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class ConsultoriosController : ControllerBase
        {
            private readonly AppDbContext _context;
            private readonly ViaCepService _viaCepService;

            public ConsultoriosController(AppDbContext context, ViaCepService viaCepService)
            {
                _context = context;
                _viaCepService = viaCepService;
            }

            [HttpGet]
            public async Task<ActionResult<IEnumerable<Models.Consultorio>>> GetConsultorios()
            {
                var consultorios = await _context.Consultorios.ToListAsync();
                return consultorios;
            }

            [HttpPost]
            public async Task<ActionResult<Models.Consultorio>> PostConsultorio(Models.Consultorio consultorio)
            {
                var endereco = await _viaCepService.BuscarEnderecoAsync(consultorio.Cep);

                if (endereco != null)
                {
                    consultorio.Logradouro = endereco.logradouro;
                    consultorio.Bairro = endereco.bairro;
                    consultorio.Localidade = endereco.localidade;
                    consultorio.Uf = endereco.uf;
                }

                _context.Consultorios.Add(consultorio);
                await _context.SaveChangesAsync();

                return Ok(CreatedAtAction(nameof(GetConsultorio), new { id = consultorio.Id }, consultorio));
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<Models.Consultorio>> GetConsultorio(int id)
            {
                var consultorio = await _context.Consultorios.FindAsync(id);

                if (consultorio == null) return NotFound();

                return consultorio;
            }

            [HttpDelete("{Id}")]
            public async Task<ActionResult<Models.Consultorio>> DeleteConsultorio(int Id)
            {
                var consultorio = await _context.Consultorios.FindAsync(Id);
                if (consultorio == null) return NotFound();
                _context.Remove(consultorio);
                await _context.SaveChangesAsync();
                return Ok(consultorio);
            }

            [HttpPut("{Id}")]
            public async Task<ActionResult<Models.Consultorio>> UpdateConsultorio(int Id, Models.Consultorio consultorio)
            {
                var consultorioOld = await _context.Consultorios.FindAsync(Id);
                if (consultorioOld == null || consultorio.Id != consultorioOld.Id) return NotFound();

                consultorioOld.Nome = consultorio.Nome;
                consultorioOld.Cep = consultorio.Cep;
                consultorioOld.Numero = consultorio.Numero;


                _context.Update(consultorioOld);
                await _context.SaveChangesAsync();
                return Ok(consultorioOld);
            }
        }
    }