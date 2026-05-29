using System.ComponentModel.DataAnnotations;

namespace ML
{
    public class Usuario
    {
        public int? IdUsuario { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre de usuario no puede exceder los 100 caracteres")]
        public string? NombreUser { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(500, ErrorMessage = "La contraseña no puede exceder los 500 caracteres")]
        public string? Password { get; set; }

        public bool? Estatus { get; set; }

        public ML.Rol? Rol { get; set; }
    }
}
