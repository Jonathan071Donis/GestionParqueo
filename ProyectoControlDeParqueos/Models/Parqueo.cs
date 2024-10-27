using System.ComponentModel.DataAnnotations;

namespace ProyectoControlDeParqueos.Models
{
    public class Parqueo
    {
        [Key]
        public int idParqueo { get; set; }

        [Display(Name = "Nombre del parqueo")]
        [Required(ErrorMessage = "El nombre del parqueo es requerido.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Solo se permiten letras y espacios.")]
        public string? nombreParqueo { get; set; }

        [Display(Name = "Ubicación")]
        [Required(ErrorMessage = "La ubicación es requerida.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Solo se permiten letras y espacios.")]
        public string? ubicacion { get; set; }

        [Display(Name = "Capacidad total")]
        [Required(ErrorMessage = "La capacidad total es requerida.")]
        [Range(1, int.MaxValue, ErrorMessage = "La capacidad total debe ser un número positivo.")]
        public int capacidadTotal { get; set; }

        [Display(Name = "Espacios disponibles")]
        [Required(ErrorMessage = "Los espacios disponibles son requeridos.")]
        [Range(0, int.MaxValue, ErrorMessage = "Los espacios disponibles no pueden ser negativos.")]
        public int espaciosDisponibles { get; set; }

        [Display(Name = "Estado")]
        public bool estado { get; set; }

        // Reporte - Porcentaje de ocupación
        public decimal ReporteOcupacion()
        {
            if (capacidadTotal > 0)
            {
                return (decimal)(capacidadTotal - espaciosDisponibles) / capacidadTotal * 100;
            }
            return 0;
        }
    }
}
