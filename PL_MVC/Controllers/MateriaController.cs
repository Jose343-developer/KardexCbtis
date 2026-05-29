using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace PL_MVC.Controllers
{
    public class MateriaController : Controller
    {
        private readonly BL.Materia _materiaBL;
        private readonly BL.Especialidad _especialidadBL;

        public MateriaController(BL.Materia materiaBL, BL.Especialidad especialidadBL)
        {
            _materiaBL = materiaBL;
            _especialidadBL = especialidadBL;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            ML.Result result = _materiaBL.GetAll();
            List<ML.Materia> materiasList = new List<ML.Materia>();

            if (result.Correct)
            {
                foreach (var obj in result.Objects)
                {
                    materiasList.Add((ML.Materia)obj);
                }
            }
            else
            {
                ViewBag.Message = result.ErrorMessage;
            }

            return View(materiasList);
        }

        [HttpGet]
        public IActionResult MateriaAdd()
        {
            ML.Materia model = new ML.Materia();
            model.Especialidad = new ML.Especialidad();

            ML.Result resultEspecialidades = _especialidadBL.EspecialidadGetAll();
            if (resultEspecialidades.Correct)
            {
                model.Especialidad.Especialidades = resultEspecialidades.Objects;
            }
            else
            {
                model.Especialidad.Especialidades = new List<object>();
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult MateriaAdd(ML.Materia model)
        {
            if (ModelState.IsValid)
            {
                ML.Result result = _materiaBL.Add(model);

                if (result.Correct)
                {
                    TempData["SuccessMessage"] = "Materia agregada correctamente.";
                    return RedirectToAction("GetAll");
                }
                else
                {
                    ViewBag.ErrorMessage = result.ErrorMessage;
                }
            }

            // If we got here, something failed. Repopulate specialties and return the view.
            if (model.Especialidad == null)
            {
                model.Especialidad = new ML.Especialidad();
            }

            ML.Result resultEspecialidades = _especialidadBL.EspecialidadGetAll();
            if (resultEspecialidades.Correct)
            {
                model.Especialidad.Especialidades = resultEspecialidades.Objects;
            }
            else
            {
                model.Especialidad.Especialidades = new List<object>();
            }

            return View(model);
        }
    }
}
