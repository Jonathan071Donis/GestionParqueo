using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoControlDeParqueos.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace ProyectoControlDeParqueos.Controllers
{
    public class TicketIngresoController : Controller
    {
        private readonly LoginDbContext _context;

        public TicketIngresoController(LoginDbContext context)
        {
            _context = context;
        }

        // GET: TicketIngreso
        public async Task<IActionResult> Index()
        {
            var loginDbContext = _context.ticketIngresos.Include(t => t.Cliente).Include(t => t.Parqueo).Include(t => t.RegistroVehiculo);
            return View(await loginDbContext.ToListAsync());
        }

        // GET: TicketIngreso/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketIngreso = await _context.ticketIngresos
                .Include(t => t.Cliente)
                .Include(t => t.Parqueo)
                .Include(t => t.RegistroVehiculo)
                .FirstOrDefaultAsync(m => m.idTicketIngreso == id);
            if (ticketIngreso == null)
            {
                return NotFound();
            }

            return View(ticketIngreso);
        }

        // GET: TicketIngreso/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.clientes, "idCliente", "nombreCompleto");
            ViewData["idParqueo"] = new SelectList(_context.Parqueos, "idParqueo", "nombreParqueo");
            ViewData["RegistroVehiculoId"] = new SelectList(_context.RegistroVehiculos, "idRegistroVehiculo", "placa");
            return View();
        }

        // POST: TicketIngreso/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("idTicketIngreso,ClienteId,RegistroVehiculoId,idParqueo,fechaIngreso,estado")] TicketIngreso ticketIngreso)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ticketIngreso);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Ticket de ingreso creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.clientes, "idCliente", "nombreCompleto", ticketIngreso.ClienteId);
            ViewData["idParqueo"] = new SelectList(_context.Parqueos, "idParqueo", "nombreParqueo", ticketIngreso.idParqueo);
            ViewData["RegistroVehiculoId"] = new SelectList(_context.RegistroVehiculos, "idRegistroVehiculo", "placa", ticketIngreso.RegistroVehiculoId);
            return View(ticketIngreso);
        }

        // GET: TicketIngreso/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketIngreso = await _context.ticketIngresos.FindAsync(id);
            if (ticketIngreso == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.clientes, "idCliente", "nombreCompleto", ticketIngreso.ClienteId);
            ViewData["idParqueo"] = new SelectList(_context.Parqueos, "idParqueo", "nombreParqueo", ticketIngreso.idParqueo);
            ViewData["RegistroVehiculoId"] = new SelectList(_context.RegistroVehiculos, "idRegistroVehiculo", "placa", ticketIngreso.RegistroVehiculoId);
            return View(ticketIngreso);
        }

        // POST: TicketIngreso/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("idTicketIngreso,ClienteId,RegistroVehiculoId,idParqueo,fechaIngreso,estado")] TicketIngreso ticketIngreso)
        {
            if (id != ticketIngreso.idTicketIngreso)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticketIngreso);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Ticket de ingreso editado exitosamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketIngresoExists(ticketIngreso.idTicketIngreso))
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
            ViewData["ClienteId"] = new SelectList(_context.clientes, "idCliente", "nombreCompleto", ticketIngreso.ClienteId);
            ViewData["idParqueo"] = new SelectList(_context.Parqueos, "idParqueo", "nombreParqueo", ticketIngreso.idParqueo);
            ViewData["RegistroVehiculoId"] = new SelectList(_context.RegistroVehiculos, "idRegistroVehiculo", "placa", ticketIngreso.RegistroVehiculoId);
            return View(ticketIngreso);
        }

        // GET: TicketIngreso/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketIngreso = await _context.ticketIngresos
                .Include(t => t.Cliente)
                .Include(t => t.Parqueo)
                .Include(t => t.RegistroVehiculo)
                .FirstOrDefaultAsync(m => m.idTicketIngreso == id);
            if (ticketIngreso == null)
            {
                return NotFound();
            }

            return View(ticketIngreso);
        }

        // POST: TicketIngreso/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticketIngreso = await _context.ticketIngresos.FindAsync(id);
            if (ticketIngreso != null)
            {
                _context.ticketIngresos.Remove(ticketIngreso);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Ticket de ingreso eliminado exitosamente.";
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        //exportar a pdf 
        public async Task<IActionResult> ExportarPDF(int id)
        {
            var ticketIngreso = await _context.ticketIngresos
                .Include(t => t.Cliente)
                .Include(t => t.Parqueo)
                .Include(t => t.RegistroVehiculo)
                .FirstOrDefaultAsync(t => t.idTicketIngreso == id);

            if (ticketIngreso == null)
            {
                return NotFound();
            }

            // Generar un código único para el PDF
            string uniqueCode = $"{ticketIngreso.idTicketIngreso}_{DateTime.Now:yyyyMMddHHmmss}.pdf";

            using (var memoryStream = new MemoryStream())
            {
                try
                {
                    using (var writer = new PdfWriter(memoryStream))
                    {
                        using (var pdf = new PdfDocument(writer))
                        {
                            var document = new Document(pdf);
                            document.SetMargins(10, 10, 10, 10);

                            // Título
                            document.Add(new Paragraph("Parqueo El Tunas")
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFontSize(18)
                                .SetBold()
                                .SetMarginBottom(5));

                            // Título del ticket
                            document.Add(new Paragraph("Ticket de Ingreso")
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFontSize(22)
                                .SetBold()
                                .SetMarginBottom(10));

                            // Crear tabla
                            var table = new Table(UnitValue.CreatePercentArray(new float[] { 50, 50 })).UseAllAvailableWidth();

                            // Agregar encabezados de la tabla
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Campo").SetBold()));
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Valor").SetBold()));

                            // Agregar filas con información del ticket
                            table.AddCell("ID del Ticket:");
                            table.AddCell(ticketIngreso.idTicketIngreso.ToString());
                            table.AddCell("Fecha de Ingreso:");
                            table.AddCell(ticketIngreso.fechaIngreso.ToString("dd/MM/yyyy HH:mm"));
                            table.AddCell("Cliente:");
                            table.AddCell(ticketIngreso.Cliente.nombreCompleto);
                            table.AddCell("Registro de Vehículo:");
                            table.AddCell(ticketIngreso.RegistroVehiculo.placa);
                            table.AddCell("Parqueo:");
                            table.AddCell(ticketIngreso.Parqueo.nombreParqueo);

                            // Agregar tabla al documento
                            document.Add(table);

                            // Agregar el código único al documento
                            document.Add(new Paragraph($"Código : {uniqueCode.Replace(".pdf", "")}")
                                .SetTextAlignment(TextAlignment.LEFT)
                                .SetFontSize(12)
                                .SetMarginTop(15));

                            // Información adicional (opcional)
                            document.Add(new Paragraph("Gracias por su visita")
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFontSize(12)
                                .SetMarginTop(15));

                            // Cerrar documento
                            document.Close();
                        }
                    }

                    return File(memoryStream.ToArray(), "application/pdf", uniqueCode);
                }
                catch (Exception ex)
                {
                    return BadRequest($"Error al generar el PDF: {ex.Message}");
                }
            }
        }

        private bool TicketIngresoExists(int id)
        {
            return _context.ticketIngresos.Any(e => e.idTicketIngreso == id);
        }
    }
}