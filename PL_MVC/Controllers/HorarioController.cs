using Microsoft.AspNetCore.Mvc;
using PL_MVC.Filters;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace PL_MVC.Controllers
{
    [AuthorizeRole("Administrador", "Alumno")]
    public class HorarioController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly BL.Grupo _grupoBL;
        private readonly BL.Alumno _alumnoBL;
        private readonly BL.Horario _horarioBL;

        public HorarioController(IWebHostEnvironment env, BL.Grupo grupoBL, BL.Alumno alumnoBL, BL.Horario horarioBL)
        {
            _env = env;
            _grupoBL = grupoBL;
            _alumnoBL = alumnoBL;
            _horarioBL = horarioBL;
        }

        [AuthorizeRole("Administrador")]
        public IActionResult Index()
        {
            var resultGrupos = _grupoBL.GrupoGetAll();
            ML.Horario model = new ML.Horario();
            if (resultGrupos.Correct)
            {
                model.Grupos = resultGrupos.Objects;
            }
            return View(model);
        }

        [HttpPost]
        [AuthorizeRole("Administrador")]
        public async Task<IActionResult> Upload(int IdGrupo, IFormFile Documento)
        {
            if (IdGrupo <= 0 || Documento == null || Documento.Length == 0)
            {
                TempData["ErrorMessage"] = "Seleccione un grupo y un archivo PDF.";
                return RedirectToAction("Index");
            }

            if (Documento.ContentType != "application/pdf")
            {
                TempData["ErrorMessage"] = "El archivo debe ser un PDF.";
                return RedirectToAction("Index");
            }

            string schedulesPath = Path.Combine(_env.WebRootPath, "Schedules");
            if (!Directory.Exists(schedulesPath))
            {
                Directory.CreateDirectory(schedulesPath);
            }

            string fileName = $"horario_grupo_{IdGrupo}.pdf";
            string filePath = Path.Combine(schedulesPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await Documento.CopyToAsync(stream);
            }

            ML.Horario horario = new ML.Horario
            {
                IdGrupo = IdGrupo,
                DocumentoRuta = $"/Schedules/{fileName}"
            };

            var result = _horarioBL.AddOrUpdate(horario);
            if (result.Correct)
            {
                TempData["SuccessMessage"] = result.ErrorMessage;
            }
            else
            {
                TempData["ErrorMessage"] = result.ErrorMessage;
            }

            return RedirectToAction("Index");
        }

        [AuthorizeRole("Alumno")]
        public async Task<IActionResult> MiHorario()
        {
            var idUsuarioStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(idUsuarioStr, out int idUsuario))
            {
                var resultAlumno = _alumnoBL.GetByIdUsuario(idUsuario);
                if (resultAlumno.Correct)
                {
                    var alumno = (ML.Alumno)resultAlumno.Object;
                    if (alumno.Grupo != null && alumno.Grupo.IdGrupo > 0)
                    {
                        var resultHorario = _horarioBL.GetByGrupo(alumno.Grupo.IdGrupo ?? 0);
                        if (resultHorario.Correct)
                        {
                            var horario = (ML.Horario)resultHorario.Object;
                            return View(horario); // Enviar a la vista VerHorario.cshtml
                        }
                        else
                        {
                            return View("NotFound");
                        }
                    }
                }
            }
            
            return View("NotFound");
        }
    }
}
