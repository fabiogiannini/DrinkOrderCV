using DrinkOrderCV.Web.Models.Reponse;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DrinkOrderCV.Web.Tests
{
    [TestFixture]
    public class CatalogRequestTest
    {
        private IHost _host;

        [SetUp]
        public void Setup()
        {
            _host = TestHost.CreateHost().Start();
        }

        [Test]
        public async Task ShouldGetProducts()
        {
            var client = _host.GetTestClient();
            var response = await client.GetAsync("/api/catalog/");

            var content = await response.Content.ReadAsStringAsync();

            var responseObject = JsonConvert.DeserializeObject<IEnumerable<ProductResponse>>(content);
            Assert.IsNotNull(responseObject);
        }
    }
}