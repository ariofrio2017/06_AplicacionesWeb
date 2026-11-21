using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EvaluacionParcial1.Data;

namespace EvaluacionParcial1.Controllers
{
    public class LibroAutoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LibroAutoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LibroAutores
        public async Task<IActionResult> Index()
        {
            var relaciones = await _context.LibroAutores
                .Include(la => la.Libro)
                .Include(la => la.Autor)
                .OrderBy(la => la.Libro.Titulo)
                .ThenBy(la => la.OrdenAutoria)
                .ToListAsync();

            return View(relaciones);
        }
    }
}