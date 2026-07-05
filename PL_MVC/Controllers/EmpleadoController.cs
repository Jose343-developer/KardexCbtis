using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using PL_MVC.Filters;
using System.Linq;
using System.Security.Claims;

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

        private List<object> FilterRoles(List<object> roles)
        {
            var loggedInRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role || c.Type == "role")?.Value;
            if (!string.Equals(loggedInRole, "Administrador", System.StringComparison.OrdinalIgnoreCase))
            {
                return roles.Where(r => !string.Equals(((ML.Rol)r).Nombre, "Administrador", System.StringComparison.OrdinalIgnoreCase)).ToList();
            }
            return roles;
        }

        private void ValidateRoleAssignment(ML.Empleado empleado, ML.Result resultRoles)
        {
            var loggedInRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role || c.Type == "role")?.Value;
            if (!string.Equals(loggedInRole, "Administrador", System.StringComparison.OrdinalIgnoreCase))
            {
                if (empleado.Usuario?.Rol != null && resultRoles.Correct)
                {
                    var submittedRole = (ML.Rol)resultRoles.Objects.FirstOrDefault(r => ((ML.Rol)r).IdRol == empleado.Usuario.Rol.IdRol);
                    if (submittedRole != null && string.Equals(submittedRole.Nombre, "Administrador", System.StringComparison.OrdinalIgnoreCase))
                    {
                        ModelState.AddModelError("Usuario.Rol.IdRol", "No tienes permisos para asignar el rol de Administrador.");
                    }
                }
            }
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
                empleado.Usuario.Rol.Roles = FilterRoles(resultRoles.Objects);
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
            var rolBL = _rolBL;
            ML.Result resultRoles = rolBL.RolGetAll();

            ValidateRoleAssignment(empleado, resultRoles);

            if (!ModelState.IsValid)
            {
                if (empleado.Usuario == null) empleado.Usuario = new ML.Usuario();
                if (empleado.Usuario.Rol == null) empleado.Usuario.Rol = new ML.Rol();

                if (resultRoles.Correct)
                {
                    empleado.Usuario.Rol.Roles = FilterRoles(resultRoles.Objects);
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
                if (empleado.Usuario == null) empleado.Usuario = new ML.Usuario();
                if (empleado.Usuario.Rol == null) empleado.Usuario.Rol = new ML.Rol();

                if (resultRoles.Correct)
                {
                    empleado.Usuario.Rol.Roles = FilterRoles(resultRoles.Objects);
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
                    model.Usuario.Rol.Roles = FilterRoles(resultRoles.Objects);
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

            ML.Result resultRoles = _rolBL.RolGetAll();
            ValidateRoleAssignment(empleado, resultRoles);

            if (!ModelState.IsValid)
            {
                if (empleado.Usuario == null) empleado.Usuario = new ML.Usuario();
                if (empleado.Usuario.Rol == null) empleado.Usuario.Rol = new ML.Rol();

                if (resultRoles.Correct)
                {
                    empleado.Usuario.Rol.Roles = FilterRoles(resultRoles.Objects);
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
                if (empleado.Usuario == null) empleado.Usuario = new ML.Usuario();
                if (empleado.Usuario.Rol == null) empleado.Usuario.Rol = new ML.Rol();

                if (resultRoles.Correct)
                {
                    empleado.Usuario.Rol.Roles = FilterRoles(resultRoles.Objects);
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
