using System;
using System.Collections.Generic;

namespace DL.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public int IdRol { get; set; }

    public string NombreUser { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool? Estatus { get; set; }

    public virtual Empleado? Empleado { get; set; }

    public virtual Rol IdRolNavigation { get; set; } = null!;
}
