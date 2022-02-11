using AutoMapper;
using DrinkOrderCV.Core;
using DrinkOrderCV.Web.Models.Reponse;
using DrinkOrderCV.Web.ViewModels;

namespace DrinkOrderCV.Web
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CartRequest, CartModel>();
            CreateMap<CartProductRequest, CartProductModel>();
            CreateMap<CartModel, CartResponse>();
            CreateMap<CartProductModel, CartProductResponse>();
            CreateMap<PaymentMethodModel, PaymentMethodResponse>();
            CreateMap<ProductModel, ProductResponse>();
        }
    }
}