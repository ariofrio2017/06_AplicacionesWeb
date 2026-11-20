using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Semana4_TareaEntidadesRelacionadas.Models
{
    public class Carrera
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        public int DuracionAnios { get; set; }

        [Required]
        [StringLength(50)]
        public string Modalidad { get; set; }

        [Required]
        [StringLength(50)]
        public string Jornada { get; set; }

        // Relación con Facultad
        [ForeignKey("Facultad")]
        public int FacultadId { get; set; }
        public Facultad Facultad { get; set; }

        public ICollection<Curso> Cursos { get; set; }
    }
}
