using JsonFlatFileDataStore;

namespace DrinkOrderCV.Core.Repositories.Impl
{
    public class CartRepository : Repository<CartModel>, IRepository<CartModel>
    {
        public CartRepository(DataStore store) : base(store)
        {
        }

        public async Task<CartModel?> GetAsync(string code)
        {
            Func<dynamic, bool> func = x => x.Code == code;

            return await GetIfExist(func);
        }

        public async Task<CartModel> SaveAsync(CartModel cart)
        {
            if (cart == null) cart = GenerateNewCart();

            if(!await Exist(x =>x.Code == cart.Code))
            {
                await InsertAsync(cart);
            }
            else
            {
                if(!await UpdateAsync(cart.Code, cart))
                {
                    throw new Exception("Not updated");
                }
            }

            return cart;
        }

        public CartModel GenerateNewCart()
        {
            return new CartModel()
            {
                Code = RandomString(8)
            };
        }

        public async Task<IEnumerable<CartModel>> GetAsync()
        {
            return await GetAllAsync();
        }
    }
}