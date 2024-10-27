using System.Collections.Generic;

namespace ProyectoControlDeParqueos.Models
{
    public class ReporteIngresos
    {
        public List<Tarifa> Tarifas { get; set; }
        public decimal TotalIngresos { get; set; }
    }
}
