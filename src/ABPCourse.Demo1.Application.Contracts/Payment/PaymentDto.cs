using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace ABPCourse.Demo1.Payment
{
    public class PaymentDto: AggregateRoot<Guid>
    {
        public String Id { get; set; }
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }
        public string PaymentStatus { get; set; }
        public string PayerName { get; set; }
    }
}
