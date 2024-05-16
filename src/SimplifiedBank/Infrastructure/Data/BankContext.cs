using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SimplifiedBank.Domain.Entities;

namespace SimplifiedBank.Infrastructure.Data
{
    public class BankContext : DbContext
    {
        public BankContext(DbContextOptions options) : base(options) { }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        => base.OnModelCreating(modelBuilder);

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.LogTo(Console.WriteLine)
        .EnableDetailedErrors()
        .EnableSensitiveDataLogging(false);
    }
}