using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SimplifiedBank.Domain.Entities
{
    [Table("accounts")]
    public class Account
    {
        [Column("id")]
        public int Id { get; private set; }
        [Column("fullname")]
        public string FullName { get; private set; } = string.Empty;
        [Column("email")]
        public string Email { get; private set; } = string.Empty;
        [Column("cpf")]
        public string Cpf { get; private set; }
        [Column("balance")]
        public decimal Balance { get; private set; }
        [Column("password")]
        public string Password { get; private set; } = string.Empty;

        public Account(int id, string fullName, string email, string cpf, string password, decimal balance)
        {
            Id = id;
            FullName = fullName;
            Email = email;
            Cpf = cpf;
            Password = password;
            Balance = balance;
        }
    }
}