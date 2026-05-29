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
namespace PL_MVC.Controllers
{
    public class CalendarioController : Controller
{
    private readonly IConfiguration _config;

    public CalendarioController (IConfiguration config)
        {
            

            _config = config;
        }
        [HttpGet]

        public async Task<IActionResult> GetAllCalendarView()
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

                    List<ML.CalendarObjects> objects = new List<ML.CalendarObjects>();
                    var events = await service.Events.List(correo).ExecuteAsync();
                    if (events.Items != null)
            {
                
                    foreach(var item in events.Items)
                {
                    
                        ML.CalendarObjects ObjetoCalendario =  new ML.CalendarObjects();

                        ObjetoCalendario.summary = item.Summary;
                         ObjetoCalendario.id = item.Id;
                         ObjetoCalendario.description = item.Description;
                         ObjetoCalendario.status = item.Status;


                        if (item.Start != null)
                    {
                        
                            ObjetoCalendario.start = new ML.Start()
                            {
                                
                            dateTime = item.Start.DateTime,
                              date = item.Start.Date,
                           timeZone = item.Start.TimeZone


                            };



                    } if (item.End != null)
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