using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalSoft.Models
{
	public class Docente
	{
		[Key]
		public int docenteId {  get; set; }
		[Column("Docente_Nombre", TypeName = "varchar(30)")]
		public string? docenteNombre { get; set; }
		[Column("Docente_Apellido", TypeName = "varchar(30)")]
		public string? docenteApellido { get; set; }
		[Column("Docente_TipoId", TypeName = "varchar(20)")]
		public string? docenteTipoId { get; set; }
		[Column("Docente_NumId", TypeName = "varchar(15)")]
		public string? docenteNumId { get; set; }
		[Column("Docente_Tipo", TypeName = "varchar(15)")]
		public string? docenteTipo { get; set; }
		[Column("Docente_TipoContraro", TypeName = "varchar(5)")]
		public string? docenteTipoContrato { get; set; }
		[Column("Docente_Area", TypeName = "varchar(100)")]
		public string? docenteArea { get; set; }
		[Column("Docente_estado", TypeName = "numeric(1,0)")]
		public int docenteEstado { get; set; }

		public int? usuarioId { get; set; }
		public Usuario? usuario { get; set; }

		//relacion 0 a muchos , un docente puede tener muchos horarios
		public ICollection<Horario> Horarios { get; } = new List<Horario>();
	}
}
