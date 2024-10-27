

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProyectoControlDeParqueos.Models
{
    public class LoginDbContext : IdentityDbContext<ApplicationUsers>

    {

        public LoginDbContext (DbContextOptions<LoginDbContext> options) : base (options) 
        
        { 
        }

        
		
        public DbSet<Cliente> clientes { get; set; }
        public DbSet<Parqueo> Parqueos { get; set; }
		public DbSet<RegistroVehiculo> RegistroVehiculos { get; set; }
		public DbSet<Tarifa> Tarifas { get; set; }
        public DbSet<TicketIngreso> ticketIngresos { get; set; }
        public DbSet<TicketSalida> ticketSalidas { get; set; }
    }
}
