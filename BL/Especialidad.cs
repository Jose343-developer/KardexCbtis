using System;
using System.Security.AccessControl;
using Microsoft.EntityFrameworkCore;

namespace BL;

public class Especialidad
{


public ML.Result EspecialidadGetAll ()
    {
        
ML.Result result = new ML.Result();

        try
        {
            
            using(DL.ApplicationDbContext context = new DL.ApplicationDbContext())
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

}
