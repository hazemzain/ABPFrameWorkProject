﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace ABPCourse.Demo1.Payment
{
    public class PaymentAppServices : ApplicationService, IPaymentAppServices
    {
        #region Varible

        private readonly IRepository<payment, Guid> _paymentRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region actor

        public PaymentAppServices(IRepository<payment, Guid> paymentRepository, IObjectMapper objectMapper)
        {
            _paymentRepository = paymentRepository;
            _objectMapper = objectMapper;
        }

        #endregion

        public async Task<PaymentDto> CreatePaymentAsync(payment NewPaymentCreated)
        {
            //var payment = new payment(Guid.NewGuid(), NewTransactionCreate.TransactionId, NewTransactionCreate.Amount, "Pending", payerName);
            var inputValidation=new PaymentValidator().Validate(NewPaymentCreated);
            if (!inputValidation.IsValid)
            {
                throw new UserFriendlyException(("ValidationErrors"),
                    inputValidation.Errors.Select(e => e.ErrorMessage).JoinAsString(", "));
            }
            var result=await _paymentRepository.InsertAsync(NewPaymentCreated);
            return _objectMapper.Map<payment, PaymentDto>(result);
            

        }

        public Task DeletePaymentAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<PaymentDto> GetPaymentByIdAsync(Guid id)
        {
            var payment = await _paymentRepository.GetAsync(id);
            if (payment == null)
            {
                throw new UserFriendlyException("Payment not found");
            }
            return _objectMapper.Map<payment, PaymentDto>(payment);
        }

        public Task<List<PaymentDto>> GetPaymentsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<PaymentDto> UpdatePaymentAsync(Guid id, payment paymentToUpdate)
        {
            var InputValidation = new PaymentValidator().Validate(paymentToUpdate);
            if (!InputValidation.IsValid)
            {
                throw new UserFriendlyException(("ValidationErrors"),
                    InputValidation.Errors.Select(e => e.ErrorMessage).JoinAsString(", "));
            }
            var paymentToUpdateEntity = await _paymentRepository.GetAsync(id);
            if (paymentToUpdateEntity == null)
            {
                throw new KeyNotFoundException($"Payment with id: {id} does not exist.");
            }

            
            paymentToUpdateEntity.TransactionId = paymentToUpdate.TransactionId;
            paymentToUpdateEntity.Amount = paymentToUpdate.Amount;
            paymentToUpdateEntity.PaymentStatus = paymentToUpdate.PaymentStatus;
            paymentToUpdateEntity.PayerName = paymentToUpdate.PayerName;
            await _paymentRepository.UpdateAsync(paymentToUpdateEntity);
            return _objectMapper.Map<payment, PaymentDto>(paymentToUpdateEntity);
        }
    }
}