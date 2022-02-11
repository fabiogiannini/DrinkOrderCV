namespace DrinkOrderCV.Core
{
    public class DiscountModel : BaseModel
    {
        public string Name { get; set; }   
        public decimal DiscountValue { get; set; }   
        public DiscountType Type { get; set; }   

        public enum DiscountType
        {
            Percentual = 0,
            Fixed = 1
        }
    }
}