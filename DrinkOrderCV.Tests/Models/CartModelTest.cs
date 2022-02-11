using DrinkOrderCV.Core;
using NUnit.Framework;
using System.Collections.Generic;
using static DrinkOrderCV.Core.DiscountModel;

namespace DrinkOrderCV.Tests.Models
{
    [TestFixture]
    public class CartModelTest
    {
        [Test]
        public void CartIsActive_WhenCreated()
        {
            var cart = new CartModel();
            Assert.IsTrue(cart.IsActive);
        }

        [Test]
        public void CartProductsNotNull_WhenCreated()
        {
            var cart = new CartModel();
            Assert.IsNotNull(cart.Products);
            Assert.IsEmpty(cart.Products);
        }

        [Test]
        public void CartHasProducts_WhenAdded()
        {
            var cart = new CartModel();
            var cartProducts = new List<CartProductModel>
            {
                new CartProductModel { ProductCode = "itco", Qty = 1, Price = 4 }
            };
            cart.AddProducts(cartProducts);

            Assert.IsNotNull(cart.Products);
            Assert.IsNotEmpty(cart.Products);
        }

        [Test]
        public void CartTotalIsCaluculated_WhenHasProducts()
        {
            var cart = new CartModel();
            var cartProducts = new List<CartProductModel>
            {
                new CartProductModel { ProductCode = "itco", Qty = 1, Price = 4 },
                new CartProductModel { ProductCode = "amco", Qty = 2, Price = 3 },
            };
            cart.AddProducts(cartProducts);

            Assert.IsNotNull(cart.Products);
            Assert.IsNotEmpty(cart.Products);
            Assert.AreEqual(cart.Total, 10);
        }

        [Test]
        public void CartTotalIsZero_WhenHasDiscountFixedGreaterThanTotal()
        {
            var cart = new CartModel();
            var cartProducts = new List<CartProductModel>
            {
                new CartProductModel { ProductCode = "itco", Qty = 1, Price = 4 },
            };
            cart.AddProducts(cartProducts);
            cart.SetDiscount(new DiscountModel { Code = "a", DiscountValue = 5, Name = "discount", Type = DiscountType.Fixed });

            Assert.IsNotNull(cart.Products);
            Assert.IsNotEmpty(cart.Products);
            Assert.AreEqual(cart.Total, 0);
        }

        [Test]
        public void CartTotalIsCaluculated_WhenHasProductsAndPercentualDiscount()
        {
            var cart = new CartModel();
            var cartProducts = new List<CartProductModel>
            {
                new CartProductModel { ProductCode = "itco", Qty = 1, Price = 4 },
                new CartProductModel { ProductCode = "amco", Qty = 2, Price = 3 },
            };
            cart.AddProducts(cartProducts);
            cart.SetDiscount(new DiscountModel { Code = "a", DiscountValue = 50, Name = "discount", Type = DiscountType.Percentual });

            Assert.IsNotNull(cart.Products);
            Assert.IsNotEmpty(cart.Products);
            Assert.AreEqual(cart.Total, 5);
        }

        [Test]
        public void CartTotalIsCaluculated_WhenHasProductsAndFixedDiscount()
        {
            var cart = new CartModel();
            var cartProducts = new List<CartProductModel>
            {
                new CartProductModel { ProductCode = "itco", Qty = 1, Price = 4 },
                new CartProductModel { ProductCode = "amco", Qty = 2, Price = 3 },
            };
            cart.AddProducts(cartProducts);
            cart.SetDiscount(new DiscountModel { Code = "a", DiscountValue = 5, Name = "discount", Type = DiscountType.Fixed });

            Assert.IsNotNull(cart.Products);
            Assert.IsNotEmpty(cart.Products);
            Assert.AreEqual(cart.Total, 5);
        }

        [Test]
        public void CartTotalIsCaluculated_WhenProductIsUpdated()
        {
            var cart = new CartModel();
            var cartProducts = new List<CartProductModel>
            {
                new CartProductModel { ProductCode = "itco", Qty = 1, Price = 4 },
            };
            cart.AddProducts(cartProducts);

            cart.UpdateProduct("itco", 2);

            Assert.IsNotNull(cart.Products);
            Assert.IsNotEmpty(cart.Products);
            Assert.AreEqual(cart.Total, 8);
        }

        [Test]
        public void CartTotalIsZero_WhenHasNoProduct()
        {
            var cart = new CartModel();
            Assert.AreEqual(cart.Total, 0);
        }
    }
}
