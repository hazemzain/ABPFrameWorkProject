using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABPCourse.Demo1.Payment;
using Allure.NUnit;
using ABPCourse.Demo1.Products;
using Moq;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using NSubstitute;
using FluentAssertions;
using NSubstitute.ExceptionExtensions;

namespace ABPCourse.Demo1.Tests
{
    [TestFixture]
    [AllureNUnit]
    public class PaymentAppServicesTests
    {
        #region varibles
        // private const IQueryable<Product>? ProductQuery = (IQueryable<Product>)null;
        private Mock<IRepository<payment, Guid>> _PaymentRepositoryMock;
        private Mock<IObjectMapper> _MockObjectMapper;
        private PaymentAppServices _paymentAppService;


        #endregion

        #region SetupMethod

        [SetUp]
        public void SetUp()
        {
            _PaymentRepositoryMock = new Mock<IRepository<payment, Guid>>();
            _MockObjectMapper = new Mock<IObjectMapper>();
            _paymentAppService = new PaymentAppServices(_PaymentRepositoryMock.Object, _MockObjectMapper.Object);
        }

        #endregion

        #region TestCases_For_CreatePaymentAsync

        [Test]
        public async Task CreatePaymentAsync_Should_Create_Valid_Payment()
        {
            var newPayment = new payment(Guid.NewGuid(), "TXN12345", 100.5m, "Pending", "Hazem zain");
            var paymentDto = new PaymentDto
            {
                Id = newPayment.Id.ToString(),
                TransactionId = newPayment.TransactionId,
                Amount = newPayment.Amount,
                PaymentStatus = newPayment.PaymentStatus,
                PayerName = newPayment.PayerName
            };

            _PaymentRepositoryMock.Setup(x => x.InsertAsync(newPayment, false, default)).ReturnsAsync(newPayment);
            _MockObjectMapper.Setup(m => m.Map<payment, PaymentDto>(newPayment)).Returns(paymentDto);
            var result = await _paymentAppService.CreatePaymentAsync(newPayment);
            Assert.That(result.Id, Is.EqualTo(newPayment.Id.ToString()));
            Assert.That(result.TransactionId, Is.EqualTo(newPayment.TransactionId));
            Assert.That(result.Amount, Is.EqualTo(newPayment.Amount));
            Assert.That(result.PaymentStatus, Is.EqualTo(newPayment.PaymentStatus));
            Assert.That(result.PayerName, Is.EqualTo(newPayment.PayerName));
        }

        [TestCase("", 100.0, "Pending", "Hazem Zain", "Transaction ID is required.")]
        [TestCase("TXN12345", -10.0, "Pending", "Hazem Zain", "Amount must be greater than zero.")]
        [TestCase("TXN12345", 100.0, "Unknown", "Hazem Zain", "Status must be 'Pending', 'Completed', or 'Failed'.")]
        [TestCase("TXN12345", 100.0, "Pending", "", "Payer name is required.")]
        [TestCase("", -10.0, "Unknown", "",
            "Transaction ID is required., Amount must be greater than zero., Status must be 'Pending', 'Completed', or 'Failed'., Payer name is required.")]
        public void CreatePaymentAsync_Should_Throw_Exception_When_Validation_Fails(string transactionId, decimal amount,
            string status, string payerName, string expectedErrorMessage)
        {
            // Arrange
            var invalidPayment = new payment(Guid.NewGuid(), transactionId, amount, status, payerName);

            // Act
            var exception = Assert.ThrowsAsync<UserFriendlyException>(async () =>
                await _paymentAppService.CreatePaymentAsync(invalidPayment));

            // Assert
            Assert.That(exception.Code, Is.EqualTo(expectedErrorMessage));
        }

        [TestCase("", 100.0, "Pending", "Hazem Zain", "Transaction ID is required.")]
        [TestCase("TXN12345", -10.0, "Pending", "Hazem Zain", "Amount must be greater than zero.")]
        [TestCase("TXN12345", 100.0, "Unknown", "Hazem Zain", "Status must be 'Pending', 'Completed', or 'Failed'.")]
        [TestCase("TXN12345", 100.0, "Pending", "", "Payer name is required.")]
        [TestCase("", -10.0, "Unknown", "",
            "Transaction ID is required., Amount must be greater than zero., Status must be 'Pending', 'Completed', or 'Failed'., Payer name is required.")]
        public async Task UpdatePaymentAsync_Should_Throw_Exception_When_Validation_Fails(
            string transactionId, decimal amount, string status, string payerName, string expectedErrorMessage)
        {
            // Arrange
            var paymentId = Guid.NewGuid(); 
            var invalidPayment = new payment(paymentId, transactionId, amount, status, payerName);

            // Act
            var exception = Assert.ThrowsAsync<UserFriendlyException>(async () =>
                await _paymentAppService.UpdatePaymentAsync(paymentId, invalidPayment));

            // Assert
            Assert.That(exception.Code, Is.EqualTo(expectedErrorMessage));
        }



        #endregion

        #region TestCases_For_GetPaymentByIdAsync

        [Test]
        public async Task GetPaymentByIdAsync_Should_Return_PaymentDto_When_Payment_Exists()
        {
            // Arrange
            var paymentId = Guid.Parse("2D58C255-4A31-8C57-6045-3A177C268052");
            var Existpayment = new payment(paymentId, "TXN12345", 100.0m, "Pending", "HazemZain");

            var paymentDto = new PaymentDto
            {
                Id = paymentId.ToString(),
                TransactionId = Existpayment.TransactionId,
                Amount = Existpayment.Amount,
                PaymentStatus = Existpayment.PaymentStatus,
                PayerName = Existpayment.PayerName
            };

            _PaymentRepositoryMock.Setup(repo => repo.GetAsync(paymentId, true, default)).ReturnsAsync(Existpayment);
            _MockObjectMapper
                .Setup(mapper => mapper.Map<payment, PaymentDto>(It.IsAny<payment>()))
                .Returns(paymentDto);

            // Act
            var result = await _paymentAppService.GetPaymentByIdAsync(paymentId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(paymentId.ToString()));
            Assert.That(result.TransactionId, Is.EqualTo(Existpayment.TransactionId));
            Assert.That(result.Amount, Is.EqualTo(Existpayment.Amount));
            Assert.That(result.PaymentStatus, Is.EqualTo(Existpayment.PaymentStatus));
            Assert.That(result.PayerName, Is.EqualTo(Existpayment.PayerName));
        }

        [Test]
        public void GetPaymentByIdAsync_Should_Throw_Exception_When_Payment_Not_Found()
        {
            // Arrange
            var paymentId = Guid.NewGuid();

            _PaymentRepositoryMock
                .Setup(repo => repo.GetAsync(paymentId,true,default))
                .ReturnsAsync((payment)null);

            // Act
            Func<Task> act = async () => await _paymentAppService.GetPaymentByIdAsync(paymentId);

            // Assert
            act.Should().ThrowAsync<UserFriendlyException>()
                .WithMessage("Payment not found");
        }

        [Test]
        public void GetPaymentByIdAsync_Should_Throw_Exception_When_Repository_Fails()
        {
            // Arrange
            var paymentId = Guid.NewGuid();
            _PaymentRepositoryMock
                .Setup(repo => repo.GetAsync(paymentId,true, default))
                .ThrowsAsync(new Exception("Database failure"));

            // Act
            Func<Task> act = async () => await _paymentAppService.GetPaymentByIdAsync(paymentId);

            // Assert
            act.Should().ThrowAsync<Exception>().WithMessage("Database failure");
        }

        #endregion

        #region TestCases_For_UpdatePaymentAsync

        [Test]
        public async Task UpdatePaymentAsync_ShouldReturnUpdatedPaymentDto_WhenValidPaymentProvided()
        {
            // Arrange
            var paymentId = Guid.NewGuid();
            var existingPayment = new payment(paymentId, "TXN12345", 100.0m, "Pending", "HazemZain");

            var paymentToUpdate = new payment(paymentId, "TXN54321", 200.0m, "Completed", "NewPayer");
            var updatedPaymentDto = new PaymentDto
            {
                Id = paymentId.ToString(),
                TransactionId = paymentToUpdate.TransactionId,
                Amount = paymentToUpdate.Amount,
                PaymentStatus = paymentToUpdate.PaymentStatus,
                PayerName = paymentToUpdate.PayerName
            };

            _PaymentRepositoryMock.Setup(repo => repo.GetAsync(paymentId, true, default)).ReturnsAsync(existingPayment);
            _PaymentRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<payment>(),false,default)).ReturnsAsync(existingPayment);
            _MockObjectMapper.Setup(mapper => mapper.Map<payment, PaymentDto>(It.IsAny<payment>())).Returns(updatedPaymentDto);

            // Act
            var result = await _paymentAppService.UpdatePaymentAsync(paymentId, paymentToUpdate);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(paymentId.ToString()));
            Assert.That(result.TransactionId, Is.EqualTo(paymentToUpdate.TransactionId));
            Assert.That(result.Amount, Is.EqualTo(paymentToUpdate.Amount));
            Assert.That(result.PaymentStatus, Is.EqualTo(paymentToUpdate.PaymentStatus));
            Assert.That(result.PayerName, Is.EqualTo(paymentToUpdate.PayerName));
        }
        [Test]
        public async Task UpdatePaymentAsync_ShouldThrowKeyNotFoundException_WhenPaymentNotFound()
        {
            // Arrange
            var paymentId = Guid.NewGuid();
            var paymentToUpdate = new payment(paymentId, "TXN54321", 200.0m, "Completed", "NewPayer");

            _PaymentRepositoryMock.Setup(repo => repo.GetAsync(paymentId, default, default)).ReturnsAsync((payment)null);

            // Act
            Func<Task> act = async () => await _paymentAppService.UpdatePaymentAsync(paymentId, paymentToUpdate);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Payment with id: {paymentId} does not exist.");
        }



        #endregion

    }
}
