using System;
using System.Collections.Generic;

namespace DL.Models;

public partial class Evento
{
    public int IdEvento { get; set; }

    public string Titulo { get; set; } = null!;

    public string? Descripcion { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime FechaFin { get; set; }

    public string Estatus { get; set; } = null!;

    public string? Ubicacion { get; set; }
}
