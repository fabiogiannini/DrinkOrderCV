using DrinkOrderCV.Core.Services;
using DrinkOrderCV.Core.Services.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace DrinkOrderCV.Core.Repositories
{
    public static class AddServiceRegistration
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IPaymentMethodService, PaymentMethodService>();
        }
    }
}
