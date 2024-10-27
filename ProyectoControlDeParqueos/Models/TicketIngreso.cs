using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoControlDeParqueos.Models
{
    public class TicketIngreso
    {
        [Key]
        public int idTicketIngreso { get; set; }

        [ForeignKey("ClienteId")]
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        [ForeignKey("RegistroVehiculoId")]
        public int RegistroVehiculoId { get; set; }
        public RegistroVehiculo? RegistroVehiculo { get; set; }

        [ForeignKey("Parqueo")]
        public int idParqueo { get; set; }
        public Parqueo? Parqueo { get; set; }

        [Display(Name = "Fecha de ingreso")]
        [DataType(DataType.DateTime)]
        public DateTime fechaIngreso { get; set; }

        [Display(Name = "Estado")]
        public bool estado { get; set; }
    }
}
