using System;
using System.Collections.Generic;
using System.Linq;

namespace BL
{
    public class Evento
    {
        private readonly DL.ApplicationDbContext _context;

        public Evento(DL.ApplicationDbContext context)
        {
            _context = context;
        }

        public ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                var query = _context.Eventos.ToList();
                result.Objects = new List<object>();

                foreach (var item in query)
                {
                    ML.Evento ev = new ML.Evento
                    {
                        IdEvento = item.IdEvento,
                        Titulo = item.Titulo,
                        Descripcion = item.Descripcion,
                        FechaInicio = item.FechaInicio,
                        FechaFin = item.FechaFin,
                        Estatus = item.Estatus,
                        Ubicacion = item.Ubicacion
                    };
                    result.Objects.Add(ev);
                }
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

        public ML.Result Add(ML.Evento model)
        {
            ML.Result result = new ML.Result();
            try
            {
                DL.Models.Evento entity = new DL.Models.Evento
                {
                    Titulo = model.Titulo,
                    Descripcion = model.Descripcion,
                    FechaInicio = model.FechaInicio,
                    FechaFin = model.FechaFin,
                    Estatus = model.Estatus,
                    Ubicacion = model.Ubicacion
                };

                _context.Eventos.Add(entity);
                _context.SaveChanges();

                result.Correct = true;
                result.Object = entity.IdEvento;
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }

        public ML.Result Delete(int idEvento)
        {
            ML.Result result = new ML.Result();
            try
            {
                var entity = _context.Eventos.Find(idEvento);
                if (entity != null)
                {
                    _context.Eventos.Remove(entity);
                    _context.SaveChanges();
                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "El evento no existe.";
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
