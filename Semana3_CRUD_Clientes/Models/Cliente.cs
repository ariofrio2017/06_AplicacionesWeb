using System;
using System.ComponentModel.DataAnnotations;
namespace Semana3_CRUD_Clientes.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50)]
        public string? Nombres { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(50)]
        public string? Apellidos { get; set; }

        [Required(ErrorMessage = "La cédula o RUC es obligatoria.")]
        [StringLength(13, MinimumLength = 10, ErrorMessage = "La cédula debe tener entre 10 y 13 dígitos.")]
        public string? CedulaRuc { get; set; }

        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        [StringLength(100)]
        public string? Correo { get; set; }

        [Phone(ErrorMessage = "El número de teléfono no es válido.")]
        [StringLength(20)]
        public string? Telefono { get; set; }

        [StringLength(150)]
        public string? Direccion { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        [StringLength(50)]
        public string? Ciudad { get; set; }

        [Display(Name = "Estado Activo")]
        public bool Activo { get; set; } = true;
    }
}
