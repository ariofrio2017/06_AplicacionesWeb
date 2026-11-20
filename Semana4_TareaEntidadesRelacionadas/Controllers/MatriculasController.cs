using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Semana4_TareaEntidadesRelacionadas.Data;
using Semana4_TareaEntidadesRelacionadas.Models;

public class MatriculasController : Controller
{
    private readonly ApplicationDbContext _context;

    public MatriculasController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: MATRICULAS
    public async Task<IActionResult> Index()
    {
        var matriculas = _context.Matriculas
            .Include(m => m.Curso)
            .Include(m => m.Estudiante);

        return View(await matriculas.ToListAsync());
    }

    // GET: MATRICULAS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var matricula = await _context.Matriculas
            .Include(m => m.Curso)
            .Include(m => m.Estudiante)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (matricula == null)
        {
            return NotFound();
        }

        return View(matricula);
    }

    // GET: MATRICULAS/Create
    public IActionResult Create()
    {
        CargarSelectLists();
        return View();
    }

    // POST: MATRICULAS/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,FechaInscripcion,Estado,CostoMatricula,Observaciones,CursoId,EstudianteId")] Matricula matricula)
    {
        if (ModelState.IsValid)
        {
            _context.Add(matricula);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        CargarSelectLists(matricula.CursoId);
        return View(matricula);
    }

    // GET: MATRICULAS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var matricula = await _context.Matriculas.FindAsync(id);
        if (matricula == null)
        {
            return NotFound();
        }

        CargarSelectLists(matricula.CursoId);
        return View(matricula);
    }

    // POST: MATRICULAS/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,FechaInscripcion,Estado,CostoMatricula,Observaciones,CursoId,EstudianteId")] Matricula matricula)
    {
        if (id != matricula.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(matricula);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MatriculaExists(matricula.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        CargarSelectLists(matricula.CursoId);
        return View(matricula);
    }

    // GET: MATRICULAS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var matricula = await _context.Matriculas
            .Include(m => m.Curso)
            .Include(m => m.Estudiante)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (matricula == null)
        {
            return NotFound();
        }

        return View(matricula);
    }

    // POST: MATRICULAS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var matricula = await _context.Matriculas.FindAsync(id);
        if (matricula != null)
        {
            _context.Matriculas.Remove(matricula);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // Facultad -> Carreras
    [HttpGet]
    public JsonResult ObtenerCarrerasPorFacultad(int facultadId)
    {
        var carreras = _context.Carreras
            .Where(c => c.FacultadId == facultadId)
            .OrderBy(c => c.Nombre)
            .Select(c => new
            {
                id = c.Id,
                nombre = c.Nombre
            })
            .ToList();

        return Json(carreras);
    }

    //  Carrera -> Cursos
    [HttpGet]
    public JsonResult ObtenerCursosPorCarrera(int carreraId)
    {
        var cursos = _context.Cursos
            .Where(c => c.CarreraId == carreraId)
            .OrderBy(c => c.NombreCurso)
            .Select(c => new
            {
                id = c.Id,
                nombre = c.NombreCurso
            })
            .ToList();

        return Json(cursos);
    }

    private void CargarSelectLists(int? cursoIdSeleccionado = null)
    {
        int? carreraSeleccionadaId = null;
        int? facultadSeleccionadaId = null;

        if (cursoIdSeleccionado.HasValue)
        {
            var curso = _context.Cursos
                .Include(c => c.Carrera)
                .FirstOrDefault(c => c.Id == cursoIdSeleccionado.Value);

            if (curso != null)
            {
                carreraSeleccionadaId = curso.CarreraId;
                facultadSeleccionadaId = curso.Carrera?.FacultadId;
            }
        }

        ViewData["FacultadId"] = new SelectList(
            _context.Facultades.OrderBy(f => f.Nombre),
            "Id",
            "Nombre",
            facultadSeleccionadaId
        );

        ViewData["CarreraId"] = facultadSeleccionadaId.HasValue
            ? new SelectList(
                _context.Carreras
                    .Where(c => c.FacultadId == facultadSeleccionadaId.Value)
                    .OrderBy(c => c.Nombre),
                "Id",
                "Nombre",
                carreraSeleccionadaId)
            : new SelectList(Enumerable.Empty<SelectListItem>());

        ViewData["CursoId"] = carreraSeleccionadaId.HasValue
            ? new SelectList(
                _context.Cursos
                    .Where(c => c.CarreraId == carreraSeleccionadaId.Value)
                    .OrderBy(c => c.NombreCurso),
                "Id",
                "NombreCurso",
                cursoIdSeleccionado)
            : new SelectList(Enumerable.Empty<SelectListItem>());

        var estudiantes = _context.Estudiantes
            .OrderBy(e => e.Apellidos)
            .ThenBy(e => e.Nombres)
            .Select(e => new
            {
                e.Id,
                NombreCompleto = e.Nombres + " " + e.Apellidos
            })
            .ToList();

        ViewData["EstudianteId"] = new SelectList(estudiantes, "Id", "NombreCompleto");
    }

    private bool MatriculaExists(int id)
    {
        return _context.Matriculas.Any(e => e.Id == id);
    }
}