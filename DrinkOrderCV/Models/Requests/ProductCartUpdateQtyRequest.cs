using System.ComponentModel.DataAnnotations;

namespace DrinkOrderCV.Web.Controllers
{
    public class ProductCartUpdateQtyRequest
    {
        [Required]
        public decimal Qty { get; set; }
    }
}