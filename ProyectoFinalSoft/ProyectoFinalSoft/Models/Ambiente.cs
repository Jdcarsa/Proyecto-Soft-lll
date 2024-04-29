using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalSoft.Models
{
	public class Ambiente
	{
		[Key]
        public int ambienteId { get; set; }

        [Required]
        [Display(Name = "Codigo")]
        [Column("Ambiente_Codigo", TypeName = "varchar(30)")]
        public string? ambienteCodigo { get; set; }

        [Display(Name = "Nombre")]
        [Required]
        [Column("Ambiente_Nombre", TypeName = "varchar(30)")]
		public string? ambienteNombre { get; set;}

        [Display(Name = "Ubicacion")]
        [Required]
        [Column("Ambiente_ubicacion", TypeName = "varchar(100)")]
		public string? ambienteUbicacion { get; set;}

        [Display(Name = "Tipo")]
        [Required]
        [Column("Ambiente_tipo", TypeName = "varchar(30)")]
		public string? ambienteTipo {  get; set;}

        [Display(Name = "Capacidad")]
        [Required]
        [Column("Ambiente_capacidad", TypeName = "numeric(3,0)")]
		public int ambienteCapacidad { get; set;}

		[Column("Ambiente_estado", TypeName = "numeric(1,0)")]
		public int ambienteEstado { get; set;}
		//relacion 0 a muchos , un ambiente puede tenr muchos horarios
		public ICollection<Horario> Horarios { get;} = new List<Horario>();
	}
}

