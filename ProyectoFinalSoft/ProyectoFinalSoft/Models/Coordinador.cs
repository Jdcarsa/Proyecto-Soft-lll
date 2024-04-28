using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalSoft.Models
{
    public class Coordinador
    {
        [Key]
        public int coordinadorId {  get; set; }
        [Column("Coordinador_Nombre", TypeName = "varchar(30)")]
        public string? coordinadorNombre {  get; set; }
        [Column("Coordinador_Estado", TypeName = "numeric(1,0)")]
        public int coordinadorEstado {  get; set; }
        public Usuario? usuario { get; set; }
    }
}
