using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using DL;
using Microsoft.EntityFrameworkCore;

namespace BL
{
    public class Usuario
    {
        private readonly DL.ApplicationDbContext _context;

        public Usuario(DL.ApplicationDbContext context)
        {
            _context = context;
        }

        public ML.Result Login(ML.Login login)
        {
            ML.Result result = new ML.Result();
            try
            {
                // Attempt 1: SHA-256 Hashed Password
                string hashedInput = ComputeSHA256(login.Password);
                var query = _context.LoginUsuarioResults
                    .FromSqlInterpolated($"EXEC sp_LoginUsuario {login.Correo}, {hashedInput}")
                    .AsEnumerable()
                    .FirstOrDefault();

                // Attempt 2: Plain Text Password
                if (query == null)
                {
                    query = _context.LoginUsuarioResults
                        .FromSqlInterpolated($"EXEC sp_LoginUsuario {login.Correo}, {login.Password}")
                        .AsEnumerable()
                        .FirstOrDefault();
                }

                if (query != null)
                {
                    ML.Login usuarioLog = new ML.Login
                    {
                        Correo = login.Correo,
                        Resultado = true,
                        IdUsuario = query.IdUsuario
                    };

                    var rol = _context.Rols.FirstOrDefault(r => r.IdRol == query.IdRol);
                    if (rol != null)
                    {
                        usuarioLog.Rol = new ML.Rol
                        {
                            IdRol = rol.IdRol,
                            Nombre = rol.Nombre
                        };
                    }
                    else
                    {
                        usuarioLog.Rol = new ML.Rol
                        {
                            IdRol = query.IdRol,
                            Nombre = "Sin Rol"
                        };
                    }

                    result.Correct = true;
                    result.Object = usuarioLog;
                }
                else
                {
                    login.Resultado = false;
                    result.Correct = true;
                    result.Object = login;
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



        private string ComputeSHA256(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("X2"));
                }
                return builder.ToString();
            }
        }
    }
}
