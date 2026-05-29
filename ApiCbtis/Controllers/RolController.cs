using Microsoft.AspNetCore.Mvc;

namespace ApiCbtis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolController : ControllerBase
    {
        private readonly BL.Rol _rolBL;

        public RolController(BL.Rol rolBL)
        {
            _rolBL = rolBL;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _rolBL.RolGetAll();
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
