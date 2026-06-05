using System;
using System.Security.AccessControl;
using Microsoft.EntityFrameworkCore;

namespace BL;

public class Especialidad
{
    private readonly DL.ApplicationDbContext _context;

    public Especialidad(DL.ApplicationDbContext context)
    {
        _context = context;
    }

public ML.Result EspecialidadGetAll ()
    {
        
ML.Result result = new ML.Result();

        try
        {
            
            var context = _context;
            {
                
                var query = context.EspecialidadGetAlls.FromSqlInterpolated($"EXEC EspecialidadGetAll").ToList();
                if (query != null)
                {
                    
                    result.Objects = new List<object>();
                    foreach (var item in query)
                    {
                        ML.Especialidad especialidad = new ML.Especialidad();
                           especialidad.IdEspecialidad = item.IdEspecialidad;
                           especialidad.Nombre = item.Nombre;
                           especialidad.ClaveOficial = item.ClaveOficial;

                           result.Objects.Add(especialidad);




                    }
                    result.Correct = true;
                }
                else
                {
                    
                    result.Correct = false;
                    result.ErrorMessage = "ERROR AL OBTENER ESPECIALDIADES";
                    
                }


            }


        }catch (Exception ex)
        {
            result.Correct = false;
            result.ErrorMessage = ex.Message;
            result.Ex = ex;



        }

return result;

    }

    public ML.Result Add(ML.Especialidad especialidad)
    {
        ML.Result result = new ML.Result();
        try
        {
            var dbEspecialidad = new DL.Models.Especialidade
            {
                Nombre = especialidad.Nombre ?? "",
                ClaveOficial = especialidad.ClaveOficial ?? ""
            };
            _context.Especialidades.Add(dbEspecialidad);
            _context.SaveChanges();
            result.Correct = true;
        }
        catch (Exception ex)
        {
            result.Correct = false;
            result.ErrorMessage = ex.Message;
            result.Ex = ex;
        }
        return result;
    }

    public ML.Result Update(ML.Especialidad especialidad)
    {
        ML.Result result = new ML.Result();
        try
        {
            var dbEspecialidad = _context.Especialidades.Find(especialidad.IdEspecialidad);
            if (dbEspecialidad != null)
            {
                dbEspecialidad.Nombre = especialidad.Nombre ?? "";
                dbEspecialidad.ClaveOficial = especialidad.ClaveOficial ?? "";
                _context.SaveChanges();
                result.Correct = true;
            }
            else
            {
                result.Correct = false;
                result.ErrorMessage = "No se encontró la especialidad";
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

    public ML.Result Delete(int idEspecialidad)
    {
        ML.Result result = new ML.Result();
        try
        {
            var dbEspecialidad = _context.Especialidades.Find(idEspecialidad);
            if (dbEspecialidad != null)
            {
                _context.Especialidades.Remove(dbEspecialidad);
                _context.SaveChanges();
                result.Correct = true;
            }
            else
            {
                result.Correct = false;
                result.ErrorMessage = "No se encontró la especialidad";
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

    public ML.Result GetById(int idEspecialidad)
    {
        ML.Result result = new ML.Result();
        try
        {
            var dbEspecialidad = _context.Especialidades.Find(idEspecialidad);
            if (dbEspecialidad != null)
            {
                var mlEspecialidad = new ML.Especialidad
                {
                    IdEspecialidad = dbEspecialidad.IdEspecialidad,
                    Nombre = dbEspecialidad.Nombre,
                    ClaveOficial = dbEspecialidad.ClaveOficial
                };
                result.Object = mlEspecialidad;
                result.Correct = true;
            }
            else
            {
                result.Correct = false;
                result.ErrorMessage = "No se encontró la especialidad";
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
