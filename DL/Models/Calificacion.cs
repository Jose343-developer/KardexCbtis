using System;

namespace DL.Models;

public partial class Calificacion
{
    public int IdCalificacion { get; set; }

    public int IdAlumno { get; set; }

    public int IdMateria { get; set; }

    public decimal Nota { get; set; }

    public virtual Alumno IdAlumnoNavigation { get; set; } = null!;

    public virtual Materia IdMateriaNavigation { get; set; } = null!;
}
