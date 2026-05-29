using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace PL_MVC.Controllers
{
    public class EmpleadoController : Controller
    {
        [HttpGet]
        public ActionResult GetAll()
        {
            BL.Empleado empleadoBL = new BL.Empleado();
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

            BL.Rol rolBL = new BL.Rol();
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
                BL.Rol rolBL = new BL.Rol();
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

            BL.Empleado empleadoBL = new BL.Empleado();
            ML.Result result = empleadoBL.Add(empleado);

            if (result.Correct)
            {
                TempData["Message"] = "Empleado añadido correctamente";
                return RedirectToAction("GetAll");
            }
            else
            {
                BL.Rol rolBL = new BL.Rol();
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
