using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Dtos.User
{
    public class ResetPasswordDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
