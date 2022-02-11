using DrinkOrderCV.Core.Repositories;

namespace DrinkOrderCV.Core.Services.Impl
{
    public class CartService : ICartService
    {
        private IRepository<CartModel> _cartRepository;
        private IRepository<DiscountModel> _discountRepository;
        private IRepository<ProductModel> _productRepository;

        public CartService(IRepository<CartModel> cartRepository, IRepository<DiscountModel> discountRepository, IRepository<ProductModel> productRepository)
        {
            _cartRepository = cartRepository;
            _discountRepository = discountRepository;
            _productRepository = productRepository;
        }

        public async Task<CartModel> ApplyDiscountAsync(CartModel cart, string discountCode)
        {
            var discount = await _discountRepository.GetAsync(discountCode);

            if (discount == null) throw new Exception("Discount doesn't exist");

            cart.SetDiscount(discount);
            await Save(cart);

            return cart;
        }

        public async Task<CartModel?> GetAsync(string id)
        {
            return await _cartRepository.GetAsync(id);
        }

        public async Task PayCart(CartModel cart, string paymentCode)
        {
            cart.PaymentMethodCode = paymentCode;
            cart.SetActive(false);
            await Save(cart);
        }

        public async Task<IEnumerable<CartProductModel>> PopolateProductPrice(IEnumerable<CartProductModel> products)
        {
            // to optimize
            var productsRepository = await _productRepository.GetAsync();
            var productsUpdated = new List<CartProductModel>();

            foreach(var product in products)
            {
                var productFound = productsRepository.FirstOrDefault(p => p.Code == product.ProductCode);
                if (productFound != null)
                {
                    product.Price = productFound.Price;
                    productsUpdated.Add(product);   
                }
            }

            return productsUpdated;
        }

        public async Task<CartModel> Save(CartModel cartModel)
        {
            return await _cartRepository.SaveAsync(cartModel);
        }
    }
}
