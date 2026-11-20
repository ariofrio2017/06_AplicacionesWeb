
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Semana4_TareaEntidadesRelacionadas.Data;
using Semana4_TareaEntidadesRelacionadas.Models;

public class CarrerasController : Controller
{
    private readonly ApplicationDbContext _context;

    public CarrerasController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Carreras
    public async Task<IActionResult> Index()
    {
        var carreras = _context.Carreras.Include(c => c.Facultad);
        return View(await carreras.ToListAsync());
    }

    // GET: Carreras/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var carrera = await _context.Carreras
            .Include(c => c.Facultad)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (carrera == null)
        {
            return NotFound();
        }

        return View(carrera);
    }

    // GET: Carreras/Create
    public IActionResult Create()
    {
        ViewData["FacultadId"] = new SelectList(_context.Facultades, "Id", "Nombre");
        return View();
    }

    // POST: Carreras/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nombre,DuracionAnios,Modalidad,Jornada,FacultadId")] Carrera carrera)
    {
        ModelState.Remove(nameof(Carrera.Facultad));
        ModelState.Remove(nameof(Carrera.Cursos));

        if (ModelState.IsValid)
        {
            _context.Add(carrera);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["FacultadId"] = new SelectList(_context.Facultades, "Id", "Nombre", carrera.FacultadId);
        return View(carrera);
    }

    // GET: Carreras/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var carrera = await _context.Carreras.FindAsync(id);
        if (carrera == null)
        {
            return NotFound();
        }

        ViewData["FacultadId"] = new SelectList(_context.Facultades, "Id", "Nombre", carrera.FacultadId);
        return View(carrera);
    }

    // POST: Carreras/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,DuracionAnios,Modalidad,Jornada,FacultadId")] Carrera carrera)
    {
        if (id != carrera.Id)
        {
            return NotFound();
        }

        ModelState.Remove(nameof(Carrera.Facultad));
        ModelState.Remove(nameof(Carrera.Cursos));

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(carrera);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarreraExists(carrera.Id))
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

        ViewData["FacultadId"] = new SelectList(_context.Facultades, "Id", "Nombre", carrera.FacultadId);
        return View(carrera);
    }

    // GET: Carreras/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var carrera = await _context.Carreras
            .Include(c => c.Facultad)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (carrera == null)
        {
            return NotFound();
        }

        return View(carrera);
    }

    // POST: Carreras/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var carrera = await _context.Carreras.FindAsync(id);
        if (carrera != null)
        {
            _context.Carreras.Remove(carrera);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CarreraExists(int id)
    {
        return _context.Carreras.Any(e => e.Id == id);
    }
}