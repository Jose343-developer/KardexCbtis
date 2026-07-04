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

    public ML.Result AlumnoAdd(ML.Alumno alumno)
    {
        ML.Result result = new ML.Result();

        using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {
                int? idUsuario = null;

                // 1. Create the user if provided
                if (alumno.Usuario != null && !string.IsNullOrEmpty(alumno.Usuario.NombreUser))
                {
                    bool userExists = _context.Usuarios.Any(u => u.NombreUser == alumno.Usuario.NombreUser);
                    if (userExists)
                    {
                        result.Correct = false;
                        result.ErrorMessage = "El nombre de usuario ya está registrado.";
                        return result;
                    }

                    var dbUsuario = new DL.Models.Usuario
                    {
                        NombreUser = alumno.Usuario.NombreUser,
                        Password = ComputeSHA256(alumno.Usuario.Password ?? ""),
                        IdRol = 3, // Alumno
                        Estatus = true
                    };
                    _context.Usuarios.Add(dbUsuario);
                    _context.SaveChanges();
                    idUsuario = dbUsuario.IdUsuario;
                }

                // 2. Insert the student using existing stored procedure
                var context = _context;
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

                if (query > 0 || query == -1)
                {
                    // 3. Link the user to the student
                    if (idUsuario.HasValue)
                    {
                        var dbAlumno = _context.Alumnos.FirstOrDefault(a => a.Matricula == alumno.Matricula);
                        if (dbAlumno != null)
                        {
                            dbAlumno.IdUsuario = idUsuario.Value;
                            _context.SaveChanges();
                        }
                    }

                    transaction.Commit();
                    result.Correct = true;
                }
                else
                {
                    transaction.Rollback();
                    result.Correct = false;
                    result.ErrorMessage = "error al insertar datos";
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
        }
        return result;
    }

    private string ComputeSHA256(string input)
    {
        using (System.Security.Cryptography.SHA256 sha256 = System.Security.Cryptography.SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("X2"));
            }
            return builder.ToString();
        }
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

    public ML.Result ChangeStatus(int idAlumno, string estatus)
    {
        ML.Result result = new ML.Result();
        try
        {
            var alumno = _context.Alumnos.Find(idAlumno);
            if (alumno != null)
            {
                alumno.Estatus = estatus;
                _context.SaveChanges();
                result.Correct = true;
            }
            else
            {
                result.Correct = false;
                result.ErrorMessage = "No se encontró el alumno.";
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

    public ML.Result GetByIdUsuario(int idUsuario)
    {
        ML.Result result = new ML.Result();
        try
        {
            var dbAlumno = _context.Alumnos.FirstOrDefault(a => a.IdUsuario == idUsuario);
            if (dbAlumno != null)
            {
                ML.Alumno alumno = new ML.Alumno
                {
                    idAlumno = dbAlumno.IdAlumno,
                    Matricula = dbAlumno.Matricula,
                    Nombre = dbAlumno.Nombre,
                    Grupo = new ML.Grupo
                    {
                        IdGrupo = dbAlumno.IdGrupo ?? 0
                    }
                };
                result.Object = alumno;
                result.Correct = true;
            }
            else
            {
                result.Correct = false;
                result.ErrorMessage = "No se encontró el alumno.";
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

    public ML.Result Search(string term)
    {
        ML.Result result = new ML.Result();
        try
        {
            var query = (from a in _context.Alumnos
                         join u in _context.Usuarios on a.IdUsuario equals u.IdUsuario into leftJoinUsuario
                         from lu in leftJoinUsuario.DefaultIfEmpty()
                         where a.Nombre.Contains(term) || 
                               (a.Correo != null && a.Correo.Contains(term)) || 
                               (lu != null && lu.NombreUser != null && lu.NombreUser.Contains(term))
                         select new ML.Alumno
                         {
                             idAlumno = a.IdAlumno,
                             Nombre = a.Nombre,
                             ApellidoPaterno = a.ApellidoPaterno,
                             ApellidoMaterno = a.ApellidoMaterno,
                             Correo = a.Correo,
                             Matricula = a.Matricula,
                             Usuario = lu != null ? new ML.Usuario
                             {
                                 IdUsuario = lu.IdUsuario,
                                 NombreUser = lu.NombreUser
                             } : null
                         }).ToList();
            
            result.Objects = new List<object>();
            foreach (var item in query)
            {
                result.Objects.Add(item);
            }
            result.Correct = true;
        }
        catch (Exception ex)
        {
            result.Correct = false;
            result.ErrorMessage = ex.Message;
            result.Ex = ex;
        }
        return result;
    }

    public ML.Result DeleteFull(int idAlumno, int idUsuario)
    {
        ML.Result result = new ML.Result();
        using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {
                // Delete Calificaciones
                var calificaciones = _context.Calificaciones.Where(c => c.IdAlumno == idAlumno);
                if (calificaciones.Any())
                {
                    _context.Calificaciones.RemoveRange(calificaciones);
                }

                // Delete Alumno
                var dbAlumno = _context.Alumnos.Find(idAlumno);
                if (dbAlumno != null)
                {
                    _context.Alumnos.Remove(dbAlumno);
                }

                // Delete Usuario
                if (idUsuario > 0)
                {
                    var dbUsuario = _context.Usuarios.Find(idUsuario);
                    if (dbUsuario != null)
                    {
                        _context.Usuarios.Remove(dbUsuario);
                    }
                }

                _context.SaveChanges();
                transaction.Commit();
                result.Correct = true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
        }
        return result;
    }
}
