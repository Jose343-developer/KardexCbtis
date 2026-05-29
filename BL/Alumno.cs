using System;
using DL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Identity.Client;

namespace BL;

public class Alumno
{
    private readonly DL.ApplicationDbContext _context;

    public Alumno(DL.ApplicationDbContext context)
    {
        _context = context;
    }

    public ML.Result GetAll (ML.Alumno alumno)

    {
        
ML.Result result = new ML.Result();

        try
        {
            var context = _context;
            {
                

            var query = context.AlumnoGetAlls.FromSqlInterpolated($"EXEC AlumnoGetAll").ToList();
                if (query != null)
                {
                 result.Objects = new List<object>();

                    foreach(var item in query)
                    {
                        
                            ML.Alumno alumnos = new ML.Alumno();

                            alumnos.idAlumno = item.IdAlumno;
                            alumnos.Matricula = item.Matricula;
                            alumnos.Curp = item.Curp;
                            alumnos.Nombre = item.Nombre;
                            alumnos.ApellidoPaterno = item.ApellidoPaterno;
                            alumnos.ApellidoMaterno=item.ApellidoMaterno;
                            alumnos.Correo = item.Correo;
                            alumnos.Telefono = item.Telefono;
                            alumnos.Celular = item.Celular;
                            alumnos.Estatus = item.Estatus;
                            alumnos.Especialidad = new ML.Especialidad();
                            alumnos.Especialidad.IdEspecialidad= item.IdEspecialidad??0;
                            alumnos.Especialidad.Nombre = item.NombreEspecialidad??"Sin especialidad";
                            alumnos.Especialidad.ClaveOficial = item.ClaveOficial??"sin clave";
                            alumnos.Grupo = new ML.Grupo();
                            alumnos.Grupo.IdGrupo=item.IdGrupo;
                            alumnos.Grupo.Semestre = item.Semestre;
                            alumnos.Grupo.Letra = item.Letra;
                            alumnos.Grupo.Turno = item.Turno;

                            result.Objects.Add(alumnos);



                    }
                        result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "error en el obtener datos";
                }
                    
            }



        }
        catch(Exception ex)
        {
            
        result.Correct = false;
        result.ErrorMessage = ex.Message;
        result.Ex = ex;

        }
            return result;
    }

public ML.Result AlumnoAdd (ML.Alumno alumno)
    {
        
ML.Result result = new ML.Result();

        try
        {
            
                var context = _context;
            {
                
                var query = context.Database.ExecuteSqlInterpolated($@"EXEC AlumnoAdd
                
                {alumno.Matricula}, 
                {alumno.Curp}, 
                {alumno.Nombre}, 
                {alumno.ApellidoPaterno}, 
                {alumno.ApellidoMaterno}, 
                {alumno.Correo}, 
                {alumno.Telefono}, 
                {alumno.Celular}, 
                {alumno.Especialidad?.IdEspecialidad}, 
                {alumno.Grupo?.IdGrupo} ");

                if (query > 0)
                {
                    



                    result.Correct = true;

                }
                else
                {
                    
                    result.Correct = false;
                    result.ErrorMessage = "error al insertra datos";

                }


            }


        }
        catch(Exception ex)
        {
            
            result.Correct = false;
            result.ErrorMessage = ex.Message;
            result.Ex = ex;

        }
        return result;

        

    }

public ML.Result CountAlumno()
    {
        ML.Result result = new ML.Result();
        try
        {
            var context = _context;
            {
                var query = context.AlumnosCountSemestres.FromSqlInterpolated($"EXEC AlumnosCountSemestre").AsEnumerable().FirstOrDefault();
                if (query != null)
                {
                    result.Correct = true;
                    result.Object = query.Cantidad;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "NO HAY ALUMNOS";
                }
            }
        }
        catch(Exception ex)
        {
            result.Correct = false;
            result.ErrorMessage = ex.Message;
            result.Ex = ex;
        }
        return result;
    }

public ML.Result GetCountAlumnoBySemestre(int semestre)
    {
        ML.Result result = new ML.Result();
        try
        {
            
            var context = _context;
            {
                
                    var query = context.AlumnosCountSemestres.FromSqlInterpolated(
                        $@"EXEC GetCountAlumnosForSemestre {semestre}").AsEnumerable().FirstOrDefault();

                        if (query != null)
                {
                    
                    result.Object= query.Cantidad;
                    result.Correct = true;
                }
                else
                {
                    result.ErrorMessage = "no hay alumnos en estre semestre";
                    result.Correct = false;


                }
            }

        }catch(Exception ex)
        {
            result.Correct = false;
            result.ErrorMessage = ex.Message;
            result.Ex = ex;

        }
        return result;
    }
}
