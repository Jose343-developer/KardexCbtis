using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ML
{
    public class Materia
    {
        public int IdMateria { get; set; }

        [Required(ErrorMessage = "El nombre de la materia es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "El semestre es obligatorio")]
        [Range(1, 6, ErrorMessage = "El semestre debe estar entre 1 y 6")]
        public byte Semestre { get; set; }

        [Required(ErrorMessage = "Los créditos son obligatorios")]
        [Range(1, 20, ErrorMessage = "Los créditos deben estar entre 1 y 20")]
        public int Creditos { get; set; }

        public ML.Especialidad? Especialidad { get; set; }

        public List<object>? Materias { get; set; }
    }
}
