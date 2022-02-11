namespace DrinkOrderCV.Core
{
    public class CartModel : BaseModel
    {
        public CartModel()
        {
            IsActive = true; 
            Products = new List<CartProductModel>();
        }

        public string? DiscountCode { get; set; }
        public string? PaymentMethodCode { get; set; }
        public decimal DiscountValue { get; set; }
        public int? DiscountType { get; set; }
        public List<CartProductModel> Products { get; protected set; }
        public bool IsActive { get; set; }
        public void SetActive(bool isActive)
        {
            IsActive = isActive;
        }

        public decimal Total 
        {
            get
            {
                return ApplyDiscount(TotalProducts);
            }
        }

        private decimal TotalProducts
        {
            get
            {
                if (Products == null) return 0;

                return Products.Sum(cp => cp.Qty * cp.Price);
            }
        }

        private decimal ApplyDiscount(decimal totalProducts)
        {
            if (IsDiscountPercentual()) return CalculateDiscountPercentual(totalProducts);

            if (IsDiscountFixed()) return CalculateDiscountFixed(totalProducts);

            return totalProducts;
        }

        private bool IsDiscountPercentual()
        {
            return DiscountType == 0;
        }

        private bool IsDiscountFixed()
        {
            return DiscountType == 1;
        }

        private decimal CalculateDiscountPercentual(decimal totalProducts)
        {
            return CalculateTotalPositive(totalProducts * (100 - DiscountValue) * 0.01m);
        }

        private decimal CalculateDiscountFixed(decimal totalProducts)
        {
            return CalculateTotalPositive(totalProducts - DiscountValue);
        }

        private decimal CalculateTotalPositive(decimal total)
        {
            return total > 0 ? total : 0;
        }

        public void SetDiscount(DiscountModel discount)
        {
            DiscountCode = discount.Code;
            DiscountType = (int)discount.Type;
            DiscountValue = discount.DiscountValue;
        }

        public void AddProducts(IEnumerable<CartProductModel> products)
        {
            foreach(var product in products)
            {
                if (ProductExist(product)) throw new CartProductExistException(product);
                
                Products.Add(product);
            }
        }

        private bool ProductExist(CartProductModel product)
        {
            return Products.Any(p => p.ProductCode == product.ProductCode);
        }

        public void UpdateProduct(string productCode, decimal qty)
        {
            var product = GetProduct(productCode);

            if(product != null)
            {
                product.Qty = qty;
            }
        }

        private CartProductModel GetProduct(string productCode)
        {
            return Products.FirstOrDefault(p => p.ProductCode == productCode);
        }
    }
}