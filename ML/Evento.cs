using System;
using System.ComponentModel.DataAnnotations;

namespace ML
{
    public class Evento
    {
        public int IdEvento { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(100, ErrorMessage = "El título no puede exceder los 100 caracteres")]
        public string Titulo { get; set; } = null!;

        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es obligatoria")]
        public DateTime FechaFin { get; set; }

        public string Estatus { get; set; } = "confirmed";

        [StringLength(200, ErrorMessage = "La ubicación no puede exceder los 200 caracteres")]
        public string? Ubicacion { get; set; }
    }
}
