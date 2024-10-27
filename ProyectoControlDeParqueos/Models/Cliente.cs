using System.ComponentModel.DataAnnotations;

namespace ProyectoControlDeParqueos.Models
{
    public class Cliente
    {
        [Key]
        public int idCliente { get; set; }

        [Display(Name = "Nombre Completo")]
        [Required(ErrorMessage = "El nombre completo es requerido.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Sólo se permiten letras.")]
        public string? nombreCompleto { get; set; }

        [Display(Name = "Número de teléfono")]
        [Required(ErrorMessage = "El número de teléfono es requerido.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Sólo se permiten dígitos.")]
        public string? telefono { get; set; }

        [Display(Name = "Correo Electrónico")]
        [Required(ErrorMessage = "El correo electrónico es requerido.")]
        [EmailAddress(ErrorMessage = "Formato de correo electrónico no válido.")]
        public string? correoElectronico { get; set; }

        [Display(Name = "Estado")]
        public bool estado { get; set; }
    }
}
