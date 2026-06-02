using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System.Data;
using System.IO;
using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Newtonsoft.Json;
using PL_MVC.Filters;

namespace PL_MVC.Controllers
{
    [AuthorizeRole("Administrador", "Profesor")]
    public class CalendarioController : Controller
    {
        private readonly IConfiguration _config;
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

        public CalendarioController(IConfiguration config, BL.AsignacionDocente asignacionDocenteBL)
        {
            _config = config;
            _asignacionDocenteBL = asignacionDocenteBL;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCalendarView()
        {
            List<ML.CalendarObjects> objects = new List<ML.CalendarObjects>();

            // 1. Obtener eventos de Google Calendar
            try
            {
                string ruta = _config.GetValue<string>("GoogleCalendar:JsonPath");
                string correo = _config.GetValue<string>("GoogleCalendar:CalendarId");
                
                var credencial = Google.Apis.Auth.OAuth2.GoogleCredential.FromFile(ruta)
                     .CreateScoped(Google.Apis.Calendar.v3.CalendarService.Scope.CalendarReadonly);

                var service = new Google.Apis.Calendar.v3.CalendarService(new Google.Apis.Services.BaseClientService.Initializer()
                {
                    HttpClientInitializer = credencial,
                    ApplicationName = "prueba postman"
                });

                var events = await service.Events.List(correo).ExecuteAsync();
                if (events.Items != null)
                {
                    foreach (var item in events.Items)
                    {
                        ML.CalendarObjects ObjetoCalendario = new ML.CalendarObjects();
                        ObjetoCalendario.summary = item.Summary;
                        ObjetoCalendario.id = item.Id;
                        ObjetoCalendario.description = item.Description;
                        ObjetoCalendario.status = item.Status ?? "confirmed";
                        ObjetoCalendario.location = item.Location;
                        ObjetoCalendario.htmlLink = item.HtmlLink;

                        if (item.Start != null)
                        {
                            ObjetoCalendario.start = new ML.Start()
                            {
                                dateTime = item.Start.DateTime,
                                date = item.Start.Date,
                                timeZone = item.Start.TimeZone
                            };
                        }
                        if (item.End != null)
                        {
                            ObjetoCalendario.end = new ML.End()
                            {
                                dateTime = item.End.DateTime,
                                date = item.End.Date,
                                timeZone = item.End.TimeZone
                            };
                        }

                        objects.Add(ObjetoCalendario);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[Google Calendar Error] Failed to fetch events: {ex.Message}");
                ViewBag.CalendarError = "No se pudieron cargar los eventos del Calendario de Google: " + ex.Message;
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


        [HttpGet]
        public async Task <IActionResult> GetAllCalendarApi()
        {
            //ruta de las credenciales del perfil administradorr
                string ruta = _config.GetValue<string>("GoogleCalendar:JsonPath");
                /// tecnicamente no es unc orreo pero tiene la estructura solo es el id del calendario de mi perfil
                string correo = _config.GetValue<string>("GoogleCalendar:CalendarId");
                // credencial para identificarnos similar a jwt
                var credencial = Google.Apis.Auth.OAuth2.GoogleCredential.FromFile(ruta)
                     .CreateScoped(Google.Apis.Calendar.v3.CalendarService.Scope.CalendarReadonly);

                    //creamos el servico

                    var service = new Google.Apis.Calendar.v3.CalendarService(new Google.Apis.Services.BaseClientService.Initializer()
                    {
                        

                            HttpClientInitializer = credencial,
                            ApplicationName = "prueba postman"


                    });


                    var events = await service.Events.List(correo).ExecuteAsync();

                    if (events.Items == null)
            {
                
            

                Response.StatusCode = 400;
            }
           
              ViewBag.ItemsCalendarGet = events.Items;
            return Ok(events.Items);
        }
}
}