using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoControlDeParqueos.Models;

namespace ProyectoControlDeParqueos.Controllers
{
    public class TarifasController : Controller
    {
        private readonly LoginDbContext _context;

        public TarifasController(LoginDbContext context)
        {
            _context = context;
        }

        // GET: Tarifas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tarifas.ToListAsync());
        }

        // GET: Tarifas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarifa = await _context.Tarifas
                .FirstOrDefaultAsync(m => m.idTarifa == id);
            if (tarifa == null)
            {
                return NotFound();
            }

            return View(tarifa);
        }

        // GET: Tarifas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tarifas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("idTarifa,descripcion,costoPorHora,costoPorDia,estado")] Tarifa tarifa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tarifa);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Tarifa creada con exito."; // Mensaje de éxito
                return RedirectToAction(nameof(Index));
            }
            return View(tarifa);
        }

        // GET: Tarifas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarifa = await _context.Tarifas.FindAsync(id);
            if (tarifa == null)
            {
                return NotFound();
            }
            return View(tarifa);
        }

        // POST: Tarifas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("idTarifa,descripcion,costoPorHora,costoPorDia,estado")] Tarifa tarifa)
        {
            if (id != tarifa.idTarifa)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tarifa);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Tarifa editada con exito."; // Mensaje de éxito
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TarifaExists(tarifa.idTarifa))
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
            return View(tarifa);
        }

        // GET: Tarifas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarifa = await _context.Tarifas
                .FirstOrDefaultAsync(m => m.idTarifa == id);
            if (tarifa == null)
            {
                return NotFound();
            }

            return View(tarifa);
        }

        // POST: Tarifas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tarifa = await _context.Tarifas.FindAsync(id);
            if (tarifa != null)
            {
                _context.Tarifas.Remove(tarifa);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Tarifa eliminada con exito."; // Mensaje de éxito
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TarifaExists(int id)
        {
            return _context.Tarifas.Any(e => e.idTarifa == id);
        }
    }
}
