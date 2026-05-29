using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ML
{
    public class AsignacionDocente
    {
        public int IdAsignacionDocente { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un docente")]
        public ML.Empleado? Empleado { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una materia")]
        public ML.Materia? Materia { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un grupo")]
        public ML.Grupo? Grupo { get; set; }

        [Required(ErrorMessage = "El día de la semana es obligatorio")]
        public string? DiaSemana { get; set; }

        [Required(ErrorMessage = "La hora de inicio es obligatoria")]
        public TimeSpan HoraInicio { get; set; }

        [Required(ErrorMessage = "La hora de fin es obligatoria")]
        public TimeSpan HoraFin { get; set; }

        public List<object>? Asignaciones { get; set; }
    }
}
