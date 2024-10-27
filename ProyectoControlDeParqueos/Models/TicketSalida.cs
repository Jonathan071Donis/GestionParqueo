using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoControlDeParqueos.Models
{
    public class TicketSalida
    {
        [Key]
        public int idTicketSalida { get; set; }

        [Display(Name = "Cliente")]
        [ForeignKey("ClienteId")]
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        [Display(Name = "Registro Vehículo")]
        [ForeignKey("RegistroVehiculoId")]
        public int RegistroVehiculoId { get; set; }
        public RegistroVehiculo? RegistroVehiculo { get; set; }

        [Display(Name = "Parqueo")]
        [ForeignKey("ParqueoId")]
        public int ParqueoId { get; set; }
        public Parqueo? Parqueo { get; set; }

        [Display(Name = "Tarifa")]
        [ForeignKey("TarifaId")]
        public int TarifaId { get; set; }
        public Tarifa? Tarifa { get; set; }

        [Display(Name = "Fecha de salida")]
        [DataType(DataType.DateTime)]
        public DateTime fechaSalida { get; set; }


        [Display(Name = "Cantidad")]
        public int cantidad { get; set; }

        [Display(Name = "Costo total")]
        [Precision(18, 2)]
        public decimal costoTotal { get; set; }

       


        [Display(Name = "Estado")]
        public bool estado { get; set; }

      
    }
}

