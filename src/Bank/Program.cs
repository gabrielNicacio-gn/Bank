using System.Net;
using System.Security.Claims;
using System.Text;
using Bank.Bank.Application.Extensions;
using Bank.Bank.Application.Services;
using Bank.Bank.Domain.Models;
using Bank.Bank.Infrastructure.Data;
using Bank.Bank.Routes;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.DependencyInjections();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    })
    .AddCookie(options =>
    {
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(120);
        options.LoginPath = "/Account/Login";
        options.Cookie.SameSite = SameSiteMode.Strict;
    });
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxConcurrentConnections = 200;
});
builder.Services.AddAuthorization();
builder.Services
    .AddIdentity<User,IdentityRole<Guid>>()
    .AddEntityFrameworkStores<BankContext>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapSwagger();
}
app.UseAuthentication();
app.UseAuthorization();
app.MapEndpoints();
app.Run();
