using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PL_MVC.Filters;

namespace PL_MVC.Controllers
{
    [AuthorizeRole("Administrador", "Profesor")]
    public class CalendarioController : Controller
    {
        private readonly BL.Evento _eventoBL;
        private readonly BL.AsignacionDocente _asignacionDocenteBL;

        private static readonly Dictionary<string, DayOfWeek> SpanishDays = new(StringComparer.OrdinalIgnoreCase)
        {
            { "Lunes", DayOfWeek.Monday },
            { "Martes", DayOfWeek.Tuesday },
            { "Miércoles", DayOfWeek.Wednesday },
            { "Miercoles", DayOfWeek.Wednesday },
            { "Jueves", DayOfWeek.Thursday },
            { "Viernes", DayOfWeek.Friday },
            { "Sábado", DayOfWeek.Saturday },
            { "Sabado", DayOfWeek.Saturday },
            { "Domingo", DayOfWeek.Sunday }
        };

        public CalendarioController(BL.Evento eventoBL, BL.AsignacionDocente asignacionDocenteBL)
        {
            _eventoBL = eventoBL;
            _asignacionDocenteBL = asignacionDocenteBL;
        }

        [HttpGet]
        public IActionResult GetAllCalendarView()
        {
            List<ML.CalendarObjects> objects = new List<ML.CalendarObjects>();

            // 1. Obtener eventos locales de la Base de Datos
            try
            {
                ML.Result resultEventos = _eventoBL.GetAll();
                if (resultEventos.Correct && resultEventos.Objects != null)
                {
                    foreach (ML.Evento item in resultEventos.Objects)
                    {
                        ML.CalendarObjects ObjetoCalendario = new ML.CalendarObjects
                        {
                            id = item.IdEvento.ToString(),
                            summary = item.Titulo,
                            description = item.Descripcion,
                            status = item.Estatus ?? "confirmed",
                            location = item.Ubicacion,
                            start = new ML.Start { dateTime = item.FechaInicio },
                            end = new ML.End { dateTime = item.FechaFin }
                        };
                        objects.Add(ObjetoCalendario);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[Database Events Error] Failed to fetch events: {ex.Message}");
                ViewBag.CalendarError = "No se pudieron cargar los eventos del sistema: " + ex.Message;
            }

            // 2. Obtener y proyectar horarios de clases (Asignaciones Docente)
            try
            {
                ML.Result resultAsignaciones = _asignacionDocenteBL.GetAll();
                if (resultAsignaciones.Correct && resultAsignaciones.Objects != null)
                {
                    var startDate = DateTime.Today.AddDays(-30);
                    var endDate = DateTime.Today.AddDays(60);

                    foreach (ML.AsignacionDocente asignacion in resultAsignaciones.Objects)
                    {
                        if (string.IsNullOrEmpty(asignacion.DiaSemana) || !SpanishDays.TryGetValue(asignacion.DiaSemana, out var targetDayOfWeek))
                            continue;

                        for (var date = startDate; date <= endDate; date = date.AddDays(1))
                        {
                            if (date.DayOfWeek == targetDayOfWeek)
                            {
                                var startDateTime = date.Date.Add(asignacion.HoraInicio);
                                var endDateTime = date.Date.Add(asignacion.HoraFin);

                                var classEvent = new ML.CalendarObjects
                                {
                                    id = $"class-{asignacion.IdAsignacionDocente}-{date:yyyyMMdd}",
                                    summary = $"Clase: {asignacion.Materia?.Nombre}",
                                    description = $"Docente: {asignacion.Empleado?.Nombre} {asignacion.Empleado?.ApellidoPaterno}\nGrupo: {asignacion.Grupo?.Semestre} - {asignacion.Grupo?.Letra} ({asignacion.Grupo?.Turno})",
                                    status = "class-schedule",
                                    location = $"Grupo {asignacion.Grupo?.Semestre} {asignacion.Grupo?.Letra} ({asignacion.Grupo?.Turno})",
                                    start = new ML.Start { dateTime = startDateTime },
                                    end = new ML.End { dateTime = endDateTime }
                                };
                                objects.Add(classEvent);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[Class Schedules Error] Failed to fetch and project classes: {ex.Message}");
                ViewBag.AsignacionError = "No se pudieron cargar los horarios de clases: " + ex.Message;
            }

            return View(objects);
        }

        [HttpPost]
        public JsonResult AddEvent([FromBody] ML.Evento model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { correct = false, errorMessage = string.Join(" ", errors) });
            }

            var result = _eventoBL.Add(model);
            if (result.Correct)
            {
                return Json(new { correct = true, idEvento = result.Object });
            }
            else
            {
                return Json(new { correct = false, errorMessage = result.ErrorMessage });
            }
        }

        [HttpPost]
        public JsonResult DeleteEvent(int idEvento)
        {
            var result = _eventoBL.Delete(idEvento);
            if (result.Correct)
            {
                return Json(new { correct = true });
            }
            else
            {
                return Json(new { correct = false, errorMessage = result.ErrorMessage });
            }
        }

        [HttpGet]
        public JsonResult GetAllCalendarApi()
        {
            var result = _eventoBL.GetAll();
            if (result.Correct)
            {
                return Json(result.Objects);
            }
            return Json(new List<object>());
        }
    }
}