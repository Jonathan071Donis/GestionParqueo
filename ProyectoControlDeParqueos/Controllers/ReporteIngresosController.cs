using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoControlDeParqueos.Models;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System.Text;
using iText.Layout.Properties;
using iText.Commons.Actions.Contexts;
using OfficeOpenXml.Style;
using OfficeOpenXml;


namespace ProyectoControlDeParqueos.Controllers
{
    public class ReporteIngresosController : Controller
    {
        private readonly LoginDbContext _context;

        public ReporteIngresosController(LoginDbContext context)
        {
            _context = context;
        }

        // GET: ReporteIngresos/Index
        public async Task<IActionResult> Index()
        {
            var tarifas = await _context.Tarifas.ToListAsync();
            decimal totalIngresos = tarifas.Sum(t => t.costoPorHora + t.costoPorDia);

            var reporte = new ReporteIngresos
            {
                Tarifas = tarifas,
                TotalIngresos = totalIngresos
            };

            return View(reporte);
        }

        // Método para generar PDF
        // Método para generar PDF
        public async Task<IActionResult> ExportarPDF()
        {
            var tarifas = await _context.Tarifas.ToListAsync();
            decimal totalIngresos = tarifas.Sum(t => t.costoPorHora + t.costoPorDia);

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
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                            .SetMarginBottom(10));

                        // Información de contacto (opcional)
                        document.Add(new Paragraph("Dirección: Nueva Santa Rosa")
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                            .SetMarginBottom(5));
                        document.Add(new Paragraph("Teléfono: 4928-9772")
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                            .SetMarginBottom(20));

                        // Título del reporte
                        document.Add(new Paragraph("Reporte de Ingresos por Tarifa")
                            .SetFontSize(18)
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                            .SetMarginBottom(20));

                        // Crear tabla con estilo
                        var table = new Table(3);
                        table.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER); // Centrar la tabla
                        table.SetWidth(UnitValue.CreatePercentValue(100)); // Ancho 100%

                        table.AddHeaderCell(new Cell().Add(new Paragraph("Descripción")).SetBold().SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
                        table.AddHeaderCell(new Cell().Add(new Paragraph("Costo por Hora (Q)")).SetBold().SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
                        table.AddHeaderCell(new Cell().Add(new Paragraph("Costo por Día (Q)")).SetBold().SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

                        foreach (var tarifa in tarifas)
                        {
                            table.AddCell(new Cell().Add(new Paragraph(tarifa.descripcion)).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
                            table.AddCell(new Cell().Add(new Paragraph(tarifa.costoPorHora.ToString("F2"))).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
                            table.AddCell(new Cell().Add(new Paragraph(tarifa.costoPorDia.ToString("F2"))).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
                        }

                        // Total
                        var totalCell = new Cell(1, 3) // Ocupa 3 columnas
                            .Add(new Paragraph($"Total Ingresos Generados: Q {totalIngresos:F2}"))
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                            .SetBold()
                            .SetMarginTop(10);

                        table.AddFooterCell(totalCell);
                        document.Add(table);

                        // Pie de página (opcional)
                        document.Add(new Paragraph("Gracias por su preferencia!")
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                            .SetMarginTop(20));

                        document.Close();
                    }
                }

                return File(memoryStream.ToArray(), "application/pdf", "ReporteIngresos.pdf");
            }
        }



        //// Método para generar Excel
        //public async Task<IActionResult> ExportarExcel()
        //{
        //    var tarifas = await _context.Tarifas.ToListAsync();
        //    using (var package = new ExcelPackage())
        //    {
        //        var worksheet = package.Workbook.Worksheets.Add("Reporte de Ingresos");

        //        // Estilo de encabezados
        //        worksheet.Cells[1, 1].Value = "Descripción";
        //        worksheet.Cells[1, 2].Value = "Costo por Hora (Q)";
        //        worksheet.Cells[1, 3].Value = "Costo por Día (Q)";

        //        // Estilo de encabezados
        //        using (var headerRange = worksheet.Cells[1, 1, 1, 3])
        //        {
        //            headerRange.Style.Font.Bold = true;
        //            headerRange.Style.Font.Color.SetColor(System.Drawing.Color.White);
        //            headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 123, 255)); // Azul
        //            headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            headerRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //            headerRange.AutoFitColumns();
        //        }

        //        // Agregar datos
        //        int row = 2; // Comenzamos en la segunda fila
        //        foreach (var tarifa in tarifas)
        //        {
        //            worksheet.Cells[row, 1].Value = tarifa.descripcion;
        //            worksheet.Cells[row, 2].Value = tarifa.costoPorHora.ToString("F2");
        //            worksheet.Cells[row, 3].Value = tarifa.costoPorDia.ToString("F2");
        //            row++;
        //        }

        //        // Estilo de las celdas
        //        var dataRange = worksheet.Cells[2, 1, row - 1, 3];
        //        dataRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //        dataRange.AutoFitColumns();

        //        // Total
        //        worksheet.Cells[row, 1].Value = "Total Ingresos Generados:";
        //        worksheet.Cells[row, 2].Formula = $"SUM(B2:B{row - 1})"; // Suma de la columna B
        //        worksheet.Cells[row, 3].Formula = $"SUM(C2:C{row - 1})"; // Suma de la columna C
        //        worksheet.Cells[row, 1, row, 3].Style.Font.Bold = true;

        //        // Resaltar total
        //        worksheet.Cells[row, 1, row, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //        worksheet.Cells[row, 1, row, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

        //        // Centrando los totales
        //        worksheet.Cells[row, 1, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //        // Crear el archivo
        //        var excelBytes = package.GetAsByteArray();
        //        return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteIngresos.xlsx");
        //    }
        //}
    }
}
