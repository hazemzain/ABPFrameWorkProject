using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace ABPCourse.Demo1.Payment
{
    public class payment:AggregateRoot<Guid>
    {
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        
        public DateTime PaymentDate { get; set; }
        public string PaymentStatus { get; set; }
        public string PayerName { get; set; }

        public payment(Guid id,string TransactionId, decimal Amount, string PaymentStatus, string PayerName)
        {
            Id= id;
            this.TransactionId = TransactionId;
            this.Amount = Amount;
            PaymentDate = DateTime.UtcNow;
            this.PaymentStatus = PaymentStatus;
            this.PayerName = PayerName;
        }
    }
}
