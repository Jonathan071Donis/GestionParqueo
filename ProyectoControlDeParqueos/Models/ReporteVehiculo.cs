
using System;

namespace ProyectoControlDeParqueos.Models
{
    public class ReporteVehiculo
    {
        public string Placa { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Color { get; set; }
        public DateTime FechaIngreso { get; set; }
        public bool Estado { get; set; }
        public decimal CostoPorHora { get; set; }
        public decimal CostoPorDia { get; set; }
        public decimal CostoTotal { get; set; }
    }
}
