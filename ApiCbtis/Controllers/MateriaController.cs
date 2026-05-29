using Microsoft.AspNetCore.Mvc;

namespace ApiCbtis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MateriaController : ControllerBase
    {
        private readonly BL.Materia _materiaBL;

        public MateriaController(BL.Materia materiaBL)
        {
            _materiaBL = materiaBL;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _materiaBL.GetAll();
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
        public IActionResult Add([FromBody] ML.Materia materia)
        {
            var result = _materiaBL.Add(materia);
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
