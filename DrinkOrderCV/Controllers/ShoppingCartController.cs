using AutoMapper;
using DrinkOrderCV.Core;
using DrinkOrderCV.Core.Services;
using DrinkOrderCV.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DrinkOrderCV.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IPaymentMethodService _paymentMethodService;
        private readonly IMapper _mapper;

        public ShoppingCartController(ICartService cartService, IPaymentMethodService paymentMethodService, IMapper mapper)
        {
            _cartService = cartService;
            _paymentMethodService = paymentMethodService;
            _mapper = mapper;
        }

        /// <summary>
        /// Getting products in the cart with total, if empty it generates an Id that identifies the cart
        /// </summary>
        /// <param name="cartCode">Shopping cart id</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/cart/2LU8VLIV is closed and should throw exception with status code 400
        ///     GET /api/cart/TX9HA4IE is open and should return the cart
        ///
        /// </remarks>
        /// <returns>A cart object with Id, discounts, products and total </returns>
        [HttpGet("{cartCode}")]
        public async Task<ActionResult<CartResponse>> GetCartAsync(string cartCode)
        {
            var cart = await GetCartOrFailClosed(cartCode);

            if (cart == null)
            {
                cart = await _cartService.Save(cart);
            }

            return Ok(_mapper.Map<CartResponse>(cart));
        }

        /// <summary>
        /// Getting products in the cart with total, if empty it generates an Id that identifies the cart
        /// </summary>
        /// <returns>A cart object with Id, discounts, products and total </returns>
        [HttpGet]
        public async Task<ActionResult<CartResponse>> GetCartAsync()
        {
            return await GetCartAsync(null);
        }

        /// <summary>
        /// Add products to cart
        /// </summary>
        /// <returns>Shopping cart</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/cart/TX9HA4IE
        ///     {
        ///        "products": [
        ///             {
        ///                 "productCode": "itco",
        ///                 "qty": 1
        ///             }
        ///        ]
        ///     }
        ///
        /// </remarks>
        /// <param name="cartCode">Shopping cart id</param>
        /// <param name="cartRequest">Shopping cart id</param>
        /// <response code="200">Returns the cart</response>
        [HttpPost("{cartCode}")]
        public async Task<ActionResult<CartResponse>> PostProductsCartAsync(string cartCode, [FromBody] CartRequest cartRequest)
        {
            var cart = await GetCartOrFailNotFoundClosed(cartCode);
            var cartPushed = _mapper.Map<CartModel>(cartRequest);

            try
            {
                cart.AddProducts(await _cartService.PopolateProductPrice(cartPushed.Products));
            } 
            catch (CartProductExistException ex)
            {
                throw new HttpStatusException(HttpStatusCode.Conflict, ex.Message);
            }

            await _cartService.Save(cart);
            
            return Created($"{cartCode}", _mapper.Map<CartResponse>(cart));
        }

        private async Task<CartModel> GetCartOrFailNotFoundClosed(string cartCode)
        {
            var cart = await _cartService.GetAsync(cartCode);
            if (cart == null) throw new HttpStatusException(HttpStatusCode.NotFound, "Cart not found");

            if (!cart.IsActive) throw new HttpStatusException(HttpStatusCode.BadRequest, "Cart is closed");

            return cart;
        }

        private async Task<CartModel> GetCartOrFailClosed(string cartCode)
        {
            var cart = await _cartService.GetAsync(cartCode);

            if (cart != null && !cart.IsActive) throw new HttpStatusException(HttpStatusCode.BadRequest, "Cart is closed");

            return cart;
        }

        /// <summary>
        /// Update product qty
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PATCH /api/cart/TX9HA4IE/products/itco
        ///     {
        ///        "qty": 3
        ///     }
        ///
        /// </remarks>
        /// <param name="cartCode">Shopping cart id</param>
        /// <param name="productCode">Product to be updated</param>
        /// <param name="productCartUpdateQtyRequest">Contains the new quantity for the provided product</param>
        /// <returns></returns>
        [HttpPatch("{cartCode}/products/{productCode}")]
        public async Task<ActionResult<CartResponse>> PatchProductCartUpdateQtyAsync(string cartCode, string productCode, [FromBody] ProductCartUpdateQtyRequest productCartUpdateQtyRequest)
        {
            var cart = await GetCartOrFailNotFoundClosed(cartCode);
            cart.UpdateProduct(productCode, productCartUpdateQtyRequest.Qty);
            await _cartService.Save(cart);
            return Ok(_mapper.Map<CartResponse>(cart));
        }

        /// <summary>
        /// apply discount, available discount codes are: "fix" to get -10EUR and "perc" to get -10%
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/cart/TX9HA4IE/discount
        ///     {
        ///        "discountCode": "fix"
        ///     }
        ///
        /// </remarks>
        /// <param name="cartCode">Shopping cart id</param>
        /// <param name="applyDiscountEditModel"></param>
        /// <returns></returns>
        [HttpPost("{cartCode}/discount")]
        public async Task<ActionResult<CartResponse>> PostApplyDiscountCart(string cartCode, [FromBody] ApplyDiscountRequest applyDiscountEditModel)
        {
            var cart = await _cartService.ApplyDiscountAsync(await GetCartOrFailNotFoundClosed(cartCode), applyDiscountEditModel.DiscountCode);
            return Ok(_mapper.Map<CartResponse>(cart));
        }

        /// <summary>
        /// Getting payment methods available for the provided shopping cart
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/cart/TX9HA4IE/payment-methods
        ///
        /// </remarks>
        /// <param name="cartCode">Shopping cart id</param>
        /// <returns>An IEnumerable with awailable payment methods</returns>
        [HttpGet("{cartCode}/payment-methods")]
        public async Task<ActionResult<IEnumerable<PaymentMethodResponse>>> GetPaymentMethodsAsync(string cartCode)
        {
            var cart = await GetCartOrFailNotFoundClosed(cartCode);

            return Ok(_mapper.Map<IEnumerable<PaymentMethodResponse>>(await _paymentMethodService.GetAsync(cart)));
        }

        // pay
        /// <summary>
        /// Payment of the cart, if cash is selected and sum is less or equal 10 it closes the shopping cart, if credit is selected closes the cart. The shopping cart is closed when IsActive=false.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/cart/TX9HA4IE/complete-payment
        ///     {
        ///        "paymentCode": "creditCard"
        ///     }
        ///
        /// </remarks>
        /// <param name="cartCode">Shopping cart id</param>
        /// <param name="payEditModel"></param>
        /// <returns></returns>
        [HttpPost("{cartCode}/complete-payment")]
        public async Task<IActionResult> PayAsync(string cartCode, [FromBody] PayRequest payEditModel)
        {
            var cart = await GetCartOrFailNotFoundClosed(cartCode);

            if(!await _paymentMethodService.CheckPaymentAvailable(cart, payEditModel.PaymentCode)) throw new HttpStatusException(HttpStatusCode.BadRequest, "Payment method is not accepted");

            await _cartService.PayCart(cart, payEditModel.PaymentCode);

            return Ok("Payment successful");
        }
    }
}
