using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalSoft.Models;

namespace ProyectoFinalSoft.Services
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions options) : base(options) { }

		public DbSet<Ambiente> Ambientes { get; set; }
		public DbSet<Docente> Docentes { get; set; }
		public DbSet<Horario> Horarios { get; set; }
		public DbSet<PeriodoAcademico> PeriodosAcademicos { get; set; }
		public DbSet<Usuario> Usuarios { get; set; }
		public DbSet<Programa> Programas { get; set; }
		public DbSet<Competencia> Competencias { get; set; }
	}
}
