using Ecommerce.Application.Interfaces.Services.IExportExcel;
using Ecommerce.Application.Interfaces.Services.IExportPdf;
using Ecommerce.Application.Interfaces.Services.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IExportExcel _exportExcel;
        private readonly IExportPdf _exportPdf;
        public ReportController(IProductService productService, IExportExcel exportExcel, IExportPdf exportPdf)
        {
            _productService = productService;
            _exportExcel = exportExcel;
            _exportPdf = exportPdf;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("excel/products")]
        public async Task<IActionResult> ExportExcel(int pageNumber = 1, int pageSize = 10, string search = "", string filterType = "All", string category = "All", string status = "All", DateTime? fromDate = null, DateTime? toDate = null)
        {
            var products = await _productService.GetProductsPaged(pageNumber, pageSize, search, filterType, category, status, fromDate, toDate);

            var file = _exportExcel.ExportProductsToExcel(products);

            return File(
                 file,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"Products_{DateTime.Now:yyyyMMdd}.xlsx"
                );
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("pdf/products")]
        public async Task<IActionResult> ExportPdf(int pageNumber = 1, int pageSize = 10, string search = "", string filterType = "All", string category = "All", string status = "All", DateTime? fromDate = null, DateTime? toDate = null)
            {
            var products = await _productService.GetProductsPaged(pageNumber, pageSize, search, filterType, category, status, fromDate, toDate);

            var file = _exportPdf.ExportProductsToPdf(products);

            return File(
            file,
            "application/pdf",
            $"Products_{DateTime.Now:yyyyMMdd}.pdf");
        }
    }
}
