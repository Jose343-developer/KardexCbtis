using Microsoft.AspNetCore.Mvc;

namespace ApiCbtis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpleadoController : ControllerBase
    {
        private readonly BL.Empleado _empleadoBL;

        public EmpleadoController(BL.Empleado empleadoBL)
        {
            _empleadoBL = empleadoBL;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _empleadoBL.GetAll();
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
        public IActionResult Add([FromBody] ML.Empleado empleado)
        {
            var result = _empleadoBL.Add(empleado);
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
