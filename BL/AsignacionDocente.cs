using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BL
{
    public class AsignacionDocente
    {
        private readonly DL.ApplicationDbContext _context;

        public AsignacionDocente(DL.ApplicationDbContext context)
        {
            _context = context;
        }

        public ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
            var context = _context;
            {
                    var query = context.AsignacionDocenteGetAlls.FromSqlInterpolated($"EXEC AsignacionDocenteGetAll").ToList();

                    if (query != null && query.Count > 0)
                    {
                        result.Objects = new List<object>();
                        foreach (var item in query)
                        {
                            ML.AsignacionDocente asignacion = new ML.AsignacionDocente();
                            asignacion.IdAsignacionDocente = item.IdAsignacionDocente;
                            asignacion.DiaSemana = item.DiaSemana;
                            asignacion.HoraInicio = item.HoraInicio;
                            asignacion.HoraFin = item.HoraFin;

                            asignacion.Empleado = new ML.Empleado();
                            asignacion.Empleado.IdEmpleado = item.IdEmpleado;
                            asignacion.Empleado.Nombre = item.NombreEmpleado;
                            asignacion.Empleado.ApellidoPaterno = item.ApellidoPaternoEmpleado;
                            asignacion.Empleado.ApellidoMaterno = item.ApellidoMaternoEmpleado;

                            asignacion.Materia = new ML.Materia();
                            asignacion.Materia.IdMateria = item.IdMateria;
                            asignacion.Materia.Nombre = item.NombreMateria;
                            asignacion.Materia.Semestre = item.SemestreMateria;

                            asignacion.Grupo = new ML.Grupo();
                            asignacion.Grupo.IdGrupo = item.IdGrupo;
                            asignacion.Grupo.Semestre = item.SemestreGrupo;
                            asignacion.Grupo.Letra = item.LetraGrupo;
                            asignacion.Grupo.Turno = item.TurnoGrupo;

                            result.Objects.Add(asignacion);
                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se encontraron asignaciones de docentes";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }

        public ML.Result Add(ML.AsignacionDocente asignacion)
        {
            ML.Result result = new ML.Result();
            try
            {
            var context = _context;
            {
                    var rowsAffected = context.Database.ExecuteSqlInterpolated($@"EXEC AsignacionDocenteAdd 
                        {asignacion.Empleado.IdEmpleado}, 
                        {asignacion.Materia.IdMateria}, 
                        {asignacion.Grupo.IdGrupo}, 
                        {asignacion.DiaSemana}, 
                        {asignacion.HoraInicio}, 
                        {asignacion.HoraFin}");

                    result.Correct = true;
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                result.Ex = ex;
            }
            return result;
        }

    }
}
