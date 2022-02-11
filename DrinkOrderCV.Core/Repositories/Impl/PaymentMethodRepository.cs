using JsonFlatFileDataStore;
using Microsoft.Extensions.Options;

namespace DrinkOrderCV.Core.Repositories.Impl
{
    public class PaymentMethodRepository : Repository<PaymentMethodModel>, IRepository<PaymentMethodModel>
    {
        public PaymentMethodRepository(DataStore dataStore) : base(dataStore)
        {
        }

        public Task<PaymentMethodModel?> GetAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PaymentMethodModel>> GetAsync()
        {
            return await GetAllAsync();
        }

        public Task<PaymentMethodModel> SaveAsync(PaymentMethodModel item)
        {
            throw new NotImplementedException();
        }
    }
}
