using JsonFlatFileDataStore;

namespace DrinkOrderCV.Core.Repositories.Impl
{
    public class DiscountRepository : Repository<DiscountModel>, IRepository<DiscountModel>
    {
        public DiscountRepository(DataStore store) : base(store)
        {
        }

        public async Task<DiscountModel?> GetAsync(string code)
        {
            return await GetIfExist(x => x.Code == code);
        }

        public Task<IEnumerable<DiscountModel>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<DiscountModel> SaveAsync(DiscountModel item)
        {
            if (!await Exist(x => x.Code == item.Code))
            {
                await InsertAsync(item);
            }
            else
            {
                await UpdateAsync(item.Code, item);
            }

            return item;
        }
    }
}
