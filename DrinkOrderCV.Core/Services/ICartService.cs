namespace DrinkOrderCV.Core.Services
{
    public interface ICartService
    {
        Task<CartModel> ApplyDiscountAsync(CartModel cart, string discountCode);
        Task<CartModel> Save(CartModel cartModel);
        Task<CartModel?> GetAsync(string id);
        Task PayCart(CartModel cart, string paymentCode);
        Task<IEnumerable<CartProductModel>> PopolateProductPrice(IEnumerable<CartProductModel> products);
    }
}
