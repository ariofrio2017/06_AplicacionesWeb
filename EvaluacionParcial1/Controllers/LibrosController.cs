using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EvaluacionParcial1.Data;
using EvaluacionParcial1.Models;

namespace EvaluacionParcial1.Controllers
{
    public class LibrosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LibrosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Libros
        public async Task<IActionResult> Index()
        {
            var libros = await _context.Libros
                .Include(l => l.LibroAutores)
                    .ThenInclude(la => la.Autor)
                .OrderBy(l => l.Titulo)
                .ToListAsync();

            return View(libros);
        }

        // GET: Libros/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .Include(l => l.LibroAutores)
                    .ThenInclude(la => la.Autor)
                .FirstOrDefaultAsync(m => m.LibroId == id);

            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // GET: Libros/Create
        public async Task<IActionResult> Create()
        {
            await CargarAutoresViewBag();
            return View();
        }

        // POST: Libros/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("LibroId,Titulo,Genero,AnioPublicacion")] Libro libro,
            int[] autoresSeleccionados)
        {
            autoresSeleccionados ??= Array.Empty<int>();

            if (autoresSeleccionados.Length == 0)
            {
                ModelState.AddModelError("autoresSeleccionados", "Debe seleccionar al menos un autor.");
            }

            // Evitamos validar colecciones de navegación vacías
            ModelState.Remove(nameof(Libro.LibroAutores));
            ModelState.Remove(nameof(Libro.Prestamos));

            if (ModelState.IsValid)
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    _context.Libros.Add(libro);
                    await _context.SaveChangesAsync();

                    for (int i = 0; i < autoresSeleccionados.Length; i++)
                    {
                        _context.LibroAutores.Add(new LibroAutor
                        {
                            LibroId = libro.LibroId,
                            AutorId = autoresSeleccionados[i],
                            OrdenAutoria = i + 1
                        });
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    await transaction.RollbackAsync();
                    ModelState.AddModelError(string.Empty, "Ocurrió un error al guardar el libro y sus autores.");
                }
            }

            await CargarAutoresViewBag(autoresSeleccionados);
            return View(libro);
        }

        // GET: Libros/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .Include(l => l.LibroAutores)
                .FirstOrDefaultAsync(l => l.LibroId == id);

            if (libro == null)
            {
                return NotFound();
            }

            var seleccionados = libro.LibroAutores
                .OrderBy(la => la.OrdenAutoria)
                .Select(la => la.AutorId)
                .ToList();

            await CargarAutoresViewBag(seleccionados);
            return View(libro);
        }

        // POST: Libros/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("LibroId,Titulo,Genero,AnioPublicacion")] Libro libro,
            int[] autoresSeleccionados)
        {
            if (id != libro.LibroId)
            {
                return NotFound();
            }

            autoresSeleccionados ??= Array.Empty<int>();

            if (autoresSeleccionados.Length == 0)
            {
                ModelState.AddModelError("autoresSeleccionados", "Debe seleccionar al menos un autor.");
            }

            ModelState.Remove(nameof(Libro.LibroAutores));
            ModelState.Remove(nameof(Libro.Prestamos));

            if (ModelState.IsValid)
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    _context.Update(libro);

                    var relacionesActuales = await _context.LibroAutores
                        .Where(la => la.LibroId == libro.LibroId)
                        .ToListAsync();

                    _context.LibroAutores.RemoveRange(relacionesActuales);

                    for (int i = 0; i < autoresSeleccionados.Length; i++)
                    {
                        _context.LibroAutores.Add(new LibroAutor
                        {
                            LibroId = libro.LibroId,
                            AutorId = autoresSeleccionados[i],
                            OrdenAutoria = i + 1
                        });
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    await transaction.RollbackAsync();
                    if (!LibroExists(libro.LibroId))
                    {
                        return NotFound();
                    }
                    throw;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    ModelState.AddModelError(string.Empty, "Ocurrió un error al actualizar el libro.");
                }
            }

            await CargarAutoresViewBag(autoresSeleccionados);
            return View(libro);
        }

        // GET: Libros/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .Include(l => l.LibroAutores)
                    .ThenInclude(la => la.Autor)
                .FirstOrDefaultAsync(m => m.LibroId == id);

            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }
        

        private bool LibroExists(int id)
        {
            return _context.Libros.Any(e => e.LibroId == id);
        }

        private async Task CargarAutoresViewBag(IEnumerable<int>? seleccionados = null)
        {
            var autores = await _context.Autores
                .OrderBy(a => a.Apellido)
                .ThenBy(a => a.Nombre)
                .Select(a => new
                {
                    a.AutorId,
                    NombreCompleto = a.Nombre + " " + a.Apellido
                })
                .ToListAsync();

            ViewBag.Autores = new MultiSelectList(autores, "AutorId", "NombreCompleto", seleccionados);
        }

        // POST: Libros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro != null)
            {
                _context.Libros.Remove(libro);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}