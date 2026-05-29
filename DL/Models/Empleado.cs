using System;
using System.Collections.Generic;

namespace DL.Models;

public partial class Empleado
{
    public int IdEmpleado { get; set; }

    public int IdUsuario { get; set; }

    public string Curp { get; set; } = null!;

    public string Rfc { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string ApellidoPaterno { get; set; } = null!;

    public string? ApellidoMaterno { get; set; }

    public string? Correo { get; set; }

    public string? Telefono { get; set; }

    public string? Celular { get; set; }

    public string Departamento { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
