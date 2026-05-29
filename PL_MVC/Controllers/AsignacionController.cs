using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace PL_MVC.Controllers
{
    public class AsignacionController : Controller
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            BL.AsignacionDocente blAsignacion = new BL.AsignacionDocente();
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

            PopulateDropdowns();
            return View(model);
        }

        [HttpPost]
        public IActionResult AsignacionAdd(ML.AsignacionDocente model)
        {
            // Note: Since model binder might fail on complex nested objects, check and copy values if needed
            if (ModelState.IsValid)
            {
                BL.AsignacionDocente blAsignacion = new BL.AsignacionDocente();
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

            PopulateDropdowns();
            return View(model);
        }

        private void PopulateDropdowns()
        {
            // 1. Get Docentes (Empleados with Rol Profesor (IdRol = 2))
            BL.Empleado blEmpleado = new BL.Empleado();
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
            ViewBag.Docentes = docentes;

            // 2. Get Materias
            BL.Materia blMateria = new BL.Materia();
            ML.Result resultMaterias = blMateria.GetAll();
            ViewBag.Materias = resultMaterias.Correct ? resultMaterias.Objects : new List<object>();

            // 3. Get Grupos
            BL.Grupo blGrupo = new BL.Grupo();
            ML.Result resultGrupos = blGrupo.GrupoGetAll();
            ViewBag.Grupos = resultGrupos.Correct ? resultGrupos.Objects : new List<object>();
        }
    }
}
