using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EvaluacionParcial1.Data;
using EvaluacionParcial1.Models;

namespace EvaluacionParcial1.Controllers
{
    public class MiembrosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MiembrosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Miembros
        public async Task<IActionResult> Index()
        {
            var miembros = await _context.Miembros
                .Include(m => m.Prestamos)
                    .ThenInclude(p => p.Libro)
                .OrderBy(m => m.Apellido)
                .ThenBy(m => m.Nombre)
                .ToListAsync();

            return View(miembros);
        }

        // GET: Miembros/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var miembro = await _context.Miembros
                .Include(m => m.Prestamos)
                    .ThenInclude(p => p.Libro)
                .FirstOrDefaultAsync(m => m.MiembroId == id);

            if (miembro == null)
            {
                return NotFound();
            }

            return View(miembro);
        }

        // GET: Miembros/Create
        public IActionResult Create()
        {
            return View(new Miembro { FechaSuscripcion = DateTime.Today });
        }

        // POST: Miembros/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MiembroId,Nombre,Apellido,Email,FechaSuscripcion")] Miembro miembro)
        {
            ModelState.Remove(nameof(Miembro.Prestamos));

            if (ModelState.IsValid)
            {
                _context.Add(miembro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(miembro);
        }

        // GET: Miembros/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var miembro = await _context.Miembros.FindAsync(id);
            if (miembro == null)
            {
                return NotFound();
            }

            return View(miembro);
        }

        // POST: Miembros/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MiembroId,Nombre,Apellido,Email,FechaSuscripcion")] Miembro miembro)
        {
            if (id != miembro.MiembroId)
            {
                return NotFound();
            }

            ModelState.Remove(nameof(Miembro.Prestamos));

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(miembro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MiembroExists(miembro.MiembroId))
                    {
                        return NotFound();
                    }
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(miembro);
        }

        // POST: Miembros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var miembro = await _context.Miembros.FindAsync(id);
            if (miembro != null)
            {
                _context.Miembros.Remove(miembro);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool MiembroExists(int id)
        {
            return _context.Miembros.Any(e => e.MiembroId == id);
        }
    }
}