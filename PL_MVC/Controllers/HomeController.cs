using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PL_MVC.Models;

namespace PL_MVC.Controllers;

public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
       
            BL.Alumno alumnoCount = new BL.Alumno();
            ML.Result result = new ML.Result();

            result = alumnoCount.CountAlumno();
            int[] countSemestre =  new int[7];
            for (int i = 1; i <= 6; i++)
    {
        ML.Result resultSemestre = alumnoCount.GetCountAlumnoBySemestre(i);
        
        
        countSemestre[i] = resultSemestre.Correct ? (int)resultSemestre.Object : 0;
    }

    // 2. Mandamos al ViewBag
    ViewBag.primerSemestre = countSemestre[1];
    ViewBag.segundoSemestre = countSemestre[2]; 
    ViewBag.tercerSemestre = countSemestre[3];
    ViewBag.cuartoSemestre = countSemestre[4];
    ViewBag.quintoSemestre = countSemestre[5];
    ViewBag.sextoSemestre = countSemestre[6];
            if (result.Correct)
            {
                     ViewBag.alumnosCount = (int)result.Object;


            }
                else
                {
            
                    ViewBag.alumnosCount = 0;

                }
                    return View();
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
