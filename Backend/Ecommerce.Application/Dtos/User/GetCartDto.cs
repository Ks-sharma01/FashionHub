using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Dtos.User
{
    public class GetCartDto
    {
        public int CartId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public int Stock { get; set; }

        public int Quantity { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal DiscountedAmount { get; set; }

    }
}
