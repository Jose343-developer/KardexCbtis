using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PL_MVC.Models;
using PL_MVC.Filters;

namespace PL_MVC.Controllers;

[AuthorizeRole("Administrador", "Profesor", "Alumno")]
public class HomeController : Controller
{
    private readonly BL.Alumno _alumnoBL;

    public HomeController(BL.Alumno alumnoBL)
    {
        _alumnoBL = alumnoBL;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var alumnoCount = _alumnoBL;
        ML.Result result = alumnoCount.CountAlumno();
        
        int[] countSemestre = new int[7];
        for (int i = 1; i <= 6; i++)
        {
            ML.Result resultSemestre = alumnoCount.GetCountAlumnoBySemestre(i);
            countSemestre[i] = resultSemestre.Correct ? (int)resultSemestre.Object : 0;
        }

        ML.HomeViewModel model = new ML.HomeViewModel
        {
            AlumnosCount = result.Correct ? (int)result.Object : 0,
            PrimerSemestre = countSemestre[1],
            SegundoSemestre = countSemestre[2],
            TercerSemestre = countSemestre[3],
            CuartoSemestre = countSemestre[4],
            QuintoSemestre = countSemestre[5],
            SextoSemestre = countSemestre[6]
        };

        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
