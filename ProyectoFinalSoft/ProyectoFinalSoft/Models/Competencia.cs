using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalSoft.Models
{
    public class Competencia
    {
        [Key]
        public int competenciaId { get; set; }
        [Column("Competencia_Nombre", TypeName = "varchar(30)")]
        public string? competenciaNombre { get; set; }
        [Column("Competencia_Tipo", TypeName = "varchar(30)")]
        public string? competenciaTipo { get; set; }
        [Column("Competencia_Estado", TypeName = "numeric(1,0)")]
        public int competenciaEstado { get; set; }
        public ICollection<Programa>? Programas { get; set; }

        public ICollection<Horario> Horarios { get; } = new List<Horario>();
    }
}
