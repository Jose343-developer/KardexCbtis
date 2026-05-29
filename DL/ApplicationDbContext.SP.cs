using System;
using Microsoft.EntityFrameworkCore;

namespace DL;

public partial class ApplicationDbContext :DbContext
{
public virtual DbSet<DTO.AlumnoGetAll>AlumnoGetAlls {get; set;}

public virtual DbSet<DTO.EspecialidadGetAll>EspecialidadGetAlls {get; set;}
public virtual DbSet<DTO.GrupoGetAll>GrupoGetAlls{get; set;}

public virtual DbSet<DTO.AlumnosCountSemestre>AlumnosCountSemestres{get; set;}

public virtual DbSet<DTO.EmpleadoGetAll> EmpleadoGetAlls { get; set; }
public virtual DbSet<DTO.MateriaGetAll> MateriaGetAlls { get; set; }
public virtual DbSet<DTO.AsignacionDocenteGetAll> AsignacionDocenteGetAlls { get; set; }
public virtual DbSet<DTO.CalificacionGetByAlumno> CalificacionGetByAlumnos { get; set; }
public virtual DbSet<DTO.GrupoGetTurnos> GrupoGetTurnos { get; set; }
public virtual DbSet<DTO.GrupoGetSemestres> GrupoGetSemestres { get; set; }
public virtual DbSet<DTO.UsuarioGetParaLogin> UsuarioGetParaLogins { get; set; }
public virtual DbSet<DTO.LoginUsuarioResult> LoginUsuarioResults { get; set; }

 partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<DTO.AlumnoGetAll>().HasNoKey();
        modelBuilder.Entity<DTO.EspecialidadGetAll>().HasNoKey();
        modelBuilder.Entity<DTO.GrupoGetAll>().HasNoKey();
        modelBuilder.Entity<DTO.AlumnosCountSemestre>().HasNoKey();
        modelBuilder.Entity<DTO.EmpleadoGetAll>().HasNoKey();
        modelBuilder.Entity<DTO.MateriaGetAll>().HasNoKey();
        modelBuilder.Entity<DTO.AsignacionDocenteGetAll>().HasNoKey();
        modelBuilder.Entity<DTO.CalificacionGetByAlumno>().HasNoKey();
        modelBuilder.Entity<DTO.GrupoGetTurnos>().HasNoKey();
        modelBuilder.Entity<DTO.GrupoGetSemestres>().HasNoKey();
        modelBuilder.Entity<DTO.UsuarioGetParaLogin>().HasNoKey();
        modelBuilder.Entity<DTO.LoginUsuarioResult>().HasNoKey();
    }


}
