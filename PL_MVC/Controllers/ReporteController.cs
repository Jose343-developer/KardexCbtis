using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PL_MVC.Controllers
{
    public class ReporteController : Controller
    {
        [HttpGet]
        public IActionResult Dashboard()
        {
            BL.Calificacion blCalificacion = new BL.Calificacion();
            ML.Result result = blCalificacion.GetStatistics();

            if (result.Correct)
            {
                return View(result.Object);
            }
            else
            {
                ViewBag.ErrorMessage = result.ErrorMessage;
                return View(new Dictionary<string, object>());
            }
        }

        [HttpGet]
        public IActionResult Boleta(int idAlumno)
        {
            using (DL.ApplicationDbContext context = new DL.ApplicationDbContext())
            {
                var dbAlumno = context.Alumnos
                    .Include(a => a.IdEspecialidadNavigation)
                    .Include(a => a.IdGrupoNavigation)
                    .FirstOrDefault(a => a.IdAlumno == idAlumno);

                if (dbAlumno == null)
                {
                    TempData["ErrorMessage"] = "Alumno no encontrado.";
                    return RedirectToAction("GetAll", "Alumno");
                }

                // Map to ML.Alumno
                ML.Alumno alumno = new ML.Alumno();
                alumno.idAlumno = dbAlumno.IdAlumno;
                alumno.Matricula = dbAlumno.Matricula;
                alumno.Curp = dbAlumno.Curp;
                alumno.Nombre = dbAlumno.Nombre;
                alumno.ApellidoPaterno = dbAlumno.ApellidoPaterno;
                alumno.ApellidoMaterno = dbAlumno.ApellidoMaterno;
                alumno.Correo = dbAlumno.Correo;
                alumno.Telefono = dbAlumno.Telefono;
                alumno.Celular = dbAlumno.Celular;
                alumno.Estatus = dbAlumno.Estatus;

                alumno.Especialidad = new ML.Especialidad();
                if (dbAlumno.IdEspecialidadNavigation != null)
                {
                    alumno.Especialidad.IdEspecialidad = dbAlumno.IdEspecialidadNavigation.IdEspecialidad;
                    alumno.Especialidad.Nombre = dbAlumno.IdEspecialidadNavigation.Nombre;
                    alumno.Especialidad.ClaveOficial = dbAlumno.IdEspecialidadNavigation.ClaveOficial;
                }

                alumno.Grupo = new ML.Grupo();
                if (dbAlumno.IdGrupoNavigation != null)
                {
                    alumno.Grupo.IdGrupo = dbAlumno.IdGrupoNavigation.IdGrupo;
                    alumno.Grupo.Semestre = dbAlumno.IdGrupoNavigation.Semestre;
                    alumno.Grupo.Letra = dbAlumno.IdGrupoNavigation.Letra;
                    alumno.Grupo.Turno = dbAlumno.IdGrupoNavigation.Turno;
                }

                // Get Grades
                BL.Calificacion blCalificacion = new BL.Calificacion();
                ML.Result resultCalificaciones = blCalificacion.GetByAlumno(idAlumno);
                
                List<ML.Calificacion> calificaciones = new List<ML.Calificacion>();
                if (resultCalificaciones.Correct)
                {
                    calificaciones = resultCalificaciones.Objects.Cast<ML.Calificacion>().ToList();
                }

                alumno.Calificaciones = calificaciones;
                return View(alumno);
            }
        }
    }
}
