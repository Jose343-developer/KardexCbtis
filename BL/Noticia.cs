using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BL
{
    public class Noticia
    {
        public static ML.Result Add(ML.Noticia noticia)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.ApplicationDbContext context = new DL.ApplicationDbContext())
                {
                    DL.Models.Noticia newNoticia = new DL.Models.Noticia
                    {
                        Titulo = noticia.Titulo,
                        Contenido = noticia.Contenido,
                        ImagenBase64 = noticia.ImagenBase64,
                        FechaPublicacion = DateTime.Now,
                        Estatus = true
                    };

                    context.Noticias.Add(newNoticia);
                    context.SaveChanges();

                    result.Correct = true;
                    result.ErrorMessage = "Noticia agregada correctamente";
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.ApplicationDbContext context = new DL.ApplicationDbContext())
                {
                    var noticias = context.Noticias
                        .Where(n => n.Estatus == true)
                        .OrderByDescending(n => n.FechaPublicacion)
                        .ToList();

                    result.Objects = new List<object>();

                    if (noticias != null && noticias.Count > 0)
                    {
                        foreach (var item in noticias)
                        {
                            ML.Noticia noticia = new ML.Noticia
                            {
                                IdNoticia = item.IdNoticia,
                                Titulo = item.Titulo,
                                Contenido = item.Contenido,
                                ImagenBase64 = item.ImagenBase64,
                                FechaPublicacion = item.FechaPublicacion,
                                Estatus = item.Estatus
                            };
                            result.Objects.Add(noticia);
                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No hay noticias registradas.";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public static ML.Result GetRelevante()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.ApplicationDbContext context = new DL.ApplicationDbContext())
                {
                    var noticia = context.Noticias
                        .Where(n => n.Estatus == true)
                        .OrderByDescending(n => n.FechaPublicacion)
                        .FirstOrDefault();

                    if (noticia != null)
                    {
                        ML.Noticia objNoticia = new ML.Noticia
                        {
                            IdNoticia = noticia.IdNoticia,
                            Titulo = noticia.Titulo,
                            Contenido = noticia.Contenido,
                            ImagenBase64 = noticia.ImagenBase64,
                            FechaPublicacion = noticia.FechaPublicacion,
                            Estatus = noticia.Estatus
                        };
                        result.Object = objNoticia;
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No hay noticias recientes.";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public static ML.Result Delete(int idNoticia)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.ApplicationDbContext context = new DL.ApplicationDbContext())
                {
                    var noticia = context.Noticias.Find(idNoticia);
                    if (noticia != null)
                    {
                        context.Noticias.Remove(noticia);
                        context.SaveChanges();
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "La noticia no existe.";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }
    }
}
