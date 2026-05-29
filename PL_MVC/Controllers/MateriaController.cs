using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using PL_MVC.Filters;

namespace PL_MVC.Controllers
{
    [AuthorizeRole("Administrador")]
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

        [HttpGet]
        public IActionResult MateriaEdit(int idMateria)
        {
            ML.Result result = _materiaBL.GetById(idMateria);
            if (result.Correct)
            {
                ML.Materia model = (ML.Materia)result.Object;

                ML.Result resultEspecialidades = _especialidadBL.EspecialidadGetAll();
                if (model.Especialidad == null)
                {
                    model.Especialidad = new ML.Especialidad();
                }
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
            else
            {
                TempData["ErrorMessage"] = "No se pudo obtener los datos de la materia: " + result.ErrorMessage;
                return RedirectToAction("GetAll");
            }
        }

        [HttpPost]
        public IActionResult MateriaEdit(ML.Materia model)
        {
            if (ModelState.IsValid)
            {
                ML.Result result = _materiaBL.Update(model);
                if (result.Correct)
                {
                    TempData["SuccessMessage"] = "Materia actualizada correctamente.";
                    return RedirectToAction("GetAll");
                }
                else
                {
                    ViewBag.ErrorMessage = result.ErrorMessage;
                }
            }

            // Repopulate specialties dropdown
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

        [HttpGet]
        public IActionResult MateriaDelete(int idMateria)
        {
            ML.Result result = _materiaBL.Delete(idMateria);
            if (result.Correct)
            {
                TempData["SuccessMessage"] = "Materia eliminada correctamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "No se pudo eliminar la materia: " + result.ErrorMessage;
            }
            return RedirectToAction("GetAll");
        }
    }
}
