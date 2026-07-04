using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BL
{
    public class Horario
    {
        private readonly DL.ApplicationDbContext _context;

        public Horario(DL.ApplicationDbContext context)
        {
            _context = context;
        }

        public ML.Result AddOrUpdate(ML.Horario horario)
        {
            ML.Result result = new ML.Result();
            try
            {
                // Check if a schedule already exists for this group
                var existing = _context.Horarios.FirstOrDefault(h => h.IdGrupo == horario.IdGrupo);

                if (existing != null)
                {
                    existing.DocumentoRuta = horario.DocumentoRuta ?? "";
                    _context.SaveChanges();
                    result.Correct = true;
                    result.ErrorMessage = "Horario actualizado correctamente.";
                }
                else
                {
                    var entity = new DL.Models.Horario
                    {
                        IdGrupo = horario.IdGrupo,
                        DocumentoRuta = horario.DocumentoRuta ?? ""
                    };
                    _context.Horarios.Add(entity);
                    _context.SaveChanges();
                    result.Correct = true;
                    result.ErrorMessage = "Horario asignado correctamente.";
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

        public ML.Result GetByGrupo(int idGrupo)
        {
            ML.Result result = new ML.Result();
            try
            {
                var query = _context.Horarios
                                    .Include(h => h.Grupo)
                                    .Where(h => h.IdGrupo == idGrupo)
                                    .Select(h => new ML.Horario
                                    {
                                        IdHorario = h.IdHorario,
                                        IdGrupo = h.IdGrupo,
                                        DocumentoRuta = h.DocumentoRuta,
                                        Grupo = h.Grupo != null ? new ML.Grupo
                                        {
                                            IdGrupo = h.Grupo.IdGrupo,
                                            Semestre = h.Grupo.Semestre,
                                            Letra = h.Grupo.Letra,
                                            Turno = h.Grupo.Turno
                                        } : null
                                    }).FirstOrDefault();

                if (query != null)
                {
                    result.Object = query;
                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "No se encontró horario para el grupo seleccionado.";
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

        public ML.Result Delete(int idHorario)
        {
            ML.Result result = new ML.Result();
            try
            {
                var entity = _context.Horarios.Find(idHorario);
                if (entity != null)
                {
                    _context.Horarios.Remove(entity);
                    _context.SaveChanges();
                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "El horario no existe.";
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
