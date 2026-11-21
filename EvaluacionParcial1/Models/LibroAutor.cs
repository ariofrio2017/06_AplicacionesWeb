using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EvaluacionParcial1.Models
{
    public class LibroAutor
    {
        [Key]
        public int LibroAutorId { get; set; }

        [ForeignKey(nameof(Libro))]
        public int LibroId { get; set; }
        public Libro Libro { get; set; } = null!;

        [ForeignKey(nameof(Autor))]
        public int AutorId { get; set; }
        public Autor Autor { get; set; } = null!;

        public int OrdenAutoria { get; set; } = 1;
    }
}
