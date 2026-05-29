using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BL
{
    public class Materia
    {
        public ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.ApplicationDbContext context = new DL.ApplicationDbContext())
                {
                    var query = context.Materias
                        .Include(m => m.IdEspecialidadNavigation)
                        .ToList();

                    if (query != null && query.Count > 0)
                    {
                        result.Objects = new List<object>();
                        foreach (var item in query)
                        {
                            ML.Materia materia = new ML.Materia();
                            materia.IdMateria = item.IdMateria;
                            materia.Nombre = item.Nombre;
                            materia.Semestre = item.Semestre;
                            materia.Creditos = item.Creditos;

                            materia.Especialidad = new ML.Especialidad();
                            if (item.IdEspecialidadNavigation != null)
                            {
                                materia.Especialidad.IdEspecialidad = item.IdEspecialidadNavigation.IdEspecialidad;
                                materia.Especialidad.Nombre = item.IdEspecialidadNavigation.Nombre;
                                materia.Especialidad.ClaveOficial = item.IdEspecialidadNavigation.ClaveOficial;
                            }

                            result.Objects.Add(materia);
                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se encontraron materias";
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

        public ML.Result Add(ML.Materia materia)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.ApplicationDbContext context = new DL.ApplicationDbContext())
                {
                    DL.Models.Materia dbMateria = new DL.Models.Materia();
                    dbMateria.Nombre = materia.Nombre;
                    dbMateria.Semestre = materia.Semestre;
                    dbMateria.Creditos = materia.Creditos;
                    
                    if (materia.Especialidad != null && materia.Especialidad.IdEspecialidad > 0)
                    {
                        dbMateria.IdEspecialidad = materia.Especialidad.IdEspecialidad;
                    }
                    else
                    {
                        dbMateria.IdEspecialidad = null;
                    }

                    context.Materias.Add(dbMateria);
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
