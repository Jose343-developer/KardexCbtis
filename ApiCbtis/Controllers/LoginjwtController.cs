using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiCbtis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginjwtController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly BL.Usuario _usuario;

        public LoginjwtController(BL.Usuario usuario, IConfiguration config)
        {
            _config = config;
            _usuario = usuario;
        }

        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("tokenjwt");
            return Ok(new { mensaje = "sesion eliminada correctamente" });
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] ML.Login login)
        {
            ML.Result result = _usuario.Login(login);
            if (result.Correct)
            {
                ML.Login usuarioLog = (ML.Login)result.Object!;

                if (usuarioLog.Resultado)
                {
                    string token = GenerarJwtToken(usuarioLog.IdUsuario, login.Correo, usuarioLog.Rol.Nombre!);

                    var cookie = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = Request.IsHttps,
                        SameSite = Request.IsHttps ? SameSiteMode.None : SameSiteMode.Lax,
                        Expires = DateTime.UtcNow.AddDays(1)
                    };

                    Response.Cookies.Append("tokenjwt", token, cookie);

                    return Ok(new { Mensaje = "Login exitoso", Rol = usuarioLog.Rol.Nombre, token });
                }
                else
                {
                    return Unauthorized(new { Mensaje = "Usuario o contraseña incorrectos." });
                }
            }
            return BadRequest(new { Mensaje = result.ErrorMessage });
        }

        private string GenerarJwtToken(int idUsuario, string correo, string rol)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? "1978APPPCONGIUSER_sistem?console_%%%contrsaena#segurademuchis12345679caracrtereSV777"));
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
                expires: DateTime.UtcNow.AddDays(1), // Match the cookie expiration
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpGet("VerfyToken")]
        [Authorize]
        public IActionResult VerfyToken()
        {
            return Ok(200);
        }
    }
}
