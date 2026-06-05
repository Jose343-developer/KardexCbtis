using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace BL;

public class Grupo
{
    private readonly DL.ApplicationDbContext _context;

    public Grupo(DL.ApplicationDbContext context)
    {
        _context = context;
    }

public ML.Result GrupoGetAll()
    {
        ML.Result result = new ML.Result();

        try
        {
            var context = _context;
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
            var context = _context;
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
            var context = _context;
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

    public ML.Result Add(ML.Grupo grupo)
    {
        ML.Result result = new ML.Result();
        try
        {
            var dbGrupo = new DL.Models.Grupo
            {
                Semestre = (byte)(grupo.Semestre ?? 0),
                Letra = grupo.Letra ?? "",
                Turno = grupo.Turno ?? ""
            };
            _context.Grupos.Add(dbGrupo);
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

    public ML.Result Update(ML.Grupo grupo)
    {
        ML.Result result = new ML.Result();
        try
        {
            var dbGrupo = _context.Grupos.Find(grupo.IdGrupo);
            if (dbGrupo != null)
            {
                dbGrupo.Semestre = (byte)(grupo.Semestre ?? 0);
                dbGrupo.Letra = grupo.Letra ?? "";
                dbGrupo.Turno = grupo.Turno ?? "";
                _context.SaveChanges();
                result.Correct = true;
            }
            else
            {
                result.Correct = false;
                result.ErrorMessage = "No se encontró el grupo";
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

    public ML.Result Delete(int idGrupo)
    {
        ML.Result result = new ML.Result();
        try
        {
            var dbGrupo = _context.Grupos.Find(idGrupo);
            if (dbGrupo != null)
            {
                _context.Grupos.Remove(dbGrupo);
                _context.SaveChanges();
                result.Correct = true;
            }
            else
            {
                result.Correct = false;
                result.ErrorMessage = "No se encontró el grupo";
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

    public ML.Result GetById(int idGrupo)
    {
        ML.Result result = new ML.Result();
        try
        {
            var dbGrupo = _context.Grupos.Find(idGrupo);
            if (dbGrupo != null)
            {
                var mlGrupo = new ML.Grupo
                {
                    IdGrupo = dbGrupo.IdGrupo,
                    Semestre = dbGrupo.Semestre,
                    Letra = dbGrupo.Letra,
                    Turno = dbGrupo.Turno
                };
                result.Object = mlGrupo;
                result.Correct = true;
            }
            else
            {
                result.Correct = false;
                result.ErrorMessage = "No se encontró el grupo";
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
