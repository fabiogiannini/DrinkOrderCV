using System.ComponentModel.DataAnnotations;

namespace DrinkOrderCV.Web.ViewModels
{
    public class ApplyDiscountRequest
    {
        [Required]
        public string DiscountCode { get; set; }
    }
}