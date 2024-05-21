using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalSoft.Models
{
	public class Ambiente
	{
		[Key]
        public int ambienteId { get; set; }

        [Required(ErrorMessage = "El codigo es obligatorio.")]
        [Display(Name = "Codigo")]
        [Column("Ambiente_Codigo", TypeName = "varchar(30)")]
        [MinLength(3, ErrorMessage = "El codigo debe tener al menos 3 caracteres.")]
        [MaxLength(30, ErrorMessage = "El codigo no puede tener más de 30 caracteres.")]
        public string? ambienteCodigo { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [Column("Ambiente_Nombre", TypeName = "varchar(30)")]
        [MinLength(3, ErrorMessage = "El nombre debe tener al menos 3 caracteres.")]
        [MaxLength(30, ErrorMessage = "El nombre no puede tener más de 30 caracteres.")]
        public string? ambienteNombre { get; set;}

        [Display(Name = "Ubicacion")]
        [Required(ErrorMessage = "La ubicacion es obligatoria.")]
        [Column("Ambiente_ubicacion", TypeName = "varchar(100)")]
        [MinLength(3, ErrorMessage = "La ubicacion debe tener al menos 3 caracteres.")]
        [MaxLength(100, ErrorMessage = "La ubicacion no puede tener más de 100 caracteres.")]
        public string? ambienteUbicacion { get; set;}

        [Display(Name = "Tipo")]
        [Required]
        [Column("Ambiente_tipo", TypeName = "varchar(30)")]
		public string? ambienteTipo {  get; set;}

        [Display(Name = "Capacidad")]
        [Required(ErrorMessage = "La capacidad es obligatoria.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Solo se permiten números.")]
        [Column("Ambiente_capacidad", TypeName = "numeric(3,0)")]
		public int ambienteCapacidad { get; set;}

		[Column("Ambiente_estado", TypeName = "numeric(1,0)")]
		public int ambienteEstado { get; set;}
		//relacion 0 a muchos , un ambiente puede tenr muchos horarios
		public ICollection<Horario> Horarios { get;} = new List<Horario>();

        public string infoCompleta
        {
            get
            {
                return $"{ambienteNombre} - {ambienteCodigo}";
            }
        }
    }
}

