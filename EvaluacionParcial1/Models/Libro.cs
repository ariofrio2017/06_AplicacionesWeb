using System.ComponentModel.DataAnnotations;
namespace EvaluacionParcial1.Models
{
    public class Libro
    {
        [Key]
        public int LibroId { get; set; }

        [Required, StringLength(150)]
        public string Titulo { get; set; } = string.Empty;

        [Required, StringLength(80)]
        public string Genero { get; set; } = string.Empty;

        [Required]
        public int AnioPublicacion { get; set; }

        public ICollection<LibroAutor> LibroAutores { get; set; } = new List<LibroAutor>();
        public ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
    }
}
