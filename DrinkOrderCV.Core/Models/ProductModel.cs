namespace DrinkOrderCV.Core
{
    public class ProductModel : BaseModel
    {
        public string Name { get; set; }   
        public string Currency { get; set; }   
        public decimal Price { get; set; }
    }
}