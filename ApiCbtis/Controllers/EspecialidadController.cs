using Microsoft.AspNetCore.Mvc;

namespace ApiCbtis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EspecialidadController : ControllerBase
    {
        private readonly BL.Especialidad _especialidadBL;

        public EspecialidadController(BL.Especialidad especialidadBL)
        {
            _especialidadBL = especialidadBL;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _especialidadBL.EspecialidadGetAll();
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
