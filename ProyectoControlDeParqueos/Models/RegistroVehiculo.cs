using System.ComponentModel.DataAnnotations;

namespace ProyectoControlDeParqueos.Models
{
    public class RegistroVehiculo
    {
        internal decimal costoTotal;

        [Key]
        public int idRegistroVehiculo { get; set; }

        [Display(Name = "Placa del vehículo")]
        [Required(ErrorMessage = "La placa del vehículo es requerida.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z0-9]+$", ErrorMessage = "La placa debe contener letras y números.")]
        public string? placa { get; set; }

        [Display(Name = "Marca")]
        [Required(ErrorMessage = "La marca es requerida.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "La marca solo puede contener letras y espacios.")]
        public string? marca { get; set; }

        [Display(Name = "Modelo")]
        [Required(ErrorMessage = "El modelo es requerido.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El modelo solo puede contener números.")]
        public string? modelo { get; set; }

        [Display(Name = "Color")]
        [Required(ErrorMessage = "El color es requerido.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El color solo puede contener letras y espacios.")]
        public string? color { get; set; }

        [Display(Name = "Fecha de ingreso")]
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "La fecha de ingreso es requerida.")]
        public DateTime fechaIngreso { get; set; }

        [Display(Name = "Estado")]
        public bool estado { get; set; }

        // Reporte - Total de vehículos ingresados hoy
        public int ReporteVehiculosIngresadosHoy()
        {
            // Lógica para contar vehículos ingresados hoy
            return 0; // Valor de ejemplo
        }
    }
}
