using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Dtos.Product
{
    public class FilterProductDto
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public string Search {  get; set; }

        public string FilterType { get; set; }
    }
}
