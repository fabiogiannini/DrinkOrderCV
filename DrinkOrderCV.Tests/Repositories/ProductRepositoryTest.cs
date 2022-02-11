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
    public class ProductRepositoryTest : RepositoryTest
    {
        private DataStore _testDatabase;
        private string _collectionName;
        private IRepository<ProductModel> _discountRepository;

        [SetUp]
        public void Setup()
        {
            _testDatabase = GetTestDatabase();
            _discountRepository = new ProductRepository(_testDatabase);
            _collectionName = _discountRepository.CollectionName;
        }

        [Test]
        public async Task ShouldRetrieveAllProducts()
        {
            var records = new List<ProductModel>();
            records.Add(new ProductModel { Currency = "EUR", Code = "code1", Name = "name1", Price = 1 });
            records.Add(new ProductModel { Currency = "EUR", Code = "code2", Name = "name2", Price = 2 });
            records.Add(new ProductModel { Currency = "EUR", Code = "code3", Name = "name3", Price = 3 });
            await _testDatabase.GetCollection(_collectionName).InsertManyAsync(records);

            var products = await _discountRepository.GetAsync();
            Assert.IsNotNull(products);
            Assert.AreEqual(3, products.Count());
        }

        [Test]
        public async Task ShouldBeFilledCorrectly()
        {
            var record = new ProductModel { Currency = "EUR", Code = "code4", Name = "name4", Price = 4 };
            await _testDatabase.GetCollection(_collectionName).InsertOneAsync(record);

            var products = await _discountRepository.GetAsync();
            var product = products.FirstOrDefault(x => x.Code == record.Code);

            Assert.IsNotNull(product);
            Assert.AreEqual(record.Price, product.Price);
            Assert.AreEqual(record.Currency, product.Currency);
            Assert.AreEqual(record.Name, product.Name);
            Assert.AreEqual(record.Code, product.Code);
        }
    }
}