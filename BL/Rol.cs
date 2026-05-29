using System;
using System.Collections.Generic;
using System.Linq;

namespace BL
{
    public class Rol
    {
        public ML.Result RolGetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.ApplicationDbContext context = new DL.ApplicationDbContext())
                {
                    var query = context.Rols.ToList();
                    if (query != null && query.Count > 0)
                    {
                        result.Objects = new List<object>();
                        foreach (var item in query)
                        {
                            ML.Rol rol = new ML.Rol();
                            rol.IdRol = item.IdRol;
                            rol.Nombre = item.Nombre;
                            result.Objects.Add(rol);
                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se encontraron roles";
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
    }
}
