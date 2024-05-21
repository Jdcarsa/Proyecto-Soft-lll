using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalSoft.Models
{
	public class PeriodoAcademico
	{
		[Key]
		public int periodoId { get; set; }

        [Display(Name = "Fecha de inicio")]
        [Required]
        [Column("Periodo_Fecha_Inicio")]
        public DateOnly periodoFechaInicio { get; set; }

        [Display(Name = "Fecha de fin")]
        [Column("Periodo_Fecha_Fin")]
        [Required]
        public DateOnly periodoFechaFin {  get; set; }

        [Display(Name = "Nombre")]
        [Column("Periodo_Nombre", TypeName = "varchar(100)")]
        [MinLength(3, ErrorMessage = "El nombre debe tener al menos 3 caracteres.")]
        [MaxLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres.")]
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string? periodoNombre { get; set; }
		[Column("Periodo_estado", TypeName = "numeric(1,0)")]
		public int periodoEstado { get; set; }

		//relacion 0 a muchos , un periodo academico puede tener muchos horarios
		public ICollection<Horario> Horarios { get; } = new List<Horario>();
	}
}
