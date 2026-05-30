using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PL_MVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly BL.Usuario _usuarioBL;
        private readonly IConfiguration _config;

        public LoginController(BL.Usuario usuarioBL, IConfiguration config)
        {
            _usuarioBL = usuarioBL;
            _config = config;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // If already logged in, redirect to correct landing page
            var token = Request.Cookies["tokenjwt"];
            if (!string.IsNullOrEmpty(token))
            {
                var role = GetRoleFromToken(token);
                if (!string.IsNullOrEmpty(role))
                {
                    return RedirectToLandingPage(role);
                }
            }

            return View(new ML.Login());
        }

        [HttpPost]
        public IActionResult Login(ML.Login model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ML.Result result = _usuarioBL.Login(model);

            if (result.Correct)
            {
                ML.Login usuarioLog = (ML.Login)result.Object!;

                if (usuarioLog.Resultado)
                {
                    string token = GenerarJwtToken(usuarioLog.IdUsuario, model.Correo, usuarioLog.Rol.Nombre!);

                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = Request.IsHttps,
                        SameSite = SameSiteMode.Lax,
                        Expires = DateTime.UtcNow.AddMinutes(30)
                    };

                    Response.Cookies.Append("tokenjwt", token, cookieOptions);

                    return RedirectToLandingPage(usuarioLog.Rol.Nombre!);
                }
                else
                {
                    ModelState.AddModelError("", "Usuario o contraseña incorrectos.");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("", "Ocurrió un error al iniciar sesión: " + result.ErrorMessage);
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("tokenjwt");
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private string GenerarJwtToken(int idUsuario, string correo, string rol)
        {
            var keyStr = _config["Jwt:Key"] ?? "1978APPPCONGIUSER_sistem?console_%%%contrsaena#segurademuchis12345679caracrtereSV777";
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyStr));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, idUsuario.ToString()),
                new Claim(ClaimTypes.Name, correo),
                new Claim(ClaimTypes.Email, correo),
                new Claim(ClaimTypes.Role, rol)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"] ?? "TuAppAPI",
                audience: _config["Jwt:Audience"] ?? "TuAppUsuarios",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string? GetRoleFromToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                return jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role || x.Type == "role")?.Value;
            }
            catch
            {
                return null;
            }
        }

        private IActionResult RedirectToLandingPage(string role)
        {
            if (role.Equals("Administrador", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Index", "Home");
            }
            else if (role.Equals("Profesor", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("GetAll", "Asignacion");
            }
            else if (role.Equals("Alumno", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("MisCalificaciones", "Calificacion");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
