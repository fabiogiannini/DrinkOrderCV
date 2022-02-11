using AutoMapper;
using DrinkOrderCV.Core;
using DrinkOrderCV.Core.Services;
using DrinkOrderCV.Web.Controllers;
using DrinkOrderCV.Web.Models.Reponse;
using DrinkOrderCV.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DrinkOrderCV.Web.Tests
{
    [TestFixture]
    public class ShoppingCartControllerTest
    {
        private Mock<IPaymentMethodService> _paymentMethodService;
        private IMapper _automapper;

        [SetUp]
        public void Setup()
        {
            _paymentMethodService = new Mock<IPaymentMethodService>();
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            _automapper = mockMapper.CreateMapper();
        }

        private IEnumerable<ProductModel> GetDummyProducts()
        {
            return new List<ProductModel>
            {
                new ProductModel { Code = "product1", Name = "product1" },
                new ProductModel { Code = "product2", Name = "product2" },
            };
        }

        [Test]
        public async Task ShouldGetNewCart()
        {
            var cartService = new Mock<ICartService>();
            var controller = new ShoppingCartController(cartService.Object, _paymentMethodService.Object, _automapper);

            var cartResponse = await controller.GetCartAsync();
            Assert.IsNotNull(cartResponse);
            Assert.IsInstanceOf<ActionResult<CartResponse>>(cartResponse);
            cartService.Verify(mock => mock.Save(null), Times.Once());
        }

        [Test]
        public async Task ShouldFail_WhenCartClosed()
        {
            var cartService = new Mock<ICartService>();
            var cartClosed = new CartModel() { Code = "AAA" };
            cartClosed.SetActive(false);
            cartService.Setup(mock => mock.GetAsync("AAA")).Returns(Task.FromResult(cartClosed));
            var controller = new ShoppingCartController(cartService.Object, _paymentMethodService.Object, _automapper);

            Assert.ThrowsAsync<HttpStatusException>(async () => await controller.GetCartAsync("AAA"));
        }
    }
}
