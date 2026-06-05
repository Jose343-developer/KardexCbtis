using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using PL_MVC.Filters;

namespace PL_MVC.Controllers
{
    [AuthorizeRole("Administrador")]
    public class CatalogoController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl = "http://localhost:5013/api"; // Updated to ApiCbtis default port

        public CatalogoController(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClient = httpClientFactory.CreateClient();
            string? apiUrl = config.GetValue<string>("ApiUrl");
            if (!string.IsNullOrEmpty(apiUrl))
            {
                _apiBaseUrl = apiUrl.TrimEnd('/');
            }
            else
            {
                _apiBaseUrl = "http://localhost:5013/api"; // Default fallback
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        // --- GRUPOS ---

        [HttpGet]
        public async Task<IActionResult> Grupos()
        {
            List<ML.Grupo> grupos = new List<ML.Grupo>();
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/Grupo");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ML.Result>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (result != null && result.Correct && result.Objects != null)
                {
                    foreach (var obj in result.Objects)
                    {
                        var json = JsonSerializer.Serialize(obj);
                        var grupo = JsonSerializer.Deserialize<ML.Grupo>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        if (grupo != null) grupos.Add(grupo);
                    }
                }
            }
            return View(grupos);
        }

        [HttpGet]
        public async Task<IActionResult> GrupoForm(int? idGrupo)
        {
            ML.Grupo grupo = new ML.Grupo();
            if (idGrupo.HasValue && idGrupo.Value > 0)
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/Grupo/{idGrupo.Value}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<ML.Result>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (result != null && result.Correct && result.Object != null)
                    {
                        var json = JsonSerializer.Serialize(result.Object);
                        grupo = JsonSerializer.Deserialize<ML.Grupo>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new ML.Grupo();
                    }
                }
            }
            return View(grupo);
        }

        [HttpPost]
        public async Task<IActionResult> GrupoForm(ML.Grupo grupo)
        {
            var content = new StringContent(JsonSerializer.Serialize(grupo), Encoding.UTF8, "application/json");
            HttpResponseMessage response;

            if (grupo.IdGrupo > 0)
            {
                response = await _httpClient.PutAsync($"{_apiBaseUrl}/Grupo", content);
            }
            else
            {
                response = await _httpClient.PostAsync($"{_apiBaseUrl}/Grupo", content);
            }

            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "Grupo guardado correctamente.";
                return RedirectToAction("Grupos");
            }
            
            ViewBag.Message = "Ocurrió un error al guardar el grupo.";
            return View(grupo);
        }

        [HttpGet]
        public async Task<IActionResult> GrupoDelete(int idGrupo)
        {
            var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}/Grupo/{idGrupo}");
            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "Grupo eliminado correctamente.";
            }
            else
            {
                TempData["Message"] = "Ocurrió un error al eliminar el grupo.";
            }
            return RedirectToAction("Grupos");
        }


        // --- ESPECIALIDADES ---

        [HttpGet]
        public async Task<IActionResult> Especialidades()
        {
            List<ML.Especialidad> especialidades = new List<ML.Especialidad>();
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/Especialidad");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ML.Result>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (result != null && result.Correct && result.Objects != null)
                {
                    foreach (var obj in result.Objects)
                    {
                        var json = JsonSerializer.Serialize(obj);
                        var especialidad = JsonSerializer.Deserialize<ML.Especialidad>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        if (especialidad != null) especialidades.Add(especialidad);
                    }
                }
            }
            return View(especialidades);
        }

        [HttpGet]
        public async Task<IActionResult> EspecialidadForm(int? idEspecialidad)
        {
            ML.Especialidad especialidad = new ML.Especialidad();
            if (idEspecialidad.HasValue && idEspecialidad.Value > 0)
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/Especialidad/{idEspecialidad.Value}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<ML.Result>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (result != null && result.Correct && result.Object != null)
                    {
                        var json = JsonSerializer.Serialize(result.Object);
                        especialidad = JsonSerializer.Deserialize<ML.Especialidad>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new ML.Especialidad();
                    }
                }
            }
            return View(especialidad);
        }

        [HttpPost]
        public async Task<IActionResult> EspecialidadForm(ML.Especialidad especialidad)
        {
            var content = new StringContent(JsonSerializer.Serialize(especialidad), Encoding.UTF8, "application/json");
            HttpResponseMessage response;

            if (especialidad.IdEspecialidad > 0)
            {
                response = await _httpClient.PutAsync($"{_apiBaseUrl}/Especialidad", content);
            }
            else
            {
                response = await _httpClient.PostAsync($"{_apiBaseUrl}/Especialidad", content);
            }

            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "Especialidad guardada correctamente.";
                return RedirectToAction("Especialidades");
            }
            
            ViewBag.Message = "Ocurrió un error al guardar la especialidad.";
            return View(especialidad);
        }

        [HttpGet]
        public async Task<IActionResult> EspecialidadDelete(int idEspecialidad)
        {
            var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}/Especialidad/{idEspecialidad}");
            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "Especialidad eliminada correctamente.";
            }
            else
            {
                TempData["Message"] = "Ocurrió un error al eliminar la especialidad.";
            }
            return RedirectToAction("Especialidades");
        }
    }
}
