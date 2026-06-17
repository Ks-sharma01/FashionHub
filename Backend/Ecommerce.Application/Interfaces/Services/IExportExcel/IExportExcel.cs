using Ecommerce.Application.Dtos.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces.Services.IExportExcel
{
    public interface IExportExcel
    {
        byte[] ExportProductsToExcel(List<ProductDto> products);

    }
}
