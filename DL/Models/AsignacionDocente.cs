using System;

namespace DL.Models;

public partial class AsignacionDocente
{
    public int IdAsignacionDocente { get; set; }

    public int IdEmpleado { get; set; }

    public int IdMateria { get; set; }

    public int IdGrupo { get; set; }

    public string DiaSemana { get; set; } = null!;

    public TimeSpan HoraInicio { get; set; }

    public TimeSpan HoraFin { get; set; }

    public virtual Empleado IdEmpleadoNavigation { get; set; } = null!;

    public virtual Materia IdMateriaNavigation { get; set; } = null!;

    public virtual Grupo IdGrupoNavigation { get; set; } = null!;
}
