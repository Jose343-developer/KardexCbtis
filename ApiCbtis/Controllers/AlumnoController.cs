using Microsoft.AspNetCore.Mvc;

namespace ApiCbtis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlumnoController : ControllerBase
    {
        private readonly BL.Alumno _alumnoBL;

        public AlumnoController(BL.Alumno alumnoBL)
        {
            _alumnoBL = alumnoBL;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _alumnoBL.GetAll(new ML.Alumno());
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
        public IActionResult Add([FromBody] ML.Alumno alumno)
        {
            var result = _alumnoBL.AlumnoAdd(alumno);
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
