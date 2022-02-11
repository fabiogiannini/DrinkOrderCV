using DrinkOrderCV.Core;
using DrinkOrderCV.Core.Repositories;
using DrinkOrderCV.Core.Repositories.Impl;
using DrinkOrderCV.Web.Controllers;
using DrinkOrderCV.Web.Models.Reponse;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DrinkOrderCV.Web.Tests
{
    [TestFixture]
    public class CatalogControllerTest
    {
        private Mock<IRepository<ProductModel>> _repository;
        private CatalogController _controller;

        [SetUp]
        public void Setup()
        {
            _repository = new Mock<IRepository<ProductModel>>();
            _repository.Setup(x => x.GetAsync()).Returns(Task.FromResult(GetDummyProducts()));
            _controller = new CatalogController(_repository.Object);
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
        public async Task ShouldGetProducts()
        {
            var productReponse = await _controller.GetProducts();
            Assert.IsNotNull(productReponse);
            Assert.IsInstanceOf<ActionResult<IEnumerable<ProductResponse>>>(productReponse);
        }
    }
}
