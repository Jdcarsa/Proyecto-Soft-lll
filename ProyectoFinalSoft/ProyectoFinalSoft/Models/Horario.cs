using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProyectoFinalSoft.Models
{
	public class Horario
	{
		[Key] 
		public int horarioId { get; set; }
		[Column("Horario_dia", TypeName = "varchar(15)")]
		public string? horarioDia { get; set; }
		[Column("Horario_hora_inicio", TypeName = "numeric(2,0)")]
		public int horarioHoraInicio { get; set; }
		[Column("Horario_hora_fin", TypeName = "numeric(2,0)")]
		public int horarioHoraFin { get; set; }
		[Column("Horario_duracion", TypeName = "numeric(1,0)")]
		public int horarioDuracion { get; set; }
		[Column("Horario_estado", TypeName = "numeric(1,0)")]
		public int horarioEstado { get; set; }
		//relaciones simuladas
		public int ProgramaId { get; set; }
		public int CompetenciaId { get; set; }
		//relaciones existentes
		public int? ambienteId { get; set; }
		public Ambiente? ambiente { get; set; }
		public int? docenteId { get; set; }
		public Docente? docente { get; set; }
		public int? periodoAcademicoId {  get; set; }
		public PeriodoAcademico? periodoAcademico { get; set; }
	}
}
