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
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _especialidadBL.GetById(id);
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
        public IActionResult Add([FromBody] ML.Especialidad especialidad)
        {
            var result = _especialidadBL.Add(especialidad);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPut]
        public IActionResult Update([FromBody] ML.Especialidad especialidad)
        {
            var result = _especialidadBL.Update(especialidad);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _especialidadBL.Delete(id);
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
