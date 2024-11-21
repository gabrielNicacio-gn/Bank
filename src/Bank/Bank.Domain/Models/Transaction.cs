using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Bank.Domain.Models
{
    public class Transaction
    {
        [Column("id")]
        public Guid Id { get; private set; }
        [Column("id_sender")]
        public Guid IdSender { get; set; }
        [Column("id_receiver")]
        public Guid IdReceiver { get; set; }
        [Column("value")]
        public decimal Value { get; set; }
        [Column("date")]
        public DateTime HourOfTransaction { get; private set; }
        
        public Transaction()
        {
            Id = Guid.NewGuid();
            HourOfTransaction = DateTime.UtcNow;
        }
    }
}