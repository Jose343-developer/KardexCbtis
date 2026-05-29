using System;
using System.Collections.Generic;

namespace DL.Models;

public partial class Especialidade
{
    public int IdEspecialidad { get; set; }

    public string Nombre { get; set; } = null!;

    public string ClaveOficial { get; set; } = null!;

    public virtual ICollection<Alumno> Alumnos { get; set; } = new List<Alumno>();

    public virtual ICollection<Materia> Materias { get; set; } = new List<Materia>();
}
