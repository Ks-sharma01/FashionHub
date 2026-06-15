using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities.Orders
{
    public class tbl_Orders
    {
        public int Id { get; set; }

        public string CustomerName { get; set; }

        public string Email { get; set; }

        public string MobileNo { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Pincode { get; set; }

        public decimal TotalAmount { get; set; }

        public string PaymentMethod { get; set; }

        public string PaymentStatus { get; set; }

        public string OrderStatus { get; set; }

        public DateTime OrderDate { get; set; }

        public ICollection<tbl_OrderItem> OrderItems { get; set; }
    }
}
