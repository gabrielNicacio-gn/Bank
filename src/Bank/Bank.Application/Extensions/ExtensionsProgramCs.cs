using Bank.Bank.Application.Services;
using Bank.Bank.Domain.Interfaces;
using Bank.Bank.Infrastructure.Data;
using Bank.Bank.Infrastructure.Interfaces;
using Bank.Bank.Infrastructure.Repositories;

namespace Bank.Bank.Application.Extensions;

public static class ExtensionsProgramCs
{
    public static void DependencyInjections(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<BankContext>();
        builder.Services.AddScoped<IUserServices,UserServices>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IAccountServices, AccountServices>();
        builder.Services.AddScoped<ITransactionServices, TransactionServices>();
        builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddTransient<IAccountRepository,AccountRepository >();
        builder.Services.AddTransient<ITransactionRepository,TransactionRepository >();
    }
}