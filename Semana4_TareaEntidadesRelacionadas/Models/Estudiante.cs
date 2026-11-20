using System.ComponentModel.DataAnnotations;
namespace Semana4_TareaEntidadesRelacionadas.Models
{
    public class Estudiante
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombres { get; set; }

        [Required]
        [StringLength(50)]
        public string Apellidos { get; set; }

        [Required]
        [StringLength(10)]
        public string Cedula { get; set; }

        [Required]
        [EmailAddress]
        public string Correo { get; set; }

        public ICollection<Matricula> Matriculas { get; set; }
    }
}
