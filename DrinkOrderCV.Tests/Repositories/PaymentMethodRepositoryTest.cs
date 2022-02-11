using DrinkOrderCV.Core;
using DrinkOrderCV.Core.Repositories;
using DrinkOrderCV.Core.Repositories.Impl;
using JsonFlatFileDataStore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkOrderCV.Tests.Repositories
{
    [TestFixture]
    public class PaymentMethodRepositoryTest : RepositoryTest
    {
        private DataStore _testDatabase;
        private string _collectionName;
        private IRepository<PaymentMethodModel> _discountRepository;

        [SetUp]
        public void Setup()
        {
            _testDatabase = GetTestDatabase();
            _discountRepository = new PaymentMethodRepository(_testDatabase);
            _collectionName = _discountRepository.CollectionName;
        }

        [Test]
        public async Task ShouldRetrievePaymentMethods()
        {
            var records = new List<PaymentMethodModel>();
            records.Add(new PaymentMethodModel { Code = "code1", Name = "name1", Threshold = 1 });
            records.Add(new PaymentMethodModel { Code = "code2", Name = "name2", Threshold = 2 });
            await _testDatabase.GetCollection(_collectionName).InsertManyAsync(records);

            var products = await _discountRepository.GetAsync();
            Assert.IsNotNull(products);
            Assert.AreEqual(2, products.Count());
        }
    }
}
