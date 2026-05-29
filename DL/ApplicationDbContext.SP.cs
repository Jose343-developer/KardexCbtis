using System;
using Microsoft.EntityFrameworkCore;

namespace DL;

public partial class ApplicationDbContext :DbContext
{
public virtual DbSet<DTO.AlumnoGetAll>AlumnoGetAlls {get; set;}

public virtual DbSet<DTO.EspecialidadGetAll>EspecialidadGetAlls {get; set;}
public virtual DbSet<DTO.GrupoGetAll>GrupoGetAlls{get; set;}

public virtual DbSet<DTO.AlumnosCountSemestre>AlumnosCountSemestres{get; set;}

 partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<DTO.AlumnoGetAll>().HasNoKey();
        modelBuilder.Entity<DTO.EspecialidadGetAll>().HasNoKey();
        modelBuilder.Entity<DTO.GrupoGetAll>().HasNoKey();
        modelBuilder.Entity<DTO.AlumnosCountSemestre>().HasNoKey();
    }


}
