using Ecommerce.Application.Dtos.Product;
using Ecommerce.Application.Interfaces.Services.IExportPdf;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ecommerce.Infrastructure.ExternalSevices.PdfExport
{
    public class PdfExportService : IExportPdf
    {
        public byte[] ExportProductsToPdf(List<ProductDto> products)
        {
            var totalProducts = products.Count;
            var delivered = products.Where(x => x.Status == "Delivered").Count();
            var outOfStock = products.Where(x => x.Stock <= 0).Count();
            var totalRevenue = products.Where(x => x.Status == "Delivered").Sum(x => x.Prize);
            var totalOrders = products.Where(x => x.Status != "").Count();


            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(20);

                    page.Header().Column(column =>
                    {
                        column.Item().Text("FashionHub")
                            .FontSize(16)
                            .Bold();

                        column.Item().Text("Products Report")
                            .FontSize(14)
                            .SemiBold();

                        column.Item().Text($"Generated On: {DateTime.Now:dd MMM yyyy hh:mm tt}")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken2);

                        column.Item().PaddingTop(10);
                    });

                    page.Content().Column(column =>
                    {
                        // Summary Cards
                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Background(Colors.Blue.Lighten4)
                                .Padding(10)
                                .Column(c =>
                                {
                                    c.Item().Text("Total Products").Bold();
                                    c.Item().Text(totalProducts.ToString())
                                        .FontSize(16);
                                });

                            row.ConstantItem(10);

                            row.RelativeItem().Background(Colors.Green.Lighten4)
                                .Padding(10)
                                .Column(c =>
                                {
                                    c.Item().Text("Delivered Orders").Bold();
                                    c.Item().Text(delivered.ToString())
                                        .FontSize(16);
                                });

                            row.ConstantItem(10);

                            row.RelativeItem().Background(Colors.Red.Lighten4)
                                .Padding(10)
                                .Column(c =>
                                {
                                    c.Item().Text("Out Of Stock").Bold();
                                    c.Item().Text(outOfStock.ToString())
                                        .FontSize(16);
                                });

                            row.ConstantItem(10);

                            row.RelativeItem().Background(Colors.Purple.Lighten4)
                                .Padding(10)
                                .Column(c =>
                                {
                                    c.Item().Text("Total Orders").Bold();
                                    c.Item().Text(totalOrders.ToString())
                                        .FontSize(16);
                                });

                            row.ConstantItem(10);

                            row.RelativeItem().Background(Colors.Yellow.Lighten4)
                                .Padding(10)
                                .Column(c =>
                                {
                                    c.Item().Text("Revenue").Bold();
                                    c.Item().Text(totalRevenue.ToString())
                                        .FontSize(16);
                                });
                        });

                        column.Item().PaddingVertical(15);

                        // Table
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(40);
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn(2);
                            });

                            // Header
                            table.Header(header =>
                            {
                                void HeaderCell(string text)
                                {
                                    header.Cell()
                                        .Background(Colors.Blue.Darken2)
                                        .Padding(8)
                                        .Text(text)
                                        .FontColor(Colors.White)
                                        .Bold()
                                        .FontSize(10)
                                        ;
                                }

                                HeaderCell("S/No");
                                HeaderCell("Product Name");
                                HeaderCell("Category");
                                HeaderCell("Price");
                                HeaderCell("Stock");
                                HeaderCell("Status");
                                HeaderCell("Order Date");
                            });

                            for (int i = 0; i < products.Count; i++)
                            {
                                var p = products[i];

                                var bgColor = i % 2 == 0
                                    ? Colors.Grey.Lighten5
                                    : Colors.White;

                                void Cell(string text)
                                {
                                    table.Cell()
                                        .Background(bgColor)
                                        .BorderBottom(1)
                                        .BorderColor(Colors.Grey.Lighten2)
                                        .Padding(6)
                                        .Text(text ?? "-")
                                        .FontSize(9);
                                }

                                Cell((i+1).ToString());
                                Cell(p.Name);
                                Cell(p.Category);
                                Cell($"₹{p.Prize:N0}");
                                Cell(p.Stock.ToString());
                                Cell(p.Status == "" ? "Not Ordered Yet" : p.Status);

                                Cell(
                                    p.OrderDate == DateTime.MinValue
                                        ? "-"
                                        : p.OrderDate.ToString()
                                );
                            }
                        });
                    });

                    page.Footer()
                        .AlignCenter().Padding(1)
                        .Text(text =>
                        {
                            text.Span("Page ");
                            text.CurrentPageNumber();
                            text.Span(" of ");
                            text.TotalPages();
                        });
                });
            })
            .GeneratePdf();
        }
    }
}