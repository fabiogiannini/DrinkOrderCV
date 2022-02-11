using System.ComponentModel.DataAnnotations;

namespace DrinkOrderCV.Web.Controllers
{
    public class PayRequest
    {
        [Required]
        public string PaymentCode { get; set; }
    }
}