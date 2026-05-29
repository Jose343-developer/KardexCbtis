using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace BL;

public class Grupo
{

public ML.Result GrupoGetAll()
    {
        ML.Result result = new ML.Result();

        try
        {
            using(DL.ApplicationDbContext context = new DL.ApplicationDbContext())
            {
                var query = context.GrupoGetAlls.FromSqlInterpolated($"EXEC GrupoGetAll").ToList();
                
                if (query.Count > 0)
                {
                    
                        result.Objects = new List<object>();
                        foreach (var item in query)
                    {
                        ML.Grupo grupoItem = new ML.Grupo();
                              grupoItem.IdGrupo = item.IdGrupo;
                              grupoItem.Semestre = item.Semestre;
                            grupoItem.Letra = item.Letra;
                                grupoItem.Turno = item.Turno;

        result.Objects.Add(grupoItem);


                    }

                        result.Correct = true;
                }
                else
                {
                    
                    result.Correct = false;
                    result.ErrorMessage = "Error al traer grupos";
                    

                }
            }


        }catch(Exception ex)
        {
            
                result.Correct = false;
                result.ErrorMessage =ex.Message;
                result.Ex = ex;


        }

return result;
    }
public ML.Result GetTurno()
    {
        ML.Result result = new ML.Result();
        try
        {
            using (DL.ApplicationDbContext context = new DL.ApplicationDbContext())
            {
                var query = context.GrupoGetTurnos.FromSqlInterpolated($"EXEC GrupoGetTurnos").ToList();

                if (query.Count > 0)
                {
                    result.Objects = new List<object>();
                    foreach (var item in query)
                    {
                        if (item.Turno != null)
                        {
                            result.Objects.Add(item.Turno);
                        }
                    }
                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "error al obtener turnos";
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

    public ML.Result GetSemestres()
    {
        ML.Result result = new ML.Result();
        try
        {
            using (DL.ApplicationDbContext context = new DL.ApplicationDbContext())
            {
                var query = context.GrupoGetSemestres.FromSqlInterpolated($"EXEC GrupoGetSemestres").ToList();

                if (query.Count > 0)
                {
                    result.Objects = new List<object>();
                    foreach (var item in query)
                    {
                        if (item.Semestre != null)
                        {
                            result.Objects.Add(item.Semestre);
                        }
                    }
                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "error al obtener semestres";
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
