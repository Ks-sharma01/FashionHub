using Ecommerce.Application.Interfaces.Services.Otp;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.OtpService
{
    public class OtpService : IOtpService
    {
        private readonly IConfiguration _configuration;
        public OtpService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendOtpEmailAsync(string toEmail, string otp)
        {
            var smtpClient = new SmtpClient
            {
                Host = _configuration["EmailSettings:SmtpServer"],
                Port = Convert.ToInt32(_configuration["EmailSettings:Port"]),
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(
                    _configuration["EmailSettings:Username"],
                    _configuration["EmailSettings:Password"]
                )
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["EmailSettings:From"]),
                Subject = "FashionHub - Password Reset OTP",
                Body = $@"
<div style='font-family: Arial, sans-serif; background-color:#f4f4f4; padding:30px;'>

    <div style='max-width:600px; margin:auto; background:#ffffff; border-radius:10px; overflow:hidden; box-shadow:0 4px 10px rgba(0,0,0,0.1);'>

        <div style='background:linear-gradient(to right,#2563eb,#7c3aed); padding:20px; text-align:center; color:white;'>
            <h1>FashionHub</h1>
            <p>Password Reset Verification</p>
        </div>

        <div style='padding:30px;'>

            <h2 style='color:#333;'>Hello 👋</h2>

            <p style='font-size:16px; color:#555;'>
                We received a request to reset your FashionHub account password.
            </p>

            <p style='font-size:16px; color:#555;'>
                Use the OTP below to verify your identity:
            </p>

            <div style='background:#eef2ff; padding:20px; text-align:center; border-radius:8px; margin:20px 0;'>
                <h1 style='color:#2563eb; letter-spacing:5px; margin:0;'>
                    {otp}
                </h1>
            </div>

            <p style='color:#dc2626; font-weight:bold;'>
                This OTP is valid for 5 minutes only.
            </p>

            <p style='color:#555;'>
                If you did not request a password reset, please ignore this email.
            </p>

            <div style='text-align:center; margin-top:30px;'>
                <a href='http://localhost:5173/forgot-password'
                   style='background:#2563eb;
                          color:white;
                          padding:12px 24px;
                          text-decoration:none;
                          border-radius:6px;
                          display:inline-block;'>
                    Verify OTP
                </a>
            </div>

        </div>

        <div style='background:#f8f9fa; text-align:center; padding:15px; color:#666; font-size:14px;'>
            © 2026 FashionHub. All Rights Reserved.
        </div>

    </div>

</div>",
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
