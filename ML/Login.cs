using System.ComponentModel.DataAnnotations;

namespace ML
{
    public class Login
    {
        [Required(ErrorMessage = "El usuario/correo es obligatorio")]
        public string Correo { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; } = null!;

        public bool Resultado { get; set; }

        public int IdUsuario { get; set; }

        public ML.Rol Rol { get; set; } = new ML.Rol();
    }
}
