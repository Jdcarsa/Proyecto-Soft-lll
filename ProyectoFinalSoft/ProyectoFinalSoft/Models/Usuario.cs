using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalSoft.Models
{
	public class Usuario
	{
		[Key]
		public int usuarioId { get; set; }
		[Column("Usuario_login", TypeName = "varchar(30)")]
		public string? usuarioLogin { get; set; }
		[Column("Usuario_password", TypeName = "varchar(30)")]
		public string? usuarioPassword { get; set; }
		[Column("usuario_estado", TypeName = "numeric(1,0)")]
		public int usuarioEstado {  get; set; }
		public int? docenteId { get; set; }
		public Docente? docente { get; set; }
	}
}
