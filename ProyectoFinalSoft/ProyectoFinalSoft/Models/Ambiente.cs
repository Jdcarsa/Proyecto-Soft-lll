using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalSoft.Models
{
	public class Ambiente
	{
		[Key]
		public int ambienteId { get; set; }
		[Column("Ambiente_Nombre", TypeName = "varchar(30)")]
		public string? ambienteNombre { get; set;}
		[Column("Ambiente_ubicacion", TypeName = "varchar(100)")]
		public string? ambienteUbicacion { get; set;}
		[Column("Ambiente_tipo", TypeName = "varchar(30)")]
		public string? ambienteTipo {  get; set;}
		[Column("Ambiente_capacidad", TypeName = "numeric(3,0)")]
		public int ambienteCapacidad { get; set;}
		[Column("Ambiente_estado", TypeName = "numeric(1,0)")]
		public int ambienteEstado { get; set;}
		//relacion 0 a muchos , un ambiente puede tenr muchos horarios
		public ICollection<Horario> Horarios { get;} = new List<Horario>();
	}
}

