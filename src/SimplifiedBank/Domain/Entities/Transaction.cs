using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SimplifiedBank.Domain.Entities
{
    [Table("transactions")]
    public class Transaction
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("id_sender")]
        public int IdSender { get; set; }
        [Column("id_receiver")]
        public int IdReceiver { get; set; }
        [Column("value")]
        public decimal Value { get; set; }
        [Column("date")]
        public DateTime HourOfTransaction { get; set; }
        public Transaction(decimal value, int idSender, int idReceiver)
        {
            IdSender = idSender;
            IdReceiver = idReceiver;
            Value = value;
            HourOfTransaction = DateTime.UtcNow;
        }
    }
}