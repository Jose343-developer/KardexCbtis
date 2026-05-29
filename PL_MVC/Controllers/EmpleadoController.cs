using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using PL_MVC.Filters;

namespace PL_MVC.Controllers
{
    [AuthorizeRole("Administrador")]
    public class EmpleadoController : Controller
    {
        private readonly BL.Empleado _empleadoBL;
        private readonly BL.Rol _rolBL;

        public EmpleadoController(BL.Empleado empleadoBL, BL.Rol rolBL)
        {
            _empleadoBL = empleadoBL;
            _rolBL = rolBL;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var empleadoBL = _empleadoBL;
            ML.Result result = empleadoBL.GetAll();

            List<ML.Empleado> lista = new List<ML.Empleado>();
            if (result.Correct)
            {
                foreach (var obj in result.Objects)
                {
                    lista.Add((ML.Empleado)obj);
                }
            }
            else
            {
                ViewBag.Message = result.ErrorMessage;
            }

            return View(lista);
        }

        [HttpGet]
        public ActionResult EmpleadoAdd()
        {
            ML.Empleado empleado = new ML.Empleado();
            empleado.Usuario = new ML.Usuario();
            empleado.Usuario.Rol = new ML.Rol();

            var rolBL = _rolBL;
            ML.Result resultRoles = rolBL.RolGetAll();

            if (resultRoles.Correct)
            {
                empleado.Usuario.Rol.Roles = resultRoles.Objects;
            }
            else
            {
                empleado.Usuario.Rol.Roles = new List<object>();
            }

            return View(empleado);
        }

        [HttpPost]
        public ActionResult EmpleadoAdd(ML.Empleado empleado)
        {
            if (!ModelState.IsValid)
            {
                var rolBL = _rolBL;
                ML.Result resultRoles = rolBL.RolGetAll();

                if (empleado.Usuario == null) empleado.Usuario = new ML.Usuario();
                if (empleado.Usuario.Rol == null) empleado.Usuario.Rol = new ML.Rol();

                if (resultRoles.Correct)
                {
                    empleado.Usuario.Rol.Roles = resultRoles.Objects;
                }
                else
                {
                    empleado.Usuario.Rol.Roles = new List<object>();
                }

                return View(empleado);
            }

            var empleadoBL = _empleadoBL;
            ML.Result result = empleadoBL.Add(empleado);

            if (result.Correct)
            {
                TempData["Message"] = "Empleado añadido correctamente";
                return RedirectToAction("GetAll");
            }
            else
            {
                var rolBL = _rolBL;
                ML.Result resultRoles = rolBL.RolGetAll();

                if (empleado.Usuario == null) empleado.Usuario = new ML.Usuario();
                if (empleado.Usuario.Rol == null) empleado.Usuario.Rol = new ML.Rol();

                if (resultRoles.Correct)
                {
                    empleado.Usuario.Rol.Roles = resultRoles.Objects;
                }
                else
                {
                    empleado.Usuario.Rol.Roles = new List<object>();
                }

                ViewBag.Message = "Error al insertar Empleado: " + result.ErrorMessage;
                return View(empleado);
            }
        }

        [HttpGet]
        public ActionResult EmpleadoEdit(int idEmpleado)
        {
            ML.Result result = _empleadoBL.GetById(idEmpleado);
            if (result.Correct)
            {
                ML.Empleado model = (ML.Empleado)result.Object;
                ML.Result resultRoles = _rolBL.RolGetAll();
                if (model.Usuario == null) model.Usuario = new ML.Usuario();
                if (model.Usuario.Rol == null) model.Usuario.Rol = new ML.Rol();

                if (resultRoles.Correct)
                {
                    model.Usuario.Rol.Roles = resultRoles.Objects;
                }
                else
                {
                    model.Usuario.Rol.Roles = new List<object>();
                }
                return View(model);
            }
            else
            {
                TempData["Message"] = "Error al obtener datos del empleado: " + result.ErrorMessage;
                return RedirectToAction("GetAll");
            }
        }

        [HttpPost]
        public ActionResult EmpleadoEdit(ML.Empleado empleado)
        {
            if (string.IsNullOrEmpty(empleado.Usuario?.Password))
            {
                ModelState.Remove("Usuario.Password");
                // Obtener contraseña anterior para no perderla
                ML.Result existingResult = _empleadoBL.GetById(empleado.IdEmpleado.Value);
                if (existingResult.Correct)
                {
                    ML.Empleado existing = (ML.Empleado)existingResult.Object;
                    if (empleado.Usuario == null) empleado.Usuario = new ML.Usuario();
                    empleado.Usuario.Password = existing.Usuario.Password;
                }
            }

            if (!ModelState.IsValid)
            {
                ML.Result resultRoles = _rolBL.RolGetAll();
                if (empleado.Usuario == null) empleado.Usuario = new ML.Usuario();
                if (empleado.Usuario.Rol == null) empleado.Usuario.Rol = new ML.Rol();

                if (resultRoles.Correct)
                {
                    empleado.Usuario.Rol.Roles = resultRoles.Objects;
                }
                else
                {
                    empleado.Usuario.Rol.Roles = new List<object>();
                }
                return View(empleado);
            }

            ML.Result result = _empleadoBL.Update(empleado);
            if (result.Correct)
            {
                TempData["Message"] = "Empleado actualizado correctamente";
                return RedirectToAction("GetAll");
            }
            else
            {
                ML.Result resultRoles = _rolBL.RolGetAll();
                if (empleado.Usuario == null) empleado.Usuario = new ML.Usuario();
                if (empleado.Usuario.Rol == null) empleado.Usuario.Rol = new ML.Rol();

                if (resultRoles.Correct)
                {
                    empleado.Usuario.Rol.Roles = resultRoles.Objects;
                }
                else
                {
                    empleado.Usuario.Rol.Roles = new List<object>();
                }

                ViewBag.Message = "Error al actualizar Empleado: " + result.ErrorMessage;
                return View(empleado);
            }
        }

        [HttpGet]
        public ActionResult EmpleadoDelete(int idEmpleado)
        {
            ML.Result result = _empleadoBL.Delete(idEmpleado);
            if (result.Correct)
            {
                TempData["Message"] = "Empleado eliminado correctamente";
            }
            else
            {
                TempData["ErrorMessage"] = "Error al eliminar Empleado: " + result.ErrorMessage;
            }
            return RedirectToAction("GetAll");
        }
    }
}
