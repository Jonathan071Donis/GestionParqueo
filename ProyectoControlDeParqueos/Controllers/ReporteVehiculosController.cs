using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoControlDeParqueos.Models;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Pdf;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoControlDeParqueos.Controllers
{
    public class ReporteVehiculosController : Controller
    {
        private readonly LoginDbContext _context;

        public ReporteVehiculosController(LoginDbContext context)
        {
            _context = context;
        }

        // GET: ReporteVehiculos/Index
        public async Task<IActionResult> Index()
        {
            var vehiculos = await _context.RegistroVehiculos.ToListAsync();

            var reporte = new ReporteVehiculos
            {
                Vehiculos = vehiculos
            };

            return View(reporte);
        }

        // Método para generar PDF
        public async Task<IActionResult> ExportarPDF()
        {
            var vehiculos = await _context.RegistroVehiculos.ToListAsync();

            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new PdfWriter(memoryStream))
                {
                    using (var pdf = new PdfDocument(writer))
                    {
                        var document = new Document(pdf);

                        // Título de la factura
                        document.Add(new Paragraph("Parqueo El Tunas")
                            .SetFontSize(24)
                            .SetBold()
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetMarginBottom(10));

                        // Información de contacto (opcional)
                        document.Add(new Paragraph("Dirección: Nueva Santa Rosa")
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                            .SetMarginBottom(5));
                        document.Add(new Paragraph("Teléfono: 4928-9772")
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                            .SetMarginBottom(20));


                        // Título del reporte
                        document.Add(new Paragraph("Reporte de Vehículos Registrados")
                            .SetFontSize(18)
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetMarginBottom(20));

                        // Crear tabla con estilo
                        var vehiculosTable = new Table(6); // 6 columnas
                        vehiculosTable.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                        vehiculosTable.SetWidth(UnitValue.CreatePercentValue(100));

                        vehiculosTable.AddHeaderCell(new Cell().Add(new Paragraph("Placa")).SetBold().SetTextAlignment(TextAlignment.CENTER));
                        vehiculosTable.AddHeaderCell(new Cell().Add(new Paragraph("Marca")).SetBold().SetTextAlignment(TextAlignment.CENTER));
                        vehiculosTable.AddHeaderCell(new Cell().Add(new Paragraph("Modelo")).SetBold().SetTextAlignment(TextAlignment.CENTER));
                        vehiculosTable.AddHeaderCell(new Cell().Add(new Paragraph("Color")).SetBold().SetTextAlignment(TextAlignment.CENTER));
                        vehiculosTable.AddHeaderCell(new Cell().Add(new Paragraph("Fecha de Ingreso")).SetBold().SetTextAlignment(TextAlignment.CENTER));
                        vehiculosTable.AddHeaderCell(new Cell().Add(new Paragraph("Estado")).SetBold().SetTextAlignment(TextAlignment.CENTER));

                        foreach (var vehiculo in vehiculos)
                        {
                            vehiculosTable.AddCell(new Cell().Add(new Paragraph(vehiculo.placa)).SetTextAlignment(TextAlignment.CENTER));
                            vehiculosTable.AddCell(new Cell().Add(new Paragraph(vehiculo.marca)).SetTextAlignment(TextAlignment.CENTER));
                            vehiculosTable.AddCell(new Cell().Add(new Paragraph(vehiculo.modelo)).SetTextAlignment(TextAlignment.CENTER));
                            vehiculosTable.AddCell(new Cell().Add(new Paragraph(vehiculo.color)).SetTextAlignment(TextAlignment.CENTER));
                            vehiculosTable.AddCell(new Cell().Add(new Paragraph(vehiculo.fechaIngreso.ToShortDateString())).SetTextAlignment(TextAlignment.CENTER));
                            // Convertir estado booleano a texto
                            vehiculosTable.AddCell(new Cell().Add(new Paragraph(vehiculo.estado ? "Activo" : "Inactivo")).SetTextAlignment(TextAlignment.CENTER));
                        }

                        document.Add(vehiculosTable);
                        document.Close();
                    }
                }

                return File(memoryStream.ToArray(), "application/pdf", "ReporteVehiculos.pdf");
            }
        }
    }
}
