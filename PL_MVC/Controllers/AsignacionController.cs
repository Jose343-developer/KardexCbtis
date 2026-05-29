using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace PL_MVC.Controllers
{
    public class AsignacionController : Controller
    {
        private readonly BL.AsignacionDocente _asignacionBL;
        private readonly BL.Empleado _empleadoBL;
        private readonly BL.Materia _materiaBL;
        private readonly BL.Grupo _grupoBL;

        public AsignacionController(
            BL.AsignacionDocente asignacionBL,
            BL.Empleado empleadoBL,
            BL.Materia materiaBL,
            BL.Grupo grupoBL)
        {
            _asignacionBL = asignacionBL;
            _empleadoBL = empleadoBL;
            _materiaBL = materiaBL;
            _grupoBL = grupoBL;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var blAsignacion = _asignacionBL;
            ML.Result result = blAsignacion.GetAll();
            
            ML.AsignacionDocente model = new ML.AsignacionDocente();
            if (result.Correct)
            {
                model.Asignaciones = result.Objects;
            }
            else
            {
                model.Asignaciones = new List<object>();
                ViewBag.Message = result.ErrorMessage;
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult AsignacionAdd()
        {
            ML.AsignacionDocente model = new ML.AsignacionDocente();
            model.Empleado = new ML.Empleado();
            model.Materia = new ML.Materia();
            model.Grupo = new ML.Grupo();

            PopulateDropdowns(model);
            return View(model);
        }

        [HttpPost]
        public IActionResult AsignacionAdd(ML.AsignacionDocente model)
        {
            if (ModelState.IsValid)
            {
                var blAsignacion = _asignacionBL;
                ML.Result result = blAsignacion.Add(model);

                if (result.Correct)
                {
                    TempData["SuccessMessage"] = "Asignación docente creada correctamente.";
                    return RedirectToAction("GetAll");
                }
                else
                {
                    ViewBag.ErrorMessage = result.ErrorMessage;
                }
            }

            PopulateDropdowns(model);
            return View(model);
        }

        private void PopulateDropdowns(ML.AsignacionDocente model)
        {
            // 1. Get Docentes (Empleados with Rol Profesor (IdRol = 2))
            var blEmpleado = _empleadoBL;
            ML.Result resultEmpleados = blEmpleado.GetAll();
            List<ML.Empleado> docentes = new List<ML.Empleado>();
            if (resultEmpleados.Correct)
            {
                foreach (ML.Empleado emp in resultEmpleados.Objects)
                {
                    if (emp.Usuario != null && emp.Usuario.Rol != null && emp.Usuario.Rol.IdRol == 2)
                    {
                        docentes.Add(emp);
                    }
                }
            }
            model.Docentes = docentes;

            // 2. Get Materias
            var blMateria = _materiaBL;
            ML.Result resultMaterias = blMateria.GetAll();
            model.Materias = resultMaterias.Correct ? resultMaterias.Objects.Cast<ML.Materia>().ToList() : new List<ML.Materia>();

            // 3. Get Grupos
            var blGrupo = _grupoBL;
            ML.Result resultGrupos = blGrupo.GrupoGetAll();
            model.Grupos = resultGrupos.Correct ? resultGrupos.Objects.Cast<ML.Grupo>().ToList() : new List<ML.Grupo>();
        }
    }
}
