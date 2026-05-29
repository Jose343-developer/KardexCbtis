using System.Security.Cryptography;
using System.ComponentModel.DataAnnotations;

namespace ML
{
    public class Alumno
    {
        public int? idAlumno {get; set;}

        [Required(ErrorMessage = "La matrícula es obligatoria")]
        [StringLength(20, ErrorMessage = "La matrícula no puede exceder los 20 caracteres")]
        [RegularExpression(@"^\d+$", ErrorMessage = "La matrícula sólo debe contener dígitos")]
        public string? Matricula {get; set;}

        [Required(ErrorMessage = "La CURP es obligatoria")]
        [StringLength(18, MinimumLength = 18, ErrorMessage = "La CURP debe tener exactamente 18 caracteres")]
        [RegularExpression(@"^[A-Z]{4}\d{6}[HM][A-Z]{5}[A-Z\d]\d$", ErrorMessage = "El formato de la CURP es inválido")]
        public string? Curp {get; set;}

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres")]
        public string? Nombre {get; set;}

        [Required(ErrorMessage = "El apellido paterno es obligatorio")]
        [StringLength(50, ErrorMessage = "El apellido paterno no puede exceder los 50 caracteres")]
        public string? ApellidoPaterno {get; set;}

        [StringLength(50, ErrorMessage = "El apellido materno no puede exceder los 50 caracteres")]
        public string? ApellidoMaterno {get; set;}

        [EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
        [StringLength(100, ErrorMessage = "El correo electrónico no puede exceder los 100 caracteres")]
        public string? Correo {get; set;}

        [RegularExpression(@"^\d{10}$", ErrorMessage = "El teléfono debe contener exactamente 10 dígitos")]
        public string? Telefono {get; set;}

        [RegularExpression(@"^\d{10}$", ErrorMessage = "El celular debe contener exactamente 10 dígitos")]
        public string? Celular {get; set;}

        public string? Estatus {get; set;}  

        //PROPIEDADES DE NAVEGACION//   

        public ML.Especialidad? Especialidad {get; set;}
        public ML.Grupo? Grupo {get; set;}
    }
}