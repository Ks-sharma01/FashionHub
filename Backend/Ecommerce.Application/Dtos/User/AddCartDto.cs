using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Dtos.User
{
    public class AddCartDto
    {
        public int UserId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public string Action { get; set; }

        public DateTime AddedOn { get; set; }
    }
}
