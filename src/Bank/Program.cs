using System.Security.Claims;
using Bank.Bank.Application.Extensions;
using Bank.Bank.Application.Services;
using Bank.Bank.Domain.Models;
using Bank.Bank.Infrastructure.Data;
using Bank.Bank.Routes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.DependencyInjections();

builder.Services.AddAuthentication()
    .AddCookie(options =>
    {
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(120);
    });
builder.Services.AddAuthorization();

builder.Services
    .AddIdentityApiEndpoints<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<BankContext>();

var app = builder.Build();

var cookiePolicyOptions = new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
};

app.UseCookiePolicy(cookiePolicyOptions);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapSwagger();
}

app.MapEndpoints();

app.Run();
