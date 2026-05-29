using System;
using System.Collections.Generic;
using DL.Models;
using Microsoft.EntityFrameworkCore;

namespace DL;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Alumno> Alumnos { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<Especialidade> Especialidades { get; set; }

    public virtual DbSet<Grupo> Grupos { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Materia> Materias { get; set; }

    public virtual DbSet<AsignacionDocente> AsignacionDocentes { get; set; }

    public virtual DbSet<Calificacion> Calificaciones { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=localhost,1435;Database=CbtisKardex;User Id=sa;Password=P@ssw0rd2026!;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Alumno>(entity =>
        {
            entity.HasKey(e => e.IdAlumno).HasName("PK__Alumno__460B47407D63E494");

            entity.ToTable("Alumno");

            entity.HasIndex(e => e.Matricula, "UQ__Alumno__0FB9FB4F18B62470").IsUnique();

            entity.HasIndex(e => e.Curp, "UQ__Alumno__AFAC52E558558358").IsUnique();

            entity.Property(e => e.ApellidoMaterno)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ApellidoPaterno)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Celular)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Curp)
                .HasMaxLength(18)
                .IsUnicode(false);
            entity.Property(e => e.Estatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Activo");
            entity.Property(e => e.Matricula)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.IdEspecialidadNavigation).WithMany(p => p.Alumnos)
                .HasForeignKey(d => d.IdEspecialidad)
                .HasConstraintName("FK__Alumno__IdEspeci__5070F446");

            entity.HasOne(d => d.IdGrupoNavigation).WithMany(p => p.Alumnos)
                .HasForeignKey(d => d.IdGrupo)
                .HasConstraintName("FK__Alumno__IdGrupo__5165187F");
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.IdEmpleado).HasName("PK__Empleado__CE6D8B9E5404EBB4");

            entity.HasIndex(e => e.IdUsuario, "UQ__Empleado__5B65BF96215693A9").IsUnique();

            entity.HasIndex(e => e.Correo, "UQ__Empleado__60695A190C3B935A").IsUnique();

            entity.HasIndex(e => e.Rfc, "UQ__Empleado__CAF0BCA6FB3ACF74").IsUnique();

            entity.Property(e => e.ApellidoMaterno)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ApellidoPaterno)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Celular)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Curp)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Departamento)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Rfc)
                .HasMaxLength(13)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.IdUsuarioNavigation).WithOne(p => p.Empleado)
                .HasForeignKey<Empleado>(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Empleados__IdUsu__571DF1D5");
        });

        modelBuilder.Entity<Especialidade>(entity =>
        {
            entity.HasKey(e => e.IdEspecialidad).HasName("PK__Especial__693FA0AFBFC7CD2F");

            entity.Property(e => e.ClaveOficial)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Grupo>(entity =>
        {
            entity.HasKey(e => e.IdGrupo).HasName("PK__Grupos__303F6FD98BB834E6");

            entity.Property(e => e.Letra)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Turno)
                .HasMaxLength(25)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Rol__2A49584C32CDD70B");

            entity.ToTable("Rol");

            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__5B65BF977263332C");

            entity.ToTable("Usuario");

            entity.Property(e => e.Estatus).HasDefaultValue(true);
            entity.Property(e => e.NombreUser)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuario__IdRol__3E52440B");
        });

        modelBuilder.Entity<Materia>(entity =>
        {
            entity.HasKey(e => e.IdMateria).HasName("PK_Materia");

            entity.ToTable("Materia");

            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.IdEspecialidadNavigation).WithMany(p => p.Materias)
                .HasForeignKey(d => d.IdEspecialidad)
                .HasConstraintName("FK_Materia_Especialidades");
        });

        modelBuilder.Entity<AsignacionDocente>(entity =>
        {
            entity.HasKey(e => e.IdAsignacionDocente).HasName("PK_AsignacionDocente");

            entity.ToTable("AsignacionDocente");

            entity.Property(e => e.DiaSemana)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.IdEmpleadoNavigation).WithMany()
                .HasForeignKey(d => d.IdEmpleado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AsignacionDocente_Empleados");

            entity.HasOne(d => d.IdMateriaNavigation).WithMany(p => p.AsignacionDocentes)
                .HasForeignKey(d => d.IdMateria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AsignacionDocente_Materia");

            entity.HasOne(d => d.IdGrupoNavigation).WithMany()
                .HasForeignKey(d => d.IdGrupo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AsignacionDocente_Grupos");
        });

        modelBuilder.Entity<Calificacion>(entity =>
        {
            entity.HasKey(e => e.IdCalificacion).HasName("PK_Calificacion");

            entity.ToTable("Calificacion");

            entity.Property(e => e.Nota)
                .HasColumnName("Calificacion")
                .HasColumnType("decimal(4, 2)");

            entity.HasOne(d => d.IdAlumnoNavigation).WithMany()
                .HasForeignKey(d => d.IdAlumno)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Calificacion_Alumno");

            entity.HasOne(d => d.IdMateriaNavigation).WithMany(p => p.Calificaciones)
                .HasForeignKey(d => d.IdMateria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Calificacion_Materia");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
