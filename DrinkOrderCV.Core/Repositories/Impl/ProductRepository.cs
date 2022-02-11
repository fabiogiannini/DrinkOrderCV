using JsonFlatFileDataStore;

namespace DrinkOrderCV.Core.Repositories.Impl
{
    public class ProductRepository : Repository<ProductModel>, IRepository<ProductModel>
    {
        public ProductRepository(DataStore store) : base(store)
        {
        }

        public async Task<ProductModel?> GetAsync(string code)
        {
            return await GetIfExist(x => x.Code == code);
        }

        public async Task<IEnumerable<ProductModel>> GetAsync()
        {
            return await GetAllAsync();
        }

        public Task<ProductModel> SaveAsync(ProductModel item)
        {
            throw new NotImplementedException();
        }
    }
}
