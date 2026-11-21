using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EvaluacionParcial1.Models
{
    public class Prestamo
    {
        [Key]
        public int PrestamoId { get; set; }

        [ForeignKey(nameof(Libro))]
        public int LibroId { get; set; }
        public Libro Libro { get; set; } = null!;

        [ForeignKey(nameof(Miembro))]
        public int MiembroId { get; set; }
        public Miembro Miembro { get; set; } = null!;

        [Required]
        public DateTime FechaPrestamo { get; set; } = DateTime.Today;

        public DateTime? FechaDevolucion { get; set; }

        [Required, StringLength(30)]
        public string Estado { get; set; } = "Activo";
    }
}
