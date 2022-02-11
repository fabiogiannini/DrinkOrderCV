using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DrinkOrderCV.Web.Tests
{
    public class ShoppingCartRequestTest
    {
        private IHost _host;

        [SetUp]
        public void Setup()
        {
            // _shoppingCartControllerTest = new ShoppingCartController();
            _host = TestHost.CreateHost().Start();
        }

        [Test]
        public async Task ShouldCreateNewCart()
        {
            var client = _host.GetTestClient();
            var response = await client.GetAsync("/api/shoppingcart/");

            var content = await response.Content.ReadAsStringAsync();

            var responseObject = JsonConvert.DeserializeObject<ShoppingCartResponseTest>(content);
            Assert.IsNotNull(responseObject);
            Assert.IsNotNull(responseObject.Code);
        }
    }
}