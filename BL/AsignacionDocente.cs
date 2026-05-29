using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BL
{
    public class AsignacionDocente
    {
        public ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.ApplicationDbContext context = new DL.ApplicationDbContext())
                {
                    var query = context.AsignacionDocentes
                        .Include(a => a.IdEmpleadoNavigation)
                        .Include(a => a.IdMateriaNavigation)
                        .Include(a => a.IdGrupoNavigation)
                        .ToList();

                    if (query != null && query.Count > 0)
                    {
                        result.Objects = new List<object>();
                        foreach (var item in query)
                        {
                            ML.AsignacionDocente asignacion = new ML.AsignacionDocente();
                            asignacion.IdAsignacionDocente = item.IdAsignacionDocente;
                            asignacion.DiaSemana = item.DiaSemana;
                            asignacion.HoraInicio = item.HoraInicio;
                            asignacion.HoraFin = item.HoraFin;

                            asignacion.Empleado = new ML.Empleado();
                            if (item.IdEmpleadoNavigation != null)
                            {
                                asignacion.Empleado.IdEmpleado = item.IdEmpleadoNavigation.IdEmpleado;
                                asignacion.Empleado.Nombre = item.IdEmpleadoNavigation.Nombre;
                                asignacion.Empleado.ApellidoPaterno = item.IdEmpleadoNavigation.ApellidoPaterno;
                                asignacion.Empleado.ApellidoMaterno = item.IdEmpleadoNavigation.ApellidoMaterno;
                            }

                            asignacion.Materia = new ML.Materia();
                            if (item.IdMateriaNavigation != null)
                            {
                                asignacion.Materia.IdMateria = item.IdMateriaNavigation.IdMateria;
                                asignacion.Materia.Nombre = item.IdMateriaNavigation.Nombre;
                                asignacion.Materia.Semestre = item.IdMateriaNavigation.Semestre;
                            }

                            asignacion.Grupo = new ML.Grupo();
                            if (item.IdGrupoNavigation != null)
                            {
                                asignacion.Grupo.IdGrupo = item.IdGrupoNavigation.IdGrupo;
                                asignacion.Grupo.Semestre = item.IdGrupoNavigation.Semestre;
                                asignacion.Grupo.Letra = item.IdGrupoNavigation.Letra;
                                asignacion.Grupo.Turno = item.IdGrupoNavigation.Turno;
                            }

                            result.Objects.Add(asignacion);
                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se encontraron asignaciones de docentes";
                    }
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

        public ML.Result Add(ML.AsignacionDocente asignacion)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.ApplicationDbContext context = new DL.ApplicationDbContext())
                {
                    // 1. Validar empalmes del Docente
                    var overlapTeacher = context.AsignacionDocentes
                        .Any(a => a.IdEmpleado == asignacion.Empleado.IdEmpleado
                                  && a.DiaSemana == asignacion.DiaSemana
                                  && a.HoraInicio < asignacion.HoraFin
                                  && a.HoraFin > asignacion.HoraInicio);

                    if (overlapTeacher)
                    {
                        result.Correct = false;
                        result.ErrorMessage = "El docente seleccionado ya tiene una materia asignada en este día y rango de horas (empalme).";
                        return result;
                    }

                    // 2. Validar empalmes del Grupo
                    var overlapGroup = context.AsignacionDocentes
                        .Any(a => a.IdGrupo == asignacion.Grupo.IdGrupo
                                  && a.DiaSemana == asignacion.DiaSemana
                                  && a.HoraInicio < asignacion.HoraFin
                                  && a.HoraFin > asignacion.HoraInicio);

                    if (overlapGroup)
                    {
                        result.Correct = false;
                        result.ErrorMessage = "El grupo seleccionado ya tiene otra clase asignada en este día y rango de horas (empalme).";
                        return result;
                    }

                    // 3. Si no hay empalmes, guardar asignación
                    DL.Models.AsignacionDocente dbAsignacion = new DL.Models.AsignacionDocente();
                    dbAsignacion.IdEmpleado = asignacion.Empleado.IdEmpleado ?? 0;
                    dbAsignacion.IdMateria = asignacion.Materia.IdMateria;
                    dbAsignacion.IdGrupo = asignacion.Grupo.IdGrupo ?? 0;
                    dbAsignacion.DiaSemana = asignacion.DiaSemana;
                    dbAsignacion.HoraInicio = asignacion.HoraInicio;
                    dbAsignacion.HoraFin = asignacion.HoraFin;

                    context.AsignacionDocentes.Add(dbAsignacion);
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
    }
}
