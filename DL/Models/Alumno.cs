using System;
using System.Collections.Generic;

namespace DL.Models;

public partial class Alumno
{
    public int IdAlumno { get; set; }

    public string Matricula { get; set; } = null!;

    public string Curp { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string ApellidoPaterno { get; set; } = null!;

    public string? ApellidoMaterno { get; set; }

    public string? Correo { get; set; }

    public string? Telefono { get; set; }

    public string? Celular { get; set; }

    public int? IdEspecialidad { get; set; }

    public int? IdGrupo { get; set; }

    public string? Estatus { get; set; }

    public virtual Especialidade? IdEspecialidadNavigation { get; set; }

    public virtual Grupo? IdGrupoNavigation { get; set; }
}
