using Microsoft.AspNetCore.Mvc;
using System;

namespace ApiCbtis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HorarioController : ControllerBase
    {
        private readonly BL.Horario _horarioBL;

        public HorarioController(BL.Horario horarioBL)
        {
            _horarioBL = horarioBL;
        }

        [HttpPost]
        public IActionResult AddOrUpdate([FromBody] ML.Horario horario)
        {
            var result = _horarioBL.AddOrUpdate(horario);
            if (result.Correct)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("grupo/{idGrupo}")]
        public IActionResult GetByGrupo(int idGrupo)
        {
            var result = _horarioBL.GetByGrupo(idGrupo);
            if (result.Correct)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpDelete("{idHorario}")]
        public IActionResult Delete(int idHorario)
        {
            var result = _horarioBL.Delete(idHorario);
            if (result.Correct)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
