using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace PL_MVC.Filters
{
    public class AuthorizeRoleAttribute : ActionFilterAttribute
    {
        private readonly string[] _roles;

        public AuthorizeRoleAttribute(params string[] roles)
        {
            _roles = roles;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var token = context.HttpContext.Request.Cookies["tokenjwt"];

            if (string.IsNullOrEmpty(token))
            {
                context.Result = new RedirectToActionResult("Login", "Login", null);
                return;
            }

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var keyStr = configuration["Jwt:Key"] ?? "1978APPPCONGIUSER_sistem?console_%%%contrsaena#segurademuchis12345679caracrtereSV777";
                var key = Encoding.UTF8.GetBytes(keyStr);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Jwt:Issuer"] ?? "TuAppAPI",
                    ValidateAudience = true,
                    ValidAudience = configuration["Jwt:Audience"] ?? "TuAppUsuarios",
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                
                // Get name/email and role claims
                var roleClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role || x.Type == "role")?.Value;

                if (string.IsNullOrEmpty(roleClaim))
                {
                    context.Result = new RedirectToActionResult("AccessDenied", "Login", null);
                    return;
                }

                if (_roles.Length > 0 && !_roles.Contains(roleClaim))
                {
                    context.Result = new RedirectToActionResult("AccessDenied", "Login", null);
                    return;
                }

                // Create identity and set HttpContext.User
                var identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
                context.HttpContext.User = new ClaimsPrincipal(identity);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[JWT Auth Error] Validation failed: {ex.Message}");
                // Delete invalid or expired cookie
                context.HttpContext.Response.Cookies.Delete("tokenjwt");
                context.Result = new RedirectToActionResult("Login", "Login", null);
            }

            base.OnActionExecuting(context);
        }
    }
}
