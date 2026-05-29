using Microsoft.AspNetCore.Mvc;

namespace ApiCbtis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CalificacionController : ControllerBase
    {
        private readonly BL.Calificacion _calificacionBL;

        public CalificacionController(BL.Calificacion calificacionBL)
        {
            _calificacionBL = calificacionBL;
        }

        [HttpPost]
        public IActionResult Add([FromBody] ML.Calificacion calificacion)
        {
            var result = _calificacionBL.Add(calificacion);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet]
        [Route("GetByAlumno/{idAlumno}")]
        public IActionResult GetByAlumno(int idAlumno)
        {
            var result = _calificacionBL.GetByAlumno(idAlumno);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet]
        [Route("GetStatistics")]
        public IActionResult GetStatistics()
        {
            var result = _calificacionBL.GetStatistics();
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
