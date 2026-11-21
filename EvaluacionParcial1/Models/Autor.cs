using System.ComponentModel.DataAnnotations;
namespace EvaluacionParcial1.Models
{
    public class Autor
    {
        [Key]
        public int AutorId { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Apellido { get; set; } = string.Empty;

        [StringLength(80)]
        public string? Nacionalidad { get; set; }

        public ICollection<LibroAutor> LibroAutores { get; set; } = new List<LibroAutor>();
    }
}
