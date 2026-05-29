using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BL
{
    public class Empleado
    {
        public ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.ApplicationDbContext context = new DL.ApplicationDbContext())
                {
                    var query = context.Empleados
                        .Include(e => e.IdUsuarioNavigation)
                        .ThenInclude(u => u.IdRolNavigation)
                        .ToList();

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
                            if (item.IdUsuarioNavigation != null)
                            {
                                empleado.Usuario.NombreUser = item.IdUsuarioNavigation.NombreUser;
                                empleado.Usuario.Password = item.IdUsuarioNavigation.Password;
                                empleado.Usuario.Estatus = item.IdUsuarioNavigation.Estatus;

                                empleado.Usuario.Rol = new ML.Rol();
                                if (item.IdUsuarioNavigation.IdRolNavigation != null)
                                {
                                    empleado.Usuario.Rol.IdRol = item.IdUsuarioNavigation.IdRolNavigation.IdRol;
                                    empleado.Usuario.Rol.Nombre = item.IdUsuarioNavigation.IdRolNavigation.Nombre;
                                }
                            }

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
                using (DL.ApplicationDbContext context = new DL.ApplicationDbContext())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            // 1. Crear el usuario
                            DL.Models.Usuario dbUsuario = new DL.Models.Usuario();
                            dbUsuario.NombreUser = empleado.Usuario.NombreUser;
                            dbUsuario.Password = empleado.Usuario.Password;
                            dbUsuario.IdRol = empleado.Usuario.Rol.IdRol ?? 1;
                            dbUsuario.Estatus = true;

                            context.Usuarios.Add(dbUsuario);
                            context.SaveChanges(); // Genera IdUsuario

                            // 2. Crear el empleado
                            DL.Models.Empleado dbEmpleado = new DL.Models.Empleado();
                            dbEmpleado.IdUsuario = dbUsuario.IdUsuario;
                            dbEmpleado.Curp = empleado.Curp;
                            dbEmpleado.Rfc = empleado.Rfc;
                            dbEmpleado.Nombre = empleado.Nombre;
                            dbEmpleado.ApellidoPaterno = empleado.ApellidoPaterno;
                            dbEmpleado.ApellidoMaterno = empleado.ApellidoMaterno;
                            dbEmpleado.Correo = empleado.Correo;
                            dbEmpleado.Telefono = empleado.Telefono;
                            dbEmpleado.Celular = empleado.Celular;
                            dbEmpleado.Departamento = empleado.Departamento;

                            context.Empleados.Add(dbEmpleado);
                            context.SaveChanges();

                            transaction.Commit();
                            result.Correct = true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw;
                        }
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
    }
}
