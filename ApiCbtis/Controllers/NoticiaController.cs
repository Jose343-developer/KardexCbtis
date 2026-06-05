using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiCbtis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoticiaController : ControllerBase
    {
        [HttpGet("GetAll")]
        [Authorize]
        public IActionResult GetAll()
        {
            ML.Result result = BL.Noticia.GetAll();
            if (result.Correct)
            {
                return Ok(result);
            }
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("GetRelevante")]
        [Authorize]
        public IActionResult GetRelevante()
        {
            ML.Result result = BL.Noticia.GetRelevante();
            if (result.Correct)
            {
                return Ok(result);
            }
            return NotFound(result.ErrorMessage);
        }

        [HttpPost("Add")]
        [Authorize(Roles = "Administrador")]
        public IActionResult Add([FromBody] ML.Noticia noticia)
        {
            ML.Result result = BL.Noticia.Add(noticia);
            if (result.Correct)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
