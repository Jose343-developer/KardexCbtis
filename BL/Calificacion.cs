using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BL
{
    public class Calificacion
    {
        public ML.Result Add(ML.Calificacion calificacion)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.ApplicationDbContext context = new DL.ApplicationDbContext())
                {
                    // Check if grade already exists for this student and subject
                    var existing = context.Calificaciones
                        .FirstOrDefault(c => c.IdAlumno == calificacion.Alumno.idAlumno 
                                          && c.IdMateria == calificacion.Materia.IdMateria);

                    if (existing != null)
                    {
                        existing.Nota = calificacion.Nota;
                    }
                    else
                    {
                        DL.Models.Calificacion dbCalificacion = new DL.Models.Calificacion();
                        dbCalificacion.IdAlumno = calificacion.Alumno.idAlumno ?? 0;
                        dbCalificacion.IdMateria = calificacion.Materia.IdMateria;
                        dbCalificacion.Nota = calificacion.Nota;
                        context.Calificaciones.Add(dbCalificacion);
                    }

                    context.SaveChanges();
                    result.Correct = true;
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }

        public ML.Result GetByAlumno(int idAlumno)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.ApplicationDbContext context = new DL.ApplicationDbContext())
                {
                    // If no qualifications exist, seed some random ones for demo
                    SeedMockCalificacionesIfEmpty(context);

                    var query = context.Calificaciones
                        .Include(c => c.IdMateriaNavigation)
                        .Where(c => c.IdAlumno == idAlumno)
                        .ToList();

                    result.Objects = new List<object>();
                    foreach (var item in query)
                    {
                        ML.Calificacion calificacion = new ML.Calificacion();
                        calificacion.IdCalificacion = item.IdCalificacion;
                        calificacion.Nota = item.Nota;

                        calificacion.Materia = new ML.Materia();
                        if (item.IdMateriaNavigation != null)
                        {
                            calificacion.Materia.IdMateria = item.IdMateriaNavigation.IdMateria;
                            calificacion.Materia.Nombre = item.IdMateriaNavigation.Nombre;
                            calificacion.Materia.Semestre = item.IdMateriaNavigation.Semestre;
                            calificacion.Materia.Creditos = item.IdMateriaNavigation.Creditos;
                        }

                        result.Objects.Add(calificacion);
                    }
                    result.Correct = true;
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }

        public ML.Result GetStatistics()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.ApplicationDbContext context = new DL.ApplicationDbContext())
                {
                    // Ensure we have some data
                    SeedMockCalificacionesIfEmpty(context);

                    // 1. Alumnos status stats
                    int totalAlumnos = context.Alumnos.Count();
                    int activos = context.Alumnos.Count(a => a.Estatus == "Activo" || a.Estatus == null);
                    int bajas = context.Alumnos.Count(a => a.Estatus == "Baja" || a.Estatus == "Inactivo");

                    // 2. Grades stats
                    var todas = context.Calificaciones.ToList();
                    int totalGrades = todas.Count;
                    int aprobadas = todas.Count(c => c.Nota >= 6.00m);
                    int reprobadas = todas.Count(c => c.Nota < 6.00m);

                    decimal avgGeneral = totalGrades > 0 ? todas.Average(c => c.Nota) : 0;

                    // 3. Stats by Specialty
                    var especialidades = context.Especialidades.ToList();
                    var listEspecialidadStats = new List<Dictionary<string, object>>();

                    foreach (var esp in especialidades)
                    {
                        // Get students in this specialty
                        var alumnosEspIds = context.Alumnos
                            .Where(a => a.IdEspecialidad == esp.IdEspecialidad)
                            .Select(a => a.IdAlumno)
                            .ToList();

                        int totalEsp = alumnosEspIds.Count;
                        int bajasEsp = context.Alumnos.Count(a => a.IdEspecialidad == esp.IdEspecialidad && (a.Estatus == "Baja" || a.Estatus == "Inactivo"));
                        
                        var gradesEsp = context.Calificaciones
                            .Where(c => alumnosEspIds.Contains(c.IdAlumno))
                            .ToList();

                        decimal avgEsp = gradesEsp.Count > 0 ? gradesEsp.Average(c => c.Nota) : 0;
                        int aprobEsp = gradesEsp.Count(c => c.Nota >= 6.00m);
                        int reprobEsp = gradesEsp.Count(c => c.Nota < 6.00m);

                        listEspecialidadStats.Add(new Dictionary<string, object>
                        {
                            { "Nombre", esp.Nombre },
                            { "TotalAlumnos", totalEsp },
                            { "Bajas", bajasEsp },
                            { "Promedio", Math.Round(avgEsp, 2) },
                            { "Aprobadas", aprobEsp },
                            { "Reprobadas", reprobEsp }
                        });
                    }

                    Dictionary<string, object> stats = new Dictionary<string, object>
                    {
                        { "TotalAlumnos", totalAlumnos },
                        { "Activos", activos },
                        { "Bajas", bajas },
                        { "DesercionPorcentaje", totalAlumnos > 0 ? Math.Round((decimal)bajas / totalAlumnos * 100, 2) : 0 },
                        { "Aprobadas", aprobadas },
                        { "Reprobadas", reprobadas },
                        { "AprobacionPorcentaje", (aprobadas + reprobadas) > 0 ? Math.Round((decimal)aprobadas / (aprobadas + reprobadas) * 100, 2) : 0 },
                        { "ReprobacionPorcentaje", (aprobadas + reprobadas) > 0 ? Math.Round((decimal)reprobadas / (aprobadas + reprobadas) * 100, 2) : 0 },
                        { "PromedioGeneral", Math.Round(avgGeneral, 2) },
                        { "EspecialidadStats", listEspecialidadStats }
                    };

                    result.Object = stats;
                    result.Correct = true;
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }

        private void SeedMockCalificacionesIfEmpty(DL.ApplicationDbContext context)
        {
            if (!context.Calificaciones.Any())
            {
                var alumnos = context.Alumnos.ToList();
                var materias = context.Materias.ToList();
                Random rnd = new Random();

                if (alumnos.Count > 0 && materias.Count > 0)
                {
                    foreach (var alumno in alumnos)
                    {
                        // Add 3-5 random grades for each student
                        int numMaterias = rnd.Next(3, 6);
                        var shuffledMaterias = materias.OrderBy(x => rnd.Next()).Take(numMaterias);
                        foreach (var mat in shuffledMaterias)
                        {
                            DL.Models.Calificacion c = new DL.Models.Calificacion();
                            c.IdAlumno = alumno.IdAlumno;
                            c.IdMateria = mat.IdMateria;
                            // Generar notas entre 4.0 y 10.0
                            c.Nota = Math.Round((decimal)(rnd.NextDouble() * 6.0 + 4.0), 2);
                            context.Calificaciones.Add(c);
                        }
                    }
                    
                    // Let's also ensure we have at least one or two inactive/baja status students for statistics demonstration!
                    var activeAlumnos = alumnos.Where(a => a.Estatus == "Activo" || a.Estatus == null).ToList();
                    if (activeAlumnos.Count > 2)
                    {
                        activeAlumnos[0].Estatus = "Baja";
                    }

                    context.SaveChanges();
                }
            }
        }
    }
}
