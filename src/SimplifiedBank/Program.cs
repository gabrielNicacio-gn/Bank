using Microsoft.EntityFrameworkCore;
using SimplifiedBank.Application.UseCases;
using SimplifiedBank.Domain.Repositories;
using SimplifiedBank.Infrastructure.Data;
using SimplifiedBank.Infrastructure.Repositories;
using SimplifiedBank.Infrastructure.Repositories.AccountsRepositories;
using SimplifiedBank.Interfaces.Routes;
using SimplifiedBank.Services;
using SimplifiedBank.Services.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IAuthorizationTransaction, AuthorizationTransaction>();
builder.Services.AddScoped<IAccountRepositories, AccountRepository>();
builder.Services.AddScoped<ICreateTransaction, CreateTransaction>();
builder.Services.AddScoped<IReturnAccount, ReturnAccount>();

builder.Services.AddDbContext<BankContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnString"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapEndpoints();
app.Run();

