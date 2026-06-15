using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Dtos.Product
{
    public class CreateOrderDto
    {
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; } = null;
        public string City { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }

        public List<OrderItemDto> Items { get; set; }
    }

    public class OrderItemDto
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public DateTime OrderedDate { get; set; }
        public bool IsCancelled { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime DeliveredOn { get; set; }


    }
}
