using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SimplifiedBank.Application.Authentication;
using SimplifiedBank.Application.UseCases;
using SimplifiedBank.Domain.Interface;
using SimplifiedBank.Infrastructure.Data;
using SimplifiedBank.Infrastructure.Repositories.TransactionRepositories;
using SimplifiedBank.Infrastructure.Repositories.AccountsRepositories;
using SimplifiedBank.Interfaces.Routes;
using SimplifiedBank.Services;
using SimplifiedBank.Services.Interfaces;
using SimplifiedBank.Application.Services;
using SimplifiedBank.Interfaces.Exceptions;
using SimplifiedBank.Domain.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BankContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnString"));
}, ServiceLifetime.Transient);

builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IServiceExternalTransactionAuthorizer, ServiceExternalTransactionAuthorizer>();
builder.Services.AddScoped<IAccountRepositories, AccountsRepositories>();
builder.Services.AddScoped<ICreateTransaction, CreateTransaction>();
builder.Services.AddScoped<IReturnAccount, ReturnAccount>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<ITransactionsServices, TransactionsServices>();
builder.Services.AddScoped<IReturnsTheLatestTransactions, ReturnsTheLatestTransactions>();

var secretKey = builder.Configuration["JWT:SecretKey"] ?? throw new SecretKeyInvalidException();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };

});

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapTransactionEndpoints();
app.MapAccountEndpoints();
app.Run();

