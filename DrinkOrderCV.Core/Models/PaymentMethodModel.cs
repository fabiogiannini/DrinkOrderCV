namespace DrinkOrderCV.Core
{
    public class PaymentMethodModel : BaseModel
    {
        public string Name { get; set; }   
        public decimal? Threshold { get; set; }
    }
}