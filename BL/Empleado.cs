using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BL
{
    public class Empleado
    {
        private readonly DL.ApplicationDbContext _context;

        public Empleado(DL.ApplicationDbContext context)
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
                    var query = context.EmpleadoGetAlls.FromSqlInterpolated($"EXEC EmpleadoGetAll").ToList();

                    if (query != null && query.Count > 0)
                    {
                        result.Objects = new List<object>();
                        foreach (var item in query)
                        {
                            ML.Empleado empleado = new ML.Empleado();
                            empleado.IdEmpleado = item.IdEmpleado;
                            empleado.Curp = item.Curp;
                            empleado.Rfc = item.Rfc;
                            empleado.Nombre = item.Nombre;
                            empleado.ApellidoPaterno = item.ApellidoPaterno;
                            empleado.ApellidoMaterno = item.ApellidoMaterno;
                            empleado.Correo = item.Correo;
                            empleado.Telefono = item.Telefono;
                            empleado.Celular = item.Celular;
                            empleado.Departamento = item.Departamento;

                            empleado.Usuario = new ML.Usuario();
                            empleado.Usuario.IdUsuario = item.IdUsuario;
                            empleado.Usuario.NombreUser = item.NombreUser;
                            empleado.Usuario.Password = item.Password;
                            empleado.Usuario.Estatus = item.Estatus;

                            empleado.Usuario.Rol = new ML.Rol();
                            empleado.Usuario.Rol.IdRol = item.IdRol;
                            empleado.Usuario.Rol.Nombre = item.NombreRol;

                            result.Objects.Add(empleado);
                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se encontraron empleados";
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

        public ML.Result Add(ML.Empleado empleado)
        {
            ML.Result result = new ML.Result();
            try
            {
            var context = _context;
            {
                    var rowsAffected = context.Database.ExecuteSqlInterpolated($@"EXEC EmpleadoAdd 
                        {empleado.Usuario.NombreUser}, 
                        {empleado.Usuario.Password}, 
                        {empleado.Usuario.Rol.IdRol}, 
                        {empleado.Curp}, 
                        {empleado.Rfc}, 
                        {empleado.Nombre}, 
                        {empleado.ApellidoPaterno}, 
                        {empleado.ApellidoMaterno}, 
                        {empleado.Correo}, 
                        {empleado.Telefono}, 
                        {empleado.Celular}, 
                        {empleado.Departamento}");

                    if (rowsAffected > 0 || rowsAffected == -1)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se pudo insertar el empleado.";
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

        public ML.Result GetById(int idEmpleado)
        {
            ML.Result result = new ML.Result();
            try
            {
                var query = _context.EmpleadoGetAlls.FromSqlInterpolated($"EXEC EmpleadoGetById {idEmpleado}").AsEnumerable().FirstOrDefault();

                if (query != null)
                {
                    ML.Empleado empleado = new ML.Empleado();
                    empleado.IdEmpleado = query.IdEmpleado;
                    empleado.Curp = query.Curp;
                    empleado.Rfc = query.Rfc;
                    empleado.Nombre = query.Nombre;
                    empleado.ApellidoPaterno = query.ApellidoPaterno;
                    empleado.ApellidoMaterno = query.ApellidoMaterno;
                    empleado.Correo = query.Correo;
                    empleado.Telefono = query.Telefono;
                    empleado.Celular = query.Celular;
                    empleado.Departamento = query.Departamento;

                    empleado.Usuario = new ML.Usuario();
                    empleado.Usuario.IdUsuario = query.IdUsuario;
                    empleado.Usuario.NombreUser = query.NombreUser;
                    empleado.Usuario.Password = query.Password;
                    empleado.Usuario.Estatus = query.Estatus;

                    empleado.Usuario.Rol = new ML.Rol();
                    empleado.Usuario.Rol.IdRol = query.IdRol;
                    empleado.Usuario.Rol.Nombre = query.NombreRol;

                    result.Object = empleado;
                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "No se encontró el empleado";
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

        public ML.Result Update(ML.Empleado empleado)
        {
            ML.Result result = new ML.Result();
            try
            {
                var rowsAffected = _context.Database.ExecuteSqlInterpolated($@"EXEC EmpleadoUpdate 
                    {empleado.IdEmpleado},
                    {empleado.Usuario.NombreUser}, 
                    {empleado.Usuario.Password}, 
                    {empleado.Usuario.Rol.IdRol}, 
                    {empleado.Curp}, 
                    {empleado.Rfc}, 
                    {empleado.Nombre}, 
                    {empleado.ApellidoPaterno}, 
                    {empleado.ApellidoMaterno}, 
                    {empleado.Correo}, 
                    {empleado.Telefono}, 
                    {empleado.Celular}, 
                    {empleado.Departamento}");

                if (rowsAffected > 0 || rowsAffected == -1)
                {
                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "No se pudo actualizar el empleado.";
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

        public ML.Result Delete(int idEmpleado)
        {
            ML.Result result = new ML.Result();
            try
            {
                var rowsAffected = _context.Database.ExecuteSqlInterpolated($"EXEC EmpleadoDelete {idEmpleado}");

                if (rowsAffected > 0 || rowsAffected == -1)
                {
                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "No se pudo eliminar el empleado.";
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

    }
}
