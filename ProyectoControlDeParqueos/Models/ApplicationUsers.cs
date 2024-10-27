using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ProyectoControlDeParqueos.Models
{
    public class ApplicationUsers : IdentityUser
    {
        //[StringLength(200)]
        //public string Direccion {  get; set; }
    }

    public static class DashboardData
    {
        public static int Ocupacion { get; set; } = 10; // Ejemplo de valor inicial
        public static int VehiculosIngresadosHoy { get; set; } = 5;
        public static int Cobros { get; set; } = 3;

        public static void ActualizarDatos(int ocupacion, int vehiculos, int cobros)
        {
            Ocupacion = ocupacion;
            VehiculosIngresadosHoy = vehiculos;
            Cobros = cobros;
        }
    }

}
