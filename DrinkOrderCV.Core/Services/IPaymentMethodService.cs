namespace DrinkOrderCV.Core.Services
{
    public interface IPaymentMethodService
    {
        Task<IEnumerable<PaymentMethodModel>> GetAsync(CartModel cart);
        Task<bool> CheckPaymentAvailable(CartModel cart, string paymentCode);
    }
}