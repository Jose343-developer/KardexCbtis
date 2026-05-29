using System;
using System.Collections.Generic;

namespace DL.Models;

public partial class Grupo
{
    public int IdGrupo { get; set; }

    public byte Semestre { get; set; }

    public string Letra { get; set; } = null!;

    public string Turno { get; set; } = null!;

    public virtual ICollection<Alumno> Alumnos { get; set; } = new List<Alumno>();
}
