using System.ComponentModel.DataAnnotations;
namespace Semana4_TareaEntidadesRelacionadas.Models
{
    public class Facultad
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Decano { get; set; }

        [Required]
        [StringLength(150)]
        public string Ubicacion { get; set; }

        [Required]
        [StringLength(20)]
        public string Telefono { get; set; }

        // 
        public ICollection<Carrera> Carreras { get; set; }
    }
}
