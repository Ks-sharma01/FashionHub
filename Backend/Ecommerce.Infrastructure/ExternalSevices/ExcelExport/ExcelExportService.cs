using Ecommerce.Application.Dtos.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using System.IO;
using Ecommerce.Application.Interfaces.Services.IExportExcel;

namespace Ecommerce.Infrastructure.ExternalSevices.ExcelExport
{
    public class ExcelExportService : IExportExcel
    {
        public byte[] ExportProductsToExcel(List<ProductDto> products)
        {
            var workbook = new XLWorkbook();

            var sheet = workbook.Worksheets.Add("Products");

            sheet.Cell(1, 1).Value = "S/No";
            sheet.Cell(1, 2).Value = "Product Name";
            sheet.Cell(1, 3).Value = "Category";
            sheet.Cell(1, 4).Value = "Price";
            sheet.Cell(1, 5).Value = "Stock";
            sheet.Cell(1, 6).Value = "Status";
            sheet.Cell(1, 7).Value = "Order Date";
            sheet.SheetView.FreezeRows(1);

            var headerRange = sheet.Range("A1:G1");

            headerRange.Style.Fill.BackgroundColor = XLColor.DarkBlue;
            headerRange.Style.Font.FontColor = XLColor.White;
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            int row = 2;
            int index = 1;
            foreach(var product in products)
            {
                sheet.Cell(row, 1).Value = index;
                sheet.Cell(row, 2).Value = product.Name;
                sheet.Cell(row, 3).Value = product.Category;
                sheet.Cell(row, 4).Value = product.Prize;
                sheet.Cell(row, 5).Value = product.Stock;
                sheet.Cell(row, 6).Value = product.Status == "" ? "Not Ordered Yet" : product.Status;
                sheet.Cell(row, 7).Value = product.OrderDate.ToString() == "01-01-0001 00:00:00" ? "Not applicable" : product.OrderDate.ToString();
                row++;
                index++;
            }

            sheet.Columns().AdjustToContents();

            var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return stream.ToArray();

        }
    }
}
