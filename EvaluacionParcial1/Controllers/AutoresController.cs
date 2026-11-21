using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EvaluacionParcial1.Data;
using EvaluacionParcial1.Models;

namespace EvaluacionParcial1.Controllers
{
    public class AutoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AutoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Autores
        public async Task<IActionResult> Index()
        {
            var autores = await _context.Autores
                .Include(a => a.LibroAutores)
                    .ThenInclude(la => la.Libro)
                .OrderBy(a => a.Apellido)
                .ThenBy(a => a.Nombre)
                .ToListAsync();

            return View(autores);
        }

        // GET: Autores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _context.Autores
                .Include(a => a.LibroAutores)
                    .ThenInclude(la => la.Libro)
                .FirstOrDefaultAsync(m => m.AutorId == id);

            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }

        // GET: Autores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Autores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AutorId,Nombre,Apellido,Nacionalidad")] Autor autor)
        {
            ModelState.Remove(nameof(Autor.LibroAutores));

            if (ModelState.IsValid)
            {
                _context.Add(autor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(autor);
        }

        // GET: Autores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _context.Autores.FindAsync(id);
            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }

        // POST: Autores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AutorId,Nombre,Apellido,Nacionalidad")] Autor autor)
        {
            if (id != autor.AutorId)
            {
                return NotFound();
            }

            ModelState.Remove(nameof(Autor.LibroAutores));

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(autor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AutorExists(autor.AutorId))
                    {
                        return NotFound();
                    }
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(autor);
        }

        // POST: Autores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var autor = await _context.Autores.FindAsync(id);
            if (autor != null)
            {
                _context.Autores.Remove(autor);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AutorExists(int id)
        {
            return _context.Autores.Any(e => e.AutorId == id);
        }
    }
}