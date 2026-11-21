using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EvaluacionParcial1.Models
{
    public class Miembro
    {
        [Key]
        public int MiembroId { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Apellido { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public DateTime FechaSuscripcion { get; set; } = DateTime.Today;

        public ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
    }
}
