using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PL_MVC.Filters;

namespace PL_MVC.Controllers
{
    [AuthorizeRole("Alumno")]
    public class CalificacionController : Controller
    {
        private readonly BL.Calificacion _calificacionBL;
        private readonly DL.ApplicationDbContext _context;

        public CalificacionController(BL.Calificacion calificacionBL, DL.ApplicationDbContext context)
        {
            _calificacionBL = calificacionBL;
            _context = context;
        }

        [HttpGet]
        public IActionResult MisCalificaciones()
        {
            // 1. Get logged-in user's email/username from claims
            string? username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Login");
            }

            // 2. Find corresponding Alumno record
            var dbAlumno = _context.Alumnos
                .Include(a => a.IdEspecialidadNavigation)
                .Include(a => a.IdGrupoNavigation)
                .FirstOrDefault(a => a.Matricula == username || a.Correo == username);

            if (dbAlumno == null)
            {
                ViewBag.ErrorMessage = "No se encontró un expediente de alumno vinculado a tu usuario.";
                return View(new ML.Alumno());
            }

            // 3. Map to ML.Alumno
            ML.Alumno alumno = new ML.Alumno
            {
                idAlumno = dbAlumno.IdAlumno,
                Matricula = dbAlumno.Matricula,
                Curp = dbAlumno.Curp,
                Nombre = dbAlumno.Nombre,
                ApellidoPaterno = dbAlumno.ApellidoPaterno,
                ApellidoMaterno = dbAlumno.ApellidoMaterno,
                Correo = dbAlumno.Correo,
                Estatus = dbAlumno.Estatus
            };

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

            // 4. Fetch grades
            ML.Result resultGrades = _calificacionBL.GetByAlumno(alumno.idAlumno.Value);
            List<ML.Calificacion> calificaciones = new List<ML.Calificacion>();
            
            if (resultGrades.Correct && resultGrades.Objects != null)
            {
                calificaciones = resultGrades.Objects.Cast<ML.Calificacion>().ToList();
            }

            alumno.Calificaciones = calificaciones;

            // 5. Calculate average (promedio)
            decimal promedio = 0;
            if (calificaciones.Count > 0)
            {
                promedio = calificaciones.Average(c => c.Nota);
            }
            ViewBag.Promedio = Math.Round(promedio, 2);

            return View(alumno);
        }
    }
}
