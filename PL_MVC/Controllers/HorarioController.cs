using Microsoft.AspNetCore.Mvc;
using PL_MVC.Filters;

using System.Collections.Generic;

namespace PL_MVC.Controllers
{
    [AuthorizeRole("Administrador", "Profesor", "Alumno")]
    public class HorarioController : Controller
    {
        private readonly BL.AsignacionDocente _asignacionBL;

        public HorarioController(BL.AsignacionDocente asignacionBL)
        {
            _asignacionBL = asignacionBL;
        }

        public IActionResult Index()
        {
            var result = _asignacionBL.GetAll();
            List<ML.AsignacionDocente> asignaciones = new List<ML.AsignacionDocente>();
            if (result.Correct)
            {
                foreach (var item in result.Objects)
                {
                    asignaciones.Add((ML.AsignacionDocente)item);
                }
            }
            return View(asignaciones);
        }
    }
}
