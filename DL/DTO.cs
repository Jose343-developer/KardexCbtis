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

    public class EmpleadoGetAll
    {
        public int IdEmpleado { get; set; }
        public string? Curp { get; set; }
        public string? Rfc { get; set; }
        public string? Nombre { get; set; }
        public string? ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public string? Correo { get; set; }
        public string? Telefono { get; set; }
        public string? Celular { get; set; }
        public string? Departamento { get; set; }
        public int IdUsuario { get; set; }
        public string? NombreUser { get; set; }
        public string? Password { get; set; }
        public bool? Estatus { get; set; }
        public int IdRol { get; set; }
        public string? NombreRol { get; set; }
    }

    public class MateriaGetAll
    {
        public int IdMateria { get; set; }
        public string? Nombre { get; set; }
        public byte Semestre { get; set; }
        public int Creditos { get; set; }
        public int? IdEspecialidad { get; set; }
        public string? NombreEspecialidad { get; set; }
        public string? ClaveOficial { get; set; }
    }

    public class AsignacionDocenteGetAll
    {
        public int IdAsignacionDocente { get; set; }
        public string? DiaSemana { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public int IdEmpleado { get; set; }
        public string? NombreEmpleado { get; set; }
        public string? ApellidoPaternoEmpleado { get; set; }
        public string? ApellidoMaternoEmpleado { get; set; }
        public int IdMateria { get; set; }
        public string? NombreMateria { get; set; }
        public byte SemestreMateria { get; set; }
        public int IdGrupo { get; set; }
        public byte SemestreGrupo { get; set; }
        public string? LetraGrupo { get; set; }
        public string? TurnoGrupo { get; set; }
    }

    public class CalificacionGetByAlumno
    {
        public int IdCalificacion { get; set; }
        public decimal Nota { get; set; }
        public int IdMateria { get; set; }
        public string? NombreMateria { get; set; }
        public byte Semestre { get; set; }
        public int Creditos { get; set; }
    }

    public class GrupoGetTurnos
    {
        public string? Turno { get; set; }
    }

    public class GrupoGetSemestres
    {
        public byte? Semestre { get; set; }
    }

    public class UsuarioGetParaLogin
    {
        public int IdUsuario { get; set; }
        public int IdRol { get; set; }
        public string Password { get; set; } = null!;
        public bool? Estatus { get; set; }
    }

    public class LoginUsuarioResult
    {
        public int IdUsuario { get; set; }
        public int IdRol { get; set; }
    }
}


