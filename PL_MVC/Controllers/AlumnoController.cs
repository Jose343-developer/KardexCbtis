using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System.Data;
using System.IO;
using ExcelDataReader;
using PL_MVC.Filters;

namespace PL_MVC.Controllers
{
    [AuthorizeRole("Administrador")]
    public class AlumnoController : Controller
    {
        private readonly BL.Alumno _alumnoBL;
        private readonly BL.Grupo _grupoBL;
        private readonly BL.Especialidad _especialidadBL;

        public AlumnoController(BL.Alumno alumnoBL, BL.Grupo grupoBL, BL.Especialidad especialidadBL)
        {
            _alumnoBL = alumnoBL;
            _grupoBL = grupoBL;
            _especialidadBL = especialidadBL;
        }

        // GET: AlumnoController

        [HttpGet]
        public ActionResult GetAll()
        {
            var blAlumno = _alumnoBL;
            ML.Result result = blAlumno.GetAll(new ML.Alumno());
            
            List<ML.Alumno> alumnosList = new List<ML.Alumno>();

            if (result.Correct)
            {
                foreach (var obj in result.Objects)
                {
                    alumnosList.Add((ML.Alumno)obj);
                }
            }
            else
            {
                ViewBag.Message = result.ErrorMessage;
            }

            return View(alumnosList);
        }



     [HttpGet]
public ActionResult AlumnoAdd()
{
    ML.Alumno alumno = new ML.Alumno();
    alumno.Especialidad = new ML.Especialidad();
    alumno.Grupo = new ML.Grupo();

    var grupoBL = _grupoBL;
    var especialidadBL = _especialidadBL;

    // 1. Traemos los datos
    ML.Result resultEspecialidades = especialidadBL.EspecialidadGetAll();
    ML.Result resultSemestres = grupoBL.GetSemestres();
    ML.Result resultGruposCompletos = grupoBL.GrupoGetAll(); 

    // 2. TUS LÍNEAS MÁGICAS (Intactas, como las tenías originalmente)
    alumno.Especialidad.Especialidades = resultEspecialidades.Objects;
    alumno.Grupo.semestres = resultSemestres.Objects;

    // 3. SEGURIDAD SOLO PARA LA LISTA NUEVA (Para que no vuelva a salir la pantalla roja)
    if (resultGruposCompletos.Objects != null)
    {
        alumno.Grupo.grupos = resultGruposCompletos.Objects;
    }
    else
    {
        alumno.Grupo.grupos = new List<object>(); // Si falla, mandamos lista vacía solo aquí
    }

    return View(alumno);
}
      [HttpPost]
public ActionResult AlumnoAdd(ML.Alumno alumno)
{
    if (!ModelState.IsValid)
    {
        var especialidadBL = _especialidadBL;
        var grupoBL = _grupoBL;

        ML.Result resultEspecialidades = especialidadBL.EspecialidadGetAll();
        ML.Result resultSemestres = grupoBL.GetSemestres();
        ML.Result resultGruposCompletos = grupoBL.GrupoGetAll(); 
        
        if(alumno.Especialidad == null)
        {
             alumno.Especialidad = new ML.Especialidad();
        }
        alumno.Especialidad.Especialidades = resultEspecialidades.Objects;

        if(alumno.Grupo == null)
        {
             alumno.Grupo = new ML.Grupo();
        }
        alumno.Grupo.semestres = resultSemestres.Objects;
        
        if (resultGruposCompletos.Objects != null)
        {
            alumno.Grupo.grupos = resultGruposCompletos.Objects;
        }
        else
        {
            alumno.Grupo.grupos = new List<object>();
        }

        return View(alumno);
    }

    var alumnos = _alumnoBL;
    ML.Result result = alumnos.AlumnoAdd(alumno);
    
    if (result.Correct)
    {
        // ¡Éxito! Guardamos el mensaje y lo mandamos a la tabla principal
        TempData["Message"] = "Alumno añadido correctamente";
        return RedirectToAction("GetAll");
    }
    else
    {
        // ¡ERROR! (Ej. Matrícula duplicada, error de SQL, etc.)
        // La vista se va a recargar, así que le devolvemos TODAS sus listas (Cartas).

        var especialidadBL = _especialidadBL;
        var grupoBL = _grupoBL;

        ML.Result resultEspecialidades = especialidadBL.EspecialidadGetAll();
        ML.Result resultSemestres = grupoBL.GetSemestres();
        ML.Result resultGruposCompletos = grupoBL.GrupoGetAll(); 
        
        // 1. Rellenar Especialidades
        if(alumno.Especialidad == null)
        {
             alumno.Especialidad = new ML.Especialidad();
        }
        alumno.Especialidad.Especialidades = resultEspecialidades.Objects;

        // 2. Rellenar Grupos y Semestres para la cascada
        if(alumno.Grupo == null)
        {
             alumno.Grupo = new ML.Grupo();
        }
        alumno.Grupo.semestres = resultSemestres.Objects; // Tu línea mágica
        
        // Red de seguridad para la lista de cascada por si la BD falló feo
        if (resultGruposCompletos.Objects != null)
        {
            alumno.Grupo.grupos = resultGruposCompletos.Objects;
        }
        else
        {
            alumno.Grupo.grupos = new List<object>();
        }

        ViewBag.Message = "Error al insertar Alumno: " + result.ErrorMessage;
        return View(alumno);
    }
}


// 3. Arreglé un pequeño typo en el nombre (decía GetEspecialdiad)
// y le quité el parámetro que no usabas.
[HttpGet] 
public JsonResult GetEspecialidad()
{
    var especialidades = _especialidadBL;
    ML.Result result = especialidades.EspecialidadGetAll();
  
    return Json(result.Objects);
}

[HttpGet] 

        public JsonResult GetEspecialdiad (ML.Especialidad especialidad)
        {
            ML.Result result = new ML.Result();

            var especialidades = _especialidadBL;
            
           result = especialidades.EspecialidadGetAll();
  
    return Json(result.Objects);

           
        }

[HttpPost]
public ActionResult CargaMasiva(IFormFile archivoExcel)
{
    // 1. Validaciones iniciales
    if (archivoExcel == null || archivoExcel.Length == 0)
    {
        TempData["Message"] = "Por favor selecciona un archivo.";
        return RedirectToAction("GetAll");
    }

    if (Path.GetExtension(archivoExcel.FileName).ToLower() != ".xlsx")
    {
        TempData["Message"] = "Error: El archivo debe ser un Excel (.xlsx).";
        return RedirectToAction("GetAll");
    }

    // 2. Guardamos el archivo temporalmente
    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFiles");
    if (!Directory.Exists(uploadsFolder))
    {
        Directory.CreateDirectory(uploadsFolder);
    }

    string filePath = Path.Combine(uploadsFolder, archivoExcel.FileName);
    using (var stream = new FileStream(filePath, FileMode.Create))
    {
        archivoExcel.CopyTo(stream);
    }

    int filasInsertadas = 0;
    int filasConError = 0;

    try
    {
        // 3. OBLIGATORIO EN MAC/LINUX: Registrar la codificación de texto
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        // 4. Leemos el Excel usando ExcelDataReader
        using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                // Convertimos el Excel a un DataSet, indicando que la primera fila tiene los nombres de las columnas
                var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true
                    }
                });

                // Sacamos la primera hoja del Excel
                DataTable dt = result.Tables[0];
                var alumnoBL = _alumnoBL;

                // 5. El mismo foreach de siempre (Recorremos el DataTable)
                foreach (DataRow row in dt.Rows)
                {
                    try
                    {

                        
                        ML.Alumno alumnoNuevo = new ML.Alumno();
                        // Importante: Los nombres deben coincidir con tus columnas en Excel
                        alumnoNuevo.Matricula = row["Matricula"].ToString();
                        alumnoNuevo.Curp = row["Curp"].ToString();
                        alumnoNuevo.Nombre = row["Nombre"].ToString();
                        alumnoNuevo.ApellidoPaterno = row["ApellidoPaterno"].ToString();
                        alumnoNuevo.ApellidoMaterno = row["ApellidoMaterno"]?.ToString();
                        alumnoNuevo.Correo = row["Correo"]?.ToString();
                        alumnoNuevo.Telefono = row["Telefono"]?.ToString();
                        alumnoNuevo.Celular = row["Celular"]?.ToString();
                        
                        alumnoNuevo.Especialidad = new ML.Especialidad();
                        alumnoNuevo.Especialidad.IdEspecialidad = Convert.ToInt32(row["IdEspecialidad"]);

                        alumnoNuevo.Grupo = new ML.Grupo();
                        alumnoNuevo.Grupo.IdGrupo = Convert.ToInt32(row["IdGrupo"]);

                        // Mandamos a llamar tu Procedure
                        ML.Result resultInsert = alumnoBL.AlumnoAdd(alumnoNuevo);

                        if (resultInsert.Correct)
                            filasInsertadas++;
                        else
                            filasConError++;
                    }
                    catch (Exception)
                    {
                        filasConError++; // Si falta un dato o el formato es incorrecto
                    }
                }
            }
        }

        TempData["Message"] = $"Carga masiva finalizada. Éxitos: {filasInsertadas}. Errores: {filasConError}.";
    }
    catch (Exception ex)
    {
        TempData["Message"] = "Error al leer el Excel: " + ex.Message;
    }
    finally
    {
        // 6. Limpieza
        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }
    }

    return RedirectToAction("GetAll");
}

        [HttpGet]
        public ActionResult ChangeStatus(int idAlumno, string estatus)
        {
            var result = _alumnoBL.ChangeStatus(idAlumno, estatus);
            if (result.Correct)
            {
                TempData["Message"] = $"Estatus del alumno actualizado correctamente a: {estatus}";
            }
            else
            {
                TempData["ErrorMessage"] = "Error al actualizar estatus: " + result.ErrorMessage;
            }
            return RedirectToAction("GetAll");
        }
    }
}
