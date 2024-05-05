using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalSoft.Models
{
    public class Programa
    {
        [Key]
        public int programaId { get; set; }
        [Column("Programa_Nombre", TypeName = "varchar(30)")]
        public string? programaNombre { get; set; }
        [Column("Programa_Estado", TypeName = "numeric(1,0)")]
        public int programaEstado { get; set; }
        public ICollection<Competencia>? Competencias { get; set; }
    }
}

