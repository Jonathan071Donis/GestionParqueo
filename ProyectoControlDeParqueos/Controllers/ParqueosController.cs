using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoControlDeParqueos.Models;

namespace ProyectoControlDeParqueos.Controllers
{
    public class ParqueosController : Controller
    {
        private readonly LoginDbContext _context;

        public ParqueosController(LoginDbContext context)
        {
            _context = context;
        }

        // GET: Parqueos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Parqueos.ToListAsync());
        }

        // GET: Parqueos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parqueo = await _context.Parqueos
                .FirstOrDefaultAsync(m => m.idParqueo == id);
            if (parqueo == null)
            {
                return NotFound();
            }

            return View(parqueo);
        }

        // GET: Parqueos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Parqueos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("idParqueo,nombreParqueo,ubicacion,capacidadTotal,espaciosDisponibles,estado")] Parqueo parqueo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(parqueo);
                await _context.SaveChangesAsync();
                // Alert for successful creation
                TempData["SuccessMessage"] = "Parqueo creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(parqueo);
        }

        // GET: Parqueos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parqueo = await _context.Parqueos.FindAsync(id);
            if (parqueo == null)
            {
                return NotFound();
            }
            return View(parqueo);
        }

        // POST: Parqueos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("idParqueo,nombreParqueo,ubicacion,capacidadTotal,espaciosDisponibles,estado")] Parqueo parqueo)
        {
            if (id != parqueo.idParqueo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parqueo);
                    await _context.SaveChangesAsync();
                    // Alert for successful edit
                    TempData["SuccessMessage"] = "Parqueo editado exitosamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParqueoExists(parqueo.idParqueo))
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
            return View(parqueo);
        }

        // GET: Parqueos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parqueo = await _context.Parqueos
                .FirstOrDefaultAsync(m => m.idParqueo == id);
            if (parqueo == null)
            {
                return NotFound();
            }

            return View(parqueo);
        }

        // POST: Parqueos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parqueo = await _context.Parqueos.FindAsync(id);
            if (parqueo != null)
            {
                _context.Parqueos.Remove(parqueo);
                await _context.SaveChangesAsync();
                // Alert for successful deletion
                TempData["SuccessMessage"] = "Parqueo eliminado exitosamente.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ParqueoExists(int id)
        {
            return _context.Parqueos.Any(e => e.idParqueo == id);
        }
    }
}
