using Microsoft.AspNetCore.Mvc;

namespace ApiCbtis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GrupoController : ControllerBase
    {
        private readonly BL.Grupo _grupoBL;

        public GrupoController(BL.Grupo grupoBL)
        {
            _grupoBL = grupoBL;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _grupoBL.GrupoGetAll();
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
        [Route("GetTurno")]
        public IActionResult GetTurno()
        {
            var result = _grupoBL.GetTurno();
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
        [Route("GetSemestres")]
        public IActionResult GetSemestres()
        {
            var result = _grupoBL.GetSemestres();
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
