using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BL
{
    public class Materia
    {
        private readonly DL.ApplicationDbContext _context;

        public Materia(DL.ApplicationDbContext context)
        {
            _context = context;
        }

        public ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
            var context = _context;
            {
                    var query = context.MateriaGetAlls.FromSqlInterpolated($"EXEC MateriaGetAll").ToList();

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
                            materia.Especialidad.IdEspecialidad = item.IdEspecialidad ?? 0;
                            materia.Especialidad.Nombre = item.NombreEspecialidad ?? "Sin especialidad";
                            materia.Especialidad.ClaveOficial = item.ClaveOficial ?? "sin clave";

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
            var context = _context;
            {
                    int? idEspecialidad = (materia.Especialidad != null && materia.Especialidad.IdEspecialidad > 0) 
                        ? materia.Especialidad.IdEspecialidad 
                        : null;

                    var rowsAffected = context.Database.ExecuteSqlInterpolated($@"EXEC MateriaAdd 
                        {materia.Nombre}, 
                        {materia.Semestre}, 
                        {materia.Creditos}, 
                        {idEspecialidad}");

                    if (rowsAffected > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se pudo insertar la materia.";
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

        public ML.Result GetById(int idMateria)
        {
            ML.Result result = new ML.Result();
            try
            {
                var query = _context.MateriaGetAlls.FromSqlInterpolated($"EXEC MateriaGetById {idMateria}").AsEnumerable().FirstOrDefault();

                if (query != null)
                {
                    ML.Materia materia = new ML.Materia();
                    materia.IdMateria = query.IdMateria;
                    materia.Nombre = query.Nombre;
                    materia.Semestre = query.Semestre;
                    materia.Creditos = query.Creditos;

                    materia.Especialidad = new ML.Especialidad();
                    materia.Especialidad.IdEspecialidad = query.IdEspecialidad ?? 0;
                    materia.Especialidad.Nombre = query.NombreEspecialidad ?? "Sin especialidad";
                    materia.Especialidad.ClaveOficial = query.ClaveOficial ?? "sin clave";

                    result.Object = materia;
                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "No se encontró la materia";
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

        public ML.Result Update(ML.Materia materia)
        {
            ML.Result result = new ML.Result();
            try
            {
                int? idEspecialidad = (materia.Especialidad != null && materia.Especialidad.IdEspecialidad > 0) 
                    ? materia.Especialidad.IdEspecialidad 
                    : null;

                var rowsAffected = _context.Database.ExecuteSqlInterpolated($@"EXEC MateriaUpdate 
                    {materia.IdMateria},
                    {materia.Nombre}, 
                    {materia.Semestre}, 
                    {materia.Creditos}, 
                    {idEspecialidad}");

                 if (rowsAffected > 0 || rowsAffected == -1)
                {
                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "No se pudo actualizar la materia.";
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

        public ML.Result Delete(int idMateria)
        {
            ML.Result result = new ML.Result();
            try
            {
                var rowsAffected = _context.Database.ExecuteSqlInterpolated($"EXEC MateriaDelete {idMateria}");

                if (rowsAffected > 0 || rowsAffected == -1)
                {
                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "No se pudo eliminar la materia.";
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
