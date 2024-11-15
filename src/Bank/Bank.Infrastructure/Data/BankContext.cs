using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank.Bank.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
    
namespace Bank.Bank.Infrastructure.Data;
    public class BankContext : IdentityDbContext<User,IdentityRole<Guid>,Guid>
    {
        public BankContext() { }
        public BankContext(DbContextOptions<BankContext> options) : base(options) { }
        
        DbSet<Account> Accounts { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();
            optionsBuilder.UseNpgsql(config.GetConnectionString("ConnStr"));
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasOne(u => u.Account)
                .WithOne(a => a.User)
                .HasForeignKey<Account>(a => a.UserId);
            
            builder.Entity<Account>().HasKey(a=>a.AccountId);
            
            base.OnModelCreating(builder);
        }
    }
