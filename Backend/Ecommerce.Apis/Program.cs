using Ecommerce.Application.Interfaces.Repositories.Auth;
using Ecommerce.Application.Interfaces.Repositories.Product;
using Ecommerce.Application.Interfaces.Services.Auth;
using Ecommerce.Application.Interfaces.Services.Email;
using Ecommerce.Application.Interfaces.Services.Jwt;
using Ecommerce.Application.Interfaces.Services.Otp;
using Ecommerce.Application.Interfaces.Services.Product;
using Ecommerce.Application.Interfaces.Services.User;
using Ecommerce.Application.Interfaces.User;
using Ecommerce.Application.Services.AuthService;
using Ecommerce.Application.Services.EmailService;
using Ecommerce.Application.Services.OtpService;
using Ecommerce.Application.Services.ProductService;
using Ecommerce.Application.Services.UserService;
using Ecommerce.Infrastructure.Repositories.Auth;
using Ecommerce.Infrastructure.Repositories.Product;
using Ecommerce.Infrastructure.Repositories.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters =
        new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["JwtSecret:Issuer"],
            ValidAudience = builder.Configuration["JwtSecret:Audience"],

            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["JwtSecret:Key"])),

            RoleClaimType = ClaimTypes.Role
        };
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthRepo, AuthRepo>();
builder.Services.AddScoped<IOtpService, OtpService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowReact");

app.MapControllers();

app.Run();
