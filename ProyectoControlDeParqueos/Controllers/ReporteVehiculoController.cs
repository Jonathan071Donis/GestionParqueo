using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoControlDeParqueos.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoControlDeParqueos.Controllers
{
    public class ReporteVehiculoController : Controller
    {
        private readonly LoginDbContext _context;

        public ReporteVehiculoController(LoginDbContext context)
        {
            _context = context;
        }

        // GET: ReporteVehiculo/Index/{id}
        public async Task<IActionResult> Index(int id)
        {
            var vehiculo = await _context.RegistroVehiculos.FindAsync(id);
            var tarifa = await _context.Tarifas.FirstOrDefaultAsync(t => t.estado);

            if (vehiculo == null || tarifa == null)
            {
                return NotFound();
            }

            var reporte = new ReporteVehiculo
            {
                Placa = vehiculo.placa,
                Marca = vehiculo.marca,
                Modelo = vehiculo.modelo,
                Color = vehiculo.color,
                FechaIngreso = vehiculo.fechaIngreso,
                Estado = vehiculo.estado,
                CostoPorHora = tarifa.costoPorHora,
                CostoPorDia = tarifa.costoPorDia,
                CostoTotal = vehiculo.costoTotal
            };

            return View(reporte);
        }
    }
}
