using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProyectoFinalSoft.Models
{
	public class Horario
	{
		[Key]
		public int horarioId { get; set; }

        [Display(Name = "Dia")]
        [Required(ErrorMessage = "El día es obligatorio.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Solo se permiten caracteres alfabéticos.")]
        [Column("Horario_dia", TypeName = "varchar(15)")]
		public string? horarioDia { get; set; }

        [Display(Name = "Hora de inicio")]
        [Required(ErrorMessage = "La hora de inicio es obligatoria.")]
        [Column("Horario_hora_inicio", TypeName = "numeric(2,0)")]
		public int horarioHoraInicio { get; set; }

        [Display(Name = "Hora fin")]
        [Required(ErrorMessage = "La hora de fin es obligatoria.")]
        [Column("Horario_hora_fin", TypeName = "numeric(2,0)")]
		public int horarioHoraFin { get; set; }

        [Display(Name = "Duracion")]
        [Required]
        [Column("Horario_duracion", TypeName = "numeric(1,0)")]
		public int horarioDuracion { get; set; }

        [Column("Horario_estado", TypeName = "numeric(1,0)")]
		public int horarioEstado { get; set; }
        //relaciones existentes
        [Display(Name = "Ambiente")]
        public int? ambienteId { get; set; }
		public Ambiente? ambiente { get; set; }
        [Display(Name = "Docente")]
        public int? docenteId { get; set; }
		public Docente? docente { get; set; }
        [Display(Name = "Periodo Academico")]
        public int? periodoAcademicoId {  get; set; }
		public PeriodoAcademico? periodoAcademico { get; set; }

        //relaciones simuladas
        [Display(Name = "Programa")]
        public int ProgramaId { get; set; }
        public Programa? programa { get; set; }
        [Display(Name = "Competencia")]
        public int CompetenciaId { get; set; }
		public Competencia? competencia { get; set; }
    }
}
