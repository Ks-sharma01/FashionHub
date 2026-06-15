using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces.Services.Otp
{
    public interface IOtpService
    {
        Task SendOtpEmailAsync(string toEmail, string otp);
    }
}
