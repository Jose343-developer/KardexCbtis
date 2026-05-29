using System;

namespace DL;

public class DTO
{
public class AlumnoGetAll
    {
        public int ?IdAlumno { get; set; }

    public string? Matricula { get; set; } = null!;

    public string? Curp { get; set; } = null!;

    public string? Nombre { get; set; } = null!;

    public string? ApellidoPaterno { get; set; } = null!;

    public string? ApellidoMaterno { get; set; }

    public string? Correo { get; set; }

    public string? Telefono { get; set; }

    public string? Celular { get; set; }

    public int? IdEspecialidad { get; set; }
 public string? NombreEspecialidad { get; set; } = null!;
    public int? IdGrupo { get; set; }

    public string? Estatus { get; set; }
    public string? ClaveOficial { get; set; } = null!;

        public byte ?Semestre { get; set; }

    public string? Letra { get; set; } = null!;

    public string ?Turno { get; set; } = null!;

    }

public class EspecialidadGetAll
    {
        
public int? IdEspecialidad { get; set; }

    public string? Nombre { get; set; } = null!;

    public string? ClaveOficial { get; set; } = null!;


    }

public class GrupoGetAll
    {
        
    public int? IdGrupo { get; set; }

    public byte? Semestre { get; set; }

    public string? Letra { get; set; } = null!;

    public string? Turno { get; set; } = null!;


        
    }
    public class AlumnosCountSemestre
    {
        public int? Cantidad {get; set;}

    }


}
