using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ProyectoControlDeParqueos.Models
{
    public class Tarifa
    {
        internal decimal cantidadVehiculos;

        [Key]
        public int idTarifa { get; set; }

        [Display(Name = "Descripción de la tarifa")]
        [Required(ErrorMessage = "La descripción de la tarifa es requerida.")]
        [StringLength(100, ErrorMessage = "La descripción no puede exceder los 100 caracteres.")]
        public string? descripcion { get; set; }

        [Display(Name = "Costo por hora")]
        [Required(ErrorMessage = "El costo por hora es requerido.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El costo por hora debe ser mayor que cero.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Solo se permiten números y hasta dos decimales y no son válidos números negativos.")]
        public decimal costoPorHora { get; set; }

        [Display(Name = "Costo por día")]
        [Required(ErrorMessage = "El costo por día es requerido.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El costo por día debe ser mayor que cero.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Solo se permiten números y hasta dos decimales y no son válidos números negativos.")]
        public decimal costoPorDia { get; set; }

        [Display(Name = "Estado")]
        public bool estado { get; set; }

        // Reporte - Tarifas activas
        public int ReporteTarifasActivas()
        {
            // Lógica para contar tarifas activas
            return 0; // Valor de ejemplo
        }
    }
}
