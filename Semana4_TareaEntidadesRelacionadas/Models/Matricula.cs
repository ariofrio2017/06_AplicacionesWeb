using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Semana4_TareaEntidadesRelacionadas.Models
{
    public class Matricula
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime FechaInscripcion { get; set; }

        [Required]
        [StringLength(30)]
        public string Estado { get; set; }

        [Required]
        public decimal CostoMatricula { get; set; }

        [Required]
        [StringLength(100)]
        public string Observaciones { get; set; }

        // Relaciones
        [ForeignKey("Curso")]
        public int CursoId { get; set; }
        public Curso Curso { get; set; }

        [ForeignKey("Estudiante")]
        public int EstudianteId { get; set; }
        public Estudiante Estudiante { get; set; }
    }
}
