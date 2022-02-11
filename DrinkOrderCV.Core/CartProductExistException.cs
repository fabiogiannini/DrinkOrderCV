using System.Runtime.Serialization;

namespace DrinkOrderCV.Core
{
    [Serializable]
    public class CartProductExistException : Exception
    {
        public CartProductExistException()
        {
           
        }

        public CartProductExistException(CartProductModel product) : base($"Product {product.ProductCode} already in the cart")
        {       
        }

        public CartProductExistException(string? message) : base(message)
        {
        }

        public CartProductExistException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CartProductExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}