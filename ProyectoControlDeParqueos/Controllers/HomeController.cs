using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoControlDeParqueos.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace ProyectoControlDeParqueos.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly LoginDbContext _context;

        public HomeController(ILogger<HomeController> logger, LoginDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public static class DashboardData
        {
            public static int Ocupacion { get; set; } = 0; // Valor inicial
            public static int VehiculosIngresadosHoy { get; set; } = 0; // Valor inicial
            public static int TarifasActivas { get; set; } = 0; // Valor inicial
            public static int ClientesActivos { get; set; } = 0; // Valor inicial

            public static void ActualizarDatos(int ocupacion, int vehiculos, int tarifas, int clientes)
            {
                Ocupacion = ocupacion;
                VehiculosIngresadosHoy = vehiculos;
                TarifasActivas = tarifas;
                ClientesActivos = clientes;
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ActualizarDashboard(int ocupacion, int vehiculos, int tarifas, int clientes)
        {
            // Actualizar los datos en memoria
            DashboardData.ActualizarDatos(ocupacion, vehiculos, tarifas, clientes);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboardData()
        {
            // Lógica para obtener los datos de la base de datos
            int ocupacion = await _context.Parqueos.SumAsync(p => p.espaciosDisponibles); // Ajustar según la lógica de ocupación
            int vehiculosIngresadosHoy = await _context.RegistroVehiculos.CountAsync(r => r.estado); ;
            int tarifasActivas = await _context.Tarifas.CountAsync(t => t.estado); // Cambia 'estado' según tu modelo
            int clientesActivos = await _context.clientes.CountAsync(c => c.estado); // Cambia 'estado' según tu modelo

            // Actualiza los datos en memoria
            DashboardData.ActualizarDatos(ocupacion, vehiculosIngresadosHoy, tarifasActivas, clientesActivos);

            var data = new
            {
                ocupacion = DashboardData.Ocupacion,
                vehiculosIngresadosHoy = DashboardData.VehiculosIngresadosHoy,
                tarifasActivas = DashboardData.TarifasActivas,
                clientesActivos = DashboardData.ClientesActivos
            };

            return Json(data);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
