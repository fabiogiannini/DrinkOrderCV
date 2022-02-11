namespace DrinkOrderCV.Web.ViewModels
{
    public class PaymentMethodResponse
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal? Threshold { get; set; }
    }
}