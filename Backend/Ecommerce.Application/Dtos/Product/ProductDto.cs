using Microsoft.AspNetCore.Http;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Dtos.Product
{
    public class ProductDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public string Subcategory { get; set; }

        public decimal Prize { get; set; }

        public int Stock {  get; set; }

        public string Status { get; set; }

        public DateTime? OrderDate { get; set; } = null;

        public DateTime? DeliveredDate { get; set; } = null;

        public string PaymentMethod { get; set; }

        public IFormFile ImagePath { get; set; }

        public string ImageUrl { get; set; }

        public int TotalRecords { get; set; }






    }
}
