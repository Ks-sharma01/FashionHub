using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces.Services.Email
{
    public interface IEmailService
    {
        Task SendPasswordEmailAsync(string toEmail, string password);
        string GeneratePassword();
    }
}
