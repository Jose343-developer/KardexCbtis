using Microsoft.AspNetCore.Mvc;

namespace ApiCbtis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AsignacionDocenteController : ControllerBase
    {
        private readonly BL.AsignacionDocente _asignacionBL;

        public AsignacionDocenteController(BL.AsignacionDocente asignacionBL)
        {
            _asignacionBL = asignacionBL;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _asignacionBL.GetAll();
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost]
        public IActionResult Add([FromBody] ML.AsignacionDocente asignacion)
        {
            var result = _asignacionBL.Add(asignacion);
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
