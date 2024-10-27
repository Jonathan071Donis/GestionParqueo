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
using iText.IO.Image;

namespace ProyectoControlDeParqueos.Controllers
{
    public class TicketSalidaController : Controller
    {
        private readonly LoginDbContext _context;

        public TicketSalidaController(LoginDbContext context)
        {
            _context = context;
        }

        // GET: TicketSalida
        public async Task<IActionResult> Index()
        {
            var loginDbContext = _context.ticketSalidas
                .Include(t => t.Cliente)
                .Include(t => t.Parqueo)
                .Include(t => t.RegistroVehiculo)
                .Include(t => t.Tarifa);
            return View(await loginDbContext.ToListAsync());
        }

        // GET: TicketSalida/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketSalida = await _context.ticketSalidas
                .Include(t => t.Cliente)
                .Include(t => t.Parqueo)
                .Include(t => t.RegistroVehiculo)
                .Include(t => t.Tarifa)
                .FirstOrDefaultAsync(m => m.idTicketSalida == id);
            if (ticketSalida == null)
            {
                return NotFound();
            }

            return View(ticketSalida);
        }

        // GET: TicketSalida/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.clientes, "idCliente", "nombreCompleto");
            ViewData["ParqueoId"] = new SelectList(_context.Parqueos, "idParqueo", "nombreParqueo");
            ViewData["RegistroVehiculoId"] = new SelectList(_context.RegistroVehiculos, "idRegistroVehiculo", "placa");
            ViewData["TarifaId"] = new SelectList(_context.Tarifas, "idTarifa", "costoPorHora");
            return View();
        }

        // POST: TicketSalida/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("idTicketSalida,ClienteId,RegistroVehiculoId,ParqueoId,TarifaId,fechaSalida,cantidad,estado,costoTotal")] TicketSalida ticketSalida)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ticketSalida);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Ticket de salida creado con exito.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["ClienteId"] = new SelectList(_context.clientes, "idCliente", "nombreCompleto", ticketSalida.ClienteId);
            ViewData["ParqueoId"] = new SelectList(_context.Parqueos, "idParqueo", "nombreParqueo", ticketSalida.ParqueoId);
            ViewData["RegistroVehiculoId"] = new SelectList(_context.RegistroVehiculos, "idRegistroVehiculo", "placa", ticketSalida.RegistroVehiculoId);
            ViewData["TarifaId"] = new SelectList(_context.Tarifas, "idTarifa", "costoPorHora", ticketSalida.TarifaId);
            return View(ticketSalida);
        }

        // GET: TicketSalida/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketSalida = await _context.ticketSalidas.FindAsync(id);
            if (ticketSalida == null)
            {
                return NotFound();
            }

            ViewData["ClienteId"] = new SelectList(_context.clientes, "idCliente", "nombreCompleto", ticketSalida.ClienteId);
            ViewData["ParqueoId"] = new SelectList(_context.Parqueos, "idParqueo", "nombreParqueo", ticketSalida.ParqueoId);
            ViewData["RegistroVehiculoId"] = new SelectList(_context.RegistroVehiculos, "idRegistroVehiculo", "placa", ticketSalida.RegistroVehiculoId);
            ViewData["TarifaId"] = new SelectList(_context.Tarifas, "idTarifa", "costoPorHora", ticketSalida.TarifaId);
            return View(ticketSalida);
        }

        // POST: TicketSalida/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("idTicketSalida,ClienteId,RegistroVehiculoId,ParqueoId,TarifaId,fechaSalida,cantidad,estado,costoTotal")] TicketSalida ticketSalida)
        {
            if (id != ticketSalida.idTicketSalida)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticketSalida);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Ticket de salida actualizado con exito.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketSalidaExists(ticketSalida.idTicketSalida))
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
            ViewData["ClienteId"] = new SelectList(_context.clientes, "idCliente", "nombreCompleto", ticketSalida.ClienteId);
            ViewData["ParqueoId"] = new SelectList(_context.Parqueos, "idParqueo", "nombreParqueo", ticketSalida.ParqueoId);
            ViewData["RegistroVehiculoId"] = new SelectList(_context.RegistroVehiculos, "idRegistroVehiculo", "placa", ticketSalida.RegistroVehiculoId);
            ViewData["TarifaId"] = new SelectList(_context.Tarifas, "idTarifa", "costoPorHora", ticketSalida.TarifaId);
            return View(ticketSalida);
        }

        // GET: TicketSalida/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketSalida = await _context.ticketSalidas
                .Include(t => t.Cliente)
                .Include(t => t.Parqueo)
                .Include(t => t.RegistroVehiculo)
                .Include(t => t.Tarifa)
                .FirstOrDefaultAsync(m => m.idTicketSalida == id);
            if (ticketSalida == null)
            {
                return NotFound();
            }

            return View(ticketSalida);
        }

        // POST: TicketSalida/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticketSalida = await _context.ticketSalidas.FindAsync(id);
            _context.ticketSalidas.Remove(ticketSalida);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Ticket de salida eliminado con exito.";
            return RedirectToAction(nameof(Index));
        }


        // Exportar a Pdf 
        public async Task<IActionResult> ExportarPDF(int id)
        {
            var ticketSalida = await _context.ticketSalidas
                .Include(t => t.Cliente)
                .Include(t => t.Parqueo)
                .Include(t => t.RegistroVehiculo)
                .Include(t => t.Tarifa)
                .FirstOrDefaultAsync(t => t.idTicketSalida == id);

            if (ticketSalida == null)
            {
                return NotFound();
            }

            using (var memoryStream = new MemoryStream())
            {
                try
                {
                    using (var writer = new PdfWriter(memoryStream))
                    {
                        using (var pdf = new PdfDocument(writer))
                        {
                            var document = new Document(pdf);

                            // Estilo general del documento
                            document.SetMargins(10, 10, 10, 10);

                            // Título
                            document.Add(new Paragraph("Parqueo El Tunas")
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFontSize(18)
                                .SetBold()
                                .SetMarginBottom(5));

                            // Título del ticket
                            document.Add(new Paragraph("Ticket de Salida")
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
                            table.AddCell(ticketSalida.idTicketSalida.ToString());
                            table.AddCell("Fecha de Salida:");
                            table.AddCell(ticketSalida.fechaSalida.ToString("dd/MM/yyyy HH:mm"));
                            table.AddCell("Cliente:");
                            table.AddCell(ticketSalida.Cliente.nombreCompleto);
                            table.AddCell("Registro de Vehículo:");
                            table.AddCell(ticketSalida.RegistroVehiculo.placa);
                            table.AddCell("Parqueo:");
                            table.AddCell(ticketSalida.Parqueo.nombreParqueo);
                            table.AddCell("Tarifa:");
                            table.AddCell("Q " + ticketSalida.Tarifa.costoPorHora.ToString("0.00").Replace(",", "."));
                            table.AddCell("Cantidad:");
                            table.AddCell(ticketSalida.cantidad.ToString());
                            table.AddCell("Costo Total:");
                            table.AddCell("Q " + ticketSalida.costoTotal.ToString("0.00").Replace(",", "."));

                            // Agregar tabla al documento
                            document.Add(table);

                            // Información adicional (opcional)
                            document.Add(new Paragraph("Gracias por su visita")
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFontSize(12)
                                .SetMarginTop(15));

                            // Cerrar documento
                            document.Close();
                        }
                    }

                    return File(memoryStream.ToArray(), "application/pdf", "TicketSalida.pdf");
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones
                    return BadRequest($"Error al generar el PDF: {ex.Message}");
                }
            }
        }


        private bool TicketSalidaExists(int id)
        {
            return _context.ticketSalidas.Any(e => e.idTicketSalida == id);
        }
    }
}
