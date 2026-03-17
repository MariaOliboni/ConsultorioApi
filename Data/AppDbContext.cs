using ConsultorioApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsultorioApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Models.Consultorio> Consultorios { get; set; }
        public DbSet<Medico> Medicos { get; set; }

    }
}
