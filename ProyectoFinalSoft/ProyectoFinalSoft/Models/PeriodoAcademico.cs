using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalSoft.Models
{
	public class PeriodoAcademico
	{
		[Key]
		public int periodoId { get; set; }
		public DateOnly periodoFechaInicio { get; set; }
		public DateOnly periodoFechaFin {  get; set; }
		[Column("Periodo_Nombre", TypeName = "varchar(100)")]
		public string? periodoNombre { get; set; }
		[Column("Periodo_estado", TypeName = "numeric(1,0)")]
		public int periodoEstado { get; set; }

		//relacion 0 a muchos , un periodo academico puede tener muchos horarios
		public ICollection<Horario> Horarios { get; } = new List<Horario>();
	}
}
