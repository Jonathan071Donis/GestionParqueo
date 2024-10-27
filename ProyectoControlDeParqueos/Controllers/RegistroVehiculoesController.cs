using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoControlDeParqueos.Models;

namespace ProyectoControlDeParqueos.Controllers
{
    public class RegistroVehiculoesController : Controller
    {
        private readonly LoginDbContext _context;

        public RegistroVehiculoesController(LoginDbContext context)
        {
            _context = context;
        }

        // GET: RegistroVehiculoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.RegistroVehiculos.ToListAsync());
        }

        // GET: RegistroVehiculoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registroVehiculo = await _context.RegistroVehiculos
                .FirstOrDefaultAsync(m => m.idRegistroVehiculo == id);
            if (registroVehiculo == null)
            {
                return NotFound();
            }

            return View(registroVehiculo);
        }

        // GET: RegistroVehiculoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RegistroVehiculoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("idRegistroVehiculo,placa,marca,modelo,color,fechaIngreso,estado")] RegistroVehiculo registroVehiculo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(registroVehiculo);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Registro creado con exito."; // Mensaje de éxito
                return RedirectToAction(nameof(Index));
            }
            return View(registroVehiculo);
        }

        // GET: RegistroVehiculoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registroVehiculo = await _context.RegistroVehiculos.FindAsync(id);
            if (registroVehiculo == null)
            {
                return NotFound();
            }
            return View(registroVehiculo);
        }

        // POST: RegistroVehiculoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("idRegistroVehiculo,placa,marca,modelo,color,fechaIngreso,estado")] RegistroVehiculo registroVehiculo)
        {
            if (id != registroVehiculo.idRegistroVehiculo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registroVehiculo);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Registro editado con exito."; // Mensaje de éxito
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistroVehiculoExists(registroVehiculo.idRegistroVehiculo))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(registroVehiculo);
        }

        // GET: RegistroVehiculoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registroVehiculo = await _context.RegistroVehiculos
                .FirstOrDefaultAsync(m => m.idRegistroVehiculo == id);
            if (registroVehiculo == null)
            {
                return NotFound();
            }

            return View(registroVehiculo);
        }

        // POST: RegistroVehiculoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var registroVehiculo = await _context.RegistroVehiculos.FindAsync(id);
            if (registroVehiculo != null)
            {
                _context.RegistroVehiculos.Remove(registroVehiculo);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Registro eliminado con exito."; // Mensaje de éxito
            }
            return RedirectToAction(nameof(Index));
        }

        private bool RegistroVehiculoExists(int id)
        {
            return _context.RegistroVehiculos.Any(e => e.idRegistroVehiculo == id);
        }
    }
}
