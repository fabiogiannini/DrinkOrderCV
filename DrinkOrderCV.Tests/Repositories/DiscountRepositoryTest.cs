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
    public class DiscountRepositoryTest : RepositoryTest
    {
        private DataStore _testDatabase;
        private string _collectionName;
        private IRepository<DiscountModel> _discountRepository;

        [SetUp]
        public void Setup()
        {
            _testDatabase = GetTestDatabase();
            _discountRepository = new DiscountRepository(_testDatabase);
            _collectionName = _discountRepository.CollectionName;
        }

        [Test]
        public async Task ShouldRetrieveDiscountByCode()
        {
            var record = new DiscountModel { Code = "TEST_RETRIEVE", Name = "Test discount", DiscountValue = 10, Type = DiscountModel.DiscountType.Fixed };
            await _testDatabase.GetCollection<DiscountModel>().InsertOneAsync(record);

            var discount = await _discountRepository.GetAsync(record.Code);

            Assert.IsNotNull(discount);
            Assert.AreEqual(discount.Type, record.Type);
            Assert.AreEqual(discount.DiscountValue, record.DiscountValue);
            Assert.AreEqual(discount.Name, record.Name);
        }

        [Test]
        public async Task ShouldSaveDiscount()
        {
            var discount = new DiscountModel { Code = "TEST_SAVE", Name = "Test discount", DiscountValue = 10, Type = DiscountModel.DiscountType.Fixed };
            await _discountRepository.SaveAsync(discount);

            var record = _testDatabase.GetCollection<DiscountModel>().AsQueryable().Single(x => x.Code == discount.Code);
            Assert.IsNotNull(record);
            Assert.AreEqual(discount.Type, record.Type);
            Assert.AreEqual(discount.DiscountValue, record.DiscountValue);
            Assert.AreEqual(discount.Name, record.Name);
        }
    }
}
