using System;
using System.Collections.Generic;

namespace DL.Models;

public partial class Materia
{
    public int IdMateria { get; set; }

    public string Nombre { get; set; } = null!;

    public byte Semestre { get; set; }

    public int Creditos { get; set; }

    public int? IdEspecialidad { get; set; }

    public virtual Especialidade? IdEspecialidadNavigation { get; set; }

    public virtual ICollection<AsignacionDocente> AsignacionDocentes { get; set; } = new List<AsignacionDocente>();

    public virtual ICollection<Calificacion> Calificaciones { get; set; } = new List<Calificacion>();
}
