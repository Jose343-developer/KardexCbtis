using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace PL_MVC.Controllers
{
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
    }
}
