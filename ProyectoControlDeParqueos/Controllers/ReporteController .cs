using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using ProyectoControlDeParqueos.Models;

namespace ProyectoControlDeParqueos.Controllers
{
    public class ReporteController : Controller
    {
        private readonly LoginDbContext _context;

        public ReporteController(LoginDbContext context)
        {
            _context = context;
        }

        // GET: Reporte
        public async Task<IActionResult> Index()
        {
            // Obtener datos de los vehículos registrados
            var vehiculos = await _context.RegistroVehiculos.ToListAsync();

            // Obtener tarifas activas
            var tarifas = await _context.Tarifas.Where(t => t.estado).ToListAsync();

            // Obtener datos del parqueo
            var parqueos = await _context.Parqueos.ToListAsync();

            // Crear el modelo de reporte
            var reporte = new ReporteGeneral
            {
                Vehiculos = vehiculos,
                Tarifas = tarifas,
                Parqueos = parqueos
            };

            return View(reporte);
        }

        // Método para generar PDF
        public async Task<IActionResult> ExportarPDF()
        {
            // Obtener datos de los vehículos registrados, tarifas y parqueos
            var vehiculos = await _context.RegistroVehiculos.ToListAsync();
            var tarifas = await _context.Tarifas.Where(t => t.estado).ToListAsync();
            var parqueos = await _context.Parqueos.ToListAsync();

            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new PdfWriter(memoryStream))
                {
                    using (var pdf = new PdfDocument(writer))
                    {
                        var document = new Document(pdf);

                        // Título del reporte
                        document.Add(new Paragraph("Parqueo El Tunas")
                            .SetFontSize(24)
                            .SetBold()
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetMarginBottom(10));

                        // Información de contacto (opcional)
                        document.Add(new Paragraph("Dirección: Nueva Santa Rosa")
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetMarginBottom(5));
                        document.Add(new Paragraph("Teléfono: 4928-9772")
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetMarginBottom(20));

                        // Título del reporte
                        document.Add(new Paragraph("Reporte General")
                            .SetFontSize(18)
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetMarginBottom(20));

                        // Tabla de Vehículos
                        document.Add(new Paragraph("Vehículos Registrados")
                            .SetFontSize(16)
                            .SetBold()
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetMarginTop(20));

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


                        // Tabla de Tarifas
                        document.Add(new Paragraph("Tarifas Activas")
                            .SetFontSize(16)
                            .SetBold()
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetMarginTop(20));

                        var tarifasTable = new Table(4); // 4 columnas
                        tarifasTable.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                        tarifasTable.SetWidth(UnitValue.CreatePercentValue(100));

                        tarifasTable.AddHeaderCell(new Cell().Add(new Paragraph("Descripción")).SetBold().SetTextAlignment(TextAlignment.CENTER));
                        tarifasTable.AddHeaderCell(new Cell().Add(new Paragraph("Costo por Hora (Q)")).SetBold().SetTextAlignment(TextAlignment.CENTER));
                        tarifasTable.AddHeaderCell(new Cell().Add(new Paragraph("Costo por Día (Q)")).SetBold().SetTextAlignment(TextAlignment.CENTER));
                        tarifasTable.AddHeaderCell(new Cell().Add(new Paragraph("Estado")).SetBold().SetTextAlignment(TextAlignment.CENTER));

                        foreach (var tarifa in tarifas)
                        {
                            tarifasTable.AddCell(new Cell().Add(new Paragraph(tarifa.descripcion)).SetTextAlignment(TextAlignment.CENTER));
                            tarifasTable.AddCell(new Cell().Add(new Paragraph(tarifa.costoPorHora.ToString("F2"))).SetTextAlignment(TextAlignment.CENTER));
                            tarifasTable.AddCell(new Cell().Add(new Paragraph(tarifa.costoPorDia.ToString("F2"))).SetTextAlignment(TextAlignment.CENTER));
                            tarifasTable.AddCell(new Cell().Add(new Paragraph(tarifa.estado ? "Activo" : "Inactivo")).SetTextAlignment(TextAlignment.CENTER));
                        }

                        document.Add(tarifasTable);

                        // Tabla de Parqueos
                        document.Add(new Paragraph("Parqueos")
                            .SetFontSize(16)
                            .SetBold()
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetMarginTop(20));

                        var parqueosTable = new Table(5); // 5 columnas
                        parqueosTable.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                        parqueosTable.SetWidth(UnitValue.CreatePercentValue(100));

                        parqueosTable.AddHeaderCell(new Cell().Add(new Paragraph("Nombre")).SetBold().SetTextAlignment(TextAlignment.CENTER));
                        parqueosTable.AddHeaderCell(new Cell().Add(new Paragraph("Ubicación")).SetBold().SetTextAlignment(TextAlignment.CENTER));
                        parqueosTable.AddHeaderCell(new Cell().Add(new Paragraph("Capacidad Total")).SetBold().SetTextAlignment(TextAlignment.CENTER));
                        parqueosTable.AddHeaderCell(new Cell().Add(new Paragraph("Espacios Disponibles")).SetBold().SetTextAlignment(TextAlignment.CENTER));
                        parqueosTable.AddHeaderCell(new Cell().Add(new Paragraph("Estado")).SetBold().SetTextAlignment(TextAlignment.CENTER));

                        foreach (var parqueo in parqueos)
                        {
                            parqueosTable.AddCell(new Cell().Add(new Paragraph(parqueo.nombreParqueo)).SetTextAlignment(TextAlignment.CENTER));
                            parqueosTable.AddCell(new Cell().Add(new Paragraph(parqueo.ubicacion)).SetTextAlignment(TextAlignment.CENTER));
                            parqueosTable.AddCell(new Cell().Add(new Paragraph(parqueo.capacidadTotal.ToString())).SetTextAlignment(TextAlignment.CENTER));
                            parqueosTable.AddCell(new Cell().Add(new Paragraph(parqueo.espaciosDisponibles.ToString())).SetTextAlignment(TextAlignment.CENTER));
                            parqueosTable.AddCell(new Cell().Add(new Paragraph(parqueo.estado ? "Activo" : "Inactivo")).SetTextAlignment(TextAlignment.CENTER));
                        }

                        document.Add(parqueosTable);

                        // Pie de página (opcional)
                        document.Add(new Paragraph("Gracias por su preferencia!")
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetMarginTop(20));

                        document.Close();
                    }
                }

                return File(memoryStream.ToArray(), "application/pdf", "ReporteGeneral.pdf");
            }
        }
    }
}
