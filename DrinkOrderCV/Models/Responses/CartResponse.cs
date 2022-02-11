namespace DrinkOrderCV.Web.ViewModels
{
    public class CartResponse
    {
        public string Code { get; set; }
        public string? DiscountCode { get; set; }
        public IEnumerable<CartProductResponse> Products { get; set; }

        public decimal Total { get; set; }
    }
}