using DrinkOrderCV.Core.Repositories.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace DrinkOrderCV.Core.Repositories
{
    public static class AddRepositoryRegistration
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository<DiscountModel>, DiscountRepository>();
            services.AddScoped<IRepository<PaymentMethodModel>, PaymentMethodRepository>();
            services.AddScoped<IRepository<CartModel>, CartRepository>();
            services.AddScoped<IRepository<ProductModel>, ProductRepository>();
        }
    }
}
