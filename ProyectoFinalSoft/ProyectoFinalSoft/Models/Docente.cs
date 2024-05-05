using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalSoft.Models
{
	public class Docente
	{
		[Key]
		public int docenteId {  get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Solo se permiten caracteres alfabéticos.")]
        [Column("Docente_Nombre", TypeName = "varchar(30)")]
		public string? docenteNombre { get; set; }

        [Display(Name = "Apellido")]
        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Solo se permiten caracteres alfabéticos.")]
        [Column("Docente_Apellido", TypeName = "varchar(30)")]
		public string? docenteApellido { get; set; }

        [Display(Name = "Tipo de Identificación")]
        [Required]
        [Column("Docente_TipoId", TypeName = "varchar(20)")]
		public string? docenteTipoId { get; set; }

        [Display(Name = "Número de Identificación")]
        [Required(ErrorMessage = "El número de identificación es obligatorio.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Solo se permiten números.")]
        [Column("Docente_NumId", TypeName = "varchar(15)")]
		public string? docenteNumId { get; set; }

        [Display(Name = "Tipo")]
        [Required]
        [Column("Docente_Tipo", TypeName = "varchar(15)")]
		public string? docenteTipo { get; set; }

        [Display(Name = "Contrato")]
        [Required]
        [Column("Docente_TipoContraro", TypeName = "varchar(5)")]
		public string? docenteTipoContrato { get; set; }

        [Display(Name = "Area")]
        [Required(ErrorMessage = "El area es obligatorio.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Solo se permiten caracteres alfabéticos.")]
        [Column("Docente_Area", TypeName = "varchar(100)")]
		public string? docenteArea { get; set; }

        [Display(Name = "Estado")]
        [Column("Docente_estado", TypeName = "numeric(1,0)")]
		public int docenteEstado { get; set; }

		public Usuario? usuario { get; set; }

		//relacion 0 a muchos , un docente puede tener muchos horarios
		public ICollection<Horario> Horarios { get; } = new List<Horario>();

        public string infoCompleta
        {
            get
            {
                return $"{docenteNombre} {docenteApellido} - {docenteNumId}";
            }
        }
    }
}
