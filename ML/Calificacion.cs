using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ML
{
    public class Calificacion
    {
        public int IdCalificacion { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un alumno")]
        public ML.Alumno? Alumno { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una materia")]
        public ML.Materia? Materia { get; set; }

        [Required(ErrorMessage = "La calificación es obligatoria")]
        [Range(0.00, 10.00, ErrorMessage = "La calificación debe estar entre 0.00 y 10.00")]
        public decimal Nota { get; set; }

        public List<object>? Calificaciones { get; set; }
    }
}
