using System.ComponentModel.DataAnnotations;

namespace DrinkOrderCV.Web.ViewModels
{
    public class CartProductRequest
    {
        [Required]
        public string ProductCode { get; set; }
        [Required]
        public decimal Qty { get; set; }
    }
}