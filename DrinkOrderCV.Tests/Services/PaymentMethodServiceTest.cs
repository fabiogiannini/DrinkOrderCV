using DrinkOrderCV.Core;
using DrinkOrderCV.Core.Repositories;
using DrinkOrderCV.Core.Services;
using DrinkOrderCV.Core.Services.Impl;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkOrderCV.Tests.Services
{
    [TestFixture]
    public class PaymentMethodServiceTest
    {
        private IPaymentMethodService _paymentMethodService;

        [SetUp]
        public void Setup()
        {
            var paymentMethodRepository = new Mock<IRepository<PaymentMethodModel>>();

            paymentMethodRepository.Setup(x => x.GetAsync()).Returns(Task.FromResult(GetDummyPaymentMethods()));
            _paymentMethodService = new PaymentMethodService(paymentMethodRepository.Object);
        }

        private IEnumerable<PaymentMethodModel> GetDummyPaymentMethods()
        {
            return new List<PaymentMethodModel>
            {
                new PaymentMethodModel { Code = "payment1", Name = "payment1", Threshold = null },
                new PaymentMethodModel { Code = "payment2", Name = "payment2", Threshold = 10 },
            };
        }

        [TestCase(4, ExpectedResult = 2)]
        [TestCase(12, ExpectedResult = 1)]
        public async Task<int> ShouldGetPaymentMethods(decimal price)
        {
            var cart = new CartModel()
            {
                Code = "aaaa",
            };
            var cartProducts = new List<CartProductModel>
            {
                new CartProductModel { ProductCode = "itco", Qty = 1, Price = price },
            };
            cart.AddProducts(cartProducts);
            var paymentMethods = await _paymentMethodService.GetAsync(cart);

            Assert.IsNotEmpty(paymentMethods);

            return paymentMethods.Count();
        }

        [TestCase("payment1", ExpectedResult = true)]
        [TestCase("payment2", ExpectedResult = false)]
        public async Task<bool> ShouldGetPaymentMethods(string paymentMethodCode)
        {
            var cart = new CartModel()
            {
                Code = "aaaa",
            };
            var cartProducts = new List<CartProductModel>
            {
                new CartProductModel { ProductCode = "itco", Qty = 1, Price = 12 },
            };
            cart.AddProducts(cartProducts);

            return await _paymentMethodService.CheckPaymentAvailable(cart, paymentMethodCode);
        }
    }
}
