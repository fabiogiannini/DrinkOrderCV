using DrinkOrderCV.Core;
using DrinkOrderCV.Core.Repositories;
using DrinkOrderCV.Core.Repositories.Impl;
using JsonFlatFileDataStore;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkOrderCV.Tests.Repositories
{
    [TestFixture]
    public class CartRepositoryTest : RepositoryTest
    {
        private DataStore _testDatabase;
        private string _collectionName;
        private IRepository<CartModel> _cartRepository;

        [SetUp]
        public void Setup()
        {
            _testDatabase = GetTestDatabase();
            _cartRepository = new CartRepository(_testDatabase);
            _collectionName = _cartRepository.CollectionName;
        }

        [Test]
        public async Task ShouldRetrieveCartById()
        {
            var record = new CartModel() { Code = "TEST_RETRIEVE" };
            await _testDatabase.GetCollection(_collectionName).InsertOneAsync(record);

            var discount = await _cartRepository.GetAsync(record.Code);

            Assert.IsNotNull(discount);
            Assert.AreEqual(discount.IsActive, record.IsActive);
        }

        [Test]
        public async Task ShouldSaveCart()
        {
            var cart = new CartModel() { Code = "TEST_RETRIEVE" };
            await _cartRepository.SaveAsync(cart);

            var record = _testDatabase.GetCollection<CartModel>().AsQueryable().Single(x => x.Code == cart.Code);
            Assert.IsNotNull(record);
            Assert.AreEqual(cart.IsActive, record.IsActive);
        }

        [Test]
        public async Task ShouldCreateCart_WhenCartNull()
        {
            var cart = await _cartRepository.SaveAsync(null);

            var record = _testDatabase.GetCollection<CartModel>().AsQueryable().Single(x => x.Code == cart.Code);
            Assert.IsNotNull(record);
            Assert.AreEqual(cart.IsActive, record.IsActive);
        }
    }
}
