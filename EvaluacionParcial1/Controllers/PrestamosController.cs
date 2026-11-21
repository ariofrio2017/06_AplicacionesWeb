using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EvaluacionParcial1.Data;
using EvaluacionParcial1.Models;

namespace EvaluacionParcial1.Controllers
{
    public class PrestamosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PrestamosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Prestamos
        public async Task<IActionResult> Index()
        {
            var prestamos = await _context.Prestamos
                .Include(p => p.Libro)
                .Include(p => p.Miembro)
                .OrderByDescending(p => p.FechaPrestamo)
                .ToListAsync();

            return View(prestamos);
        }

        // GET: Prestamos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prestamo = await _context.Prestamos
                .Include(p => p.Libro)
                .Include(p => p.Miembro)
                .FirstOrDefaultAsync(m => m.PrestamoId == id);

            if (prestamo == null)
            {
                return NotFound();
            }

            return View(prestamo);
        }

        // GET: Prestamos/Create
        public async Task<IActionResult> Create()
        {
            await CargarSelectListsAsync();
            var modeloInicial = new Prestamo
            {
                FechaPrestamo = DateTime.Today,
                Estado = "Activo"
            };
            return View(modeloInicial);
        }

        // POST: Prestamos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("PrestamoId,LibroId,MiembroId,FechaPrestamo,FechaDevolucion,Estado")] Prestamo prestamo)
        {
            ModelState.Remove(nameof(Prestamo.Libro));
            ModelState.Remove(nameof(Prestamo.Miembro));

            if (prestamo.FechaDevolucion.HasValue && prestamo.FechaDevolucion.Value.Date < prestamo.FechaPrestamo.Date)
            {
                ModelState.AddModelError(nameof(Prestamo.FechaDevolucion),
                    "La fecha de devolución no puede ser anterior a la fecha de préstamo.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(prestamo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await CargarSelectListsAsync(prestamo.LibroId, prestamo.MiembroId, prestamo.Estado);
            return View(prestamo);
        }

        // GET: Prestamos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo == null)
            {
                return NotFound();
            }

            await CargarSelectListsAsync(prestamo.LibroId, prestamo.MiembroId, prestamo.Estado);
            return View(prestamo);
        }

        // POST: Prestamos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("PrestamoId,LibroId,MiembroId,FechaPrestamo,FechaDevolucion,Estado")] Prestamo prestamo)
        {
            if (id != prestamo.PrestamoId)
            {
                return NotFound();
            }

            ModelState.Remove(nameof(Prestamo.Libro));
            ModelState.Remove(nameof(Prestamo.Miembro));

            if (prestamo.FechaDevolucion.HasValue && prestamo.FechaDevolucion.Value.Date < prestamo.FechaPrestamo.Date)
            {
                ModelState.AddModelError(nameof(Prestamo.FechaDevolucion),
                    "La fecha de devolución no puede ser anterior a la fecha de préstamo.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prestamo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrestamoExists(prestamo.PrestamoId))
                    {
                        return NotFound();
                    }
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            await CargarSelectListsAsync(prestamo.LibroId, prestamo.MiembroId, prestamo.Estado);
            return View(prestamo);
        }

        // POST: Prestamos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo != null)
            {
                _context.Prestamos.Remove(prestamo);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PrestamoExists(int id)
        {
            return _context.Prestamos.Any(e => e.PrestamoId == id);
        }

        private async Task CargarSelectListsAsync(int? libroIdSeleccionado = null, int? miembroIdSeleccionado = null, string? estadoSeleccionado = null)
        {
            var libros = await _context.Libros
                .OrderBy(l => l.Titulo)
                .Select(l => new
                {
                    l.LibroId,
                    TituloMostrar = $"{l.Titulo} ({l.AnioPublicacion})"
                })
                .ToListAsync();

            var miembros = await _context.Miembros
                .OrderBy(m => m.Apellido)
                .ThenBy(m => m.Nombre)
                .Select(m => new
                {
                    m.MiembroId,
                    NombreCompleto = $"{m.Nombre} {m.Apellido}"
                })
                .ToListAsync();

            var estados = new[] { "Activo", "Devuelto", "Vencido" };

            ViewData["LibroId"] = new SelectList(libros, "LibroId", "TituloMostrar", libroIdSeleccionado);
            ViewData["MiembroId"] = new SelectList(miembros, "MiembroId", "NombreCompleto", miembroIdSeleccionado);
            ViewData["Estados"] = new SelectList(estados, estadoSeleccionado ?? "Activo");
        }
    }
}