using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DL.Models
{
    [Table("Horario")]
    public partial class Horario
    {
        [Key]
        public int IdHorario { get; set; }

        public int IdGrupo { get; set; }

        public string DocumentoRuta { get; set; } = null!;

        [ForeignKey("IdGrupo")]
        public virtual Grupo Grupo { get; set; } = null!;
    }
}
