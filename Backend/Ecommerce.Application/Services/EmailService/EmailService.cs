using Ecommerce.Application.Interfaces.Services.Email;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using static Ecommerce.Application.Services.EmailService.EmailService;

namespace Ecommerce.Application.Services.EmailService
{
        public class EmailService : IEmailService
        {
            private readonly IConfiguration _configuration;
            public EmailService(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public async Task SendPasswordEmailAsync(string toEmail, string password)
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
                Subject = "Welcome to FashionHub - Your Login Password",
                Body = $@"
    <div style='font-family: Arial, sans-serif; background-color:#f4f4f4; padding:30px;'>

        <div style='max-width:600px; margin:auto; background:#ffffff; border-radius:10px; overflow:hidden; box-shadow:0 4px 10px rgba(0,0,0,0.1);'>

            <div style='background:linear-gradient(to right,#2563eb,#7c3aed); padding:20px; text-align:center; color:white;'>
                <h1>FashionHub</h1>
                <p>Welcome to our Ecommerce Platform</p>
            </div>

            <div style='padding:30px;'>

                <h2 style='color:#333;'>Hello User 👋</h2>

                <p style='font-size:16px; color:#555;'>
                    Thank you for registering with FashionHub.
                    Your account has been created successfully.
                </p>

                <p style='font-size:16px; color:#555;'>
                    Please use the password below to login:
                </p>

                <div style='background:#eef2ff; padding:20px; text-align:center; border-radius:8px; margin:20px 0;'>
                    <h1 style='color:#2563eb; letter-spacing:3px; margin:0;'>
                        {password}
                    </h1>
                </div>

                <p style='color:#dc2626; font-weight:bold;'>
                    Keep this password secure and do not share it with anyone.
                </p>

                <div style='text-align:center; margin-top:30px;'>
                    <a href='http://localhost:5173/verify-password'
                       style='background:#2563eb;
                              color:white;
                              padding:12px 24px;
                              text-decoration:none;
                              border-radius:6px;
                              display:inline-block;'>
                        Login Now
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

        public string GeneratePassword()
        {
             string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
             string lowerCase = "abcdefghijklmnopqrstuvwxyz";
             string numbers = "0123456789";
             string specialChars = "@#$%&*";

            string validChars = upperCase + lowerCase + numbers + specialChars;

            Random random = new Random();

            return new string(
                Enumerable.Repeat(validChars, 10)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray()
            );
        }

    }
    
}
