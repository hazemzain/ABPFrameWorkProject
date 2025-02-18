using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABPCourse.Demo1.Payment;
using Volo.Abp.Application.Services;

namespace ABPCourse.Demo1.Payment
{
    public interface IPaymentAppServices: IApplicationService
    {
        public Task<PaymentDto> CreatePaymentAsync(payment NewPaymentCreated);
        public Task<PaymentDto> GetPaymentByIdAsync(Guid id);
        //public Task<List<PaymentDto>> GetPaymentsAsync();
        public Task<PaymentDto> UpdatePaymentAsync(Guid id, payment paymentToUpdate);
        public Task DeletePaymentAsync(Guid id);
        public Task<PaymentDto> ProcessRefundAsync(Guid id);
        public Task<PaymentDto> CancelPaymentAsync(Guid paymentId);

    }
}
