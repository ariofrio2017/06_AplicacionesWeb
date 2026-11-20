using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Semana4_TareaEntidadesRelacionadas.Models
{
    public class Curso
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string NombreCurso { get; set; }

        [Required]
        public int Creditos { get; set; }

        [Required]
        [StringLength(20)]
        public string Semestre { get; set; }

        [Required]
        [StringLength(50)]
        public string Paralelo { get; set; }

        // Relación con Carrera
        [ForeignKey("Carrera")]
        public int CarreraId { get; set; }
        public Carrera Carrera { get; set; }

        public ICollection<Matricula> Matriculas { get; set; }
    }
}
