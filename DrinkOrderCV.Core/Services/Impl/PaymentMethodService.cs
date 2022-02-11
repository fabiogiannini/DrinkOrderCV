using DrinkOrderCV.Core.Repositories;

namespace DrinkOrderCV.Core.Services.Impl
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private IRepository<PaymentMethodModel> _paymentMethodRepository;

        public PaymentMethodService(IRepository<PaymentMethodModel> paymentMethodRepository)
        {
            _paymentMethodRepository = paymentMethodRepository;
        }

        public async Task<bool> CheckPaymentAvailable(CartModel cart, string paymentCode)
        {
            return (await GetAsync(cart)).Any(p => p.Code == paymentCode);
        }

        public async Task<IEnumerable<PaymentMethodModel>> GetAsync(CartModel cart)
        {
            return await GetPaymentMethodByTotalCost(cart.Total);
        }

        private async Task<IEnumerable<PaymentMethodModel>> GetPaymentMethodByTotalCost(decimal totalCost)
        {
            return (await _paymentMethodRepository.GetAsync()).Where(pm=>pm.Threshold >= totalCost || pm.Threshold == null);
        }
    }
}
