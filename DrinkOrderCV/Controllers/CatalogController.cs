using DrinkOrderCV.Core;
using DrinkOrderCV.Core.Repositories;
using DrinkOrderCV.Web.Models.Reponse;
using Microsoft.AspNetCore.Mvc;

namespace DrinkOrderCV.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IRepository<ProductModel> _productRepository;

        public CatalogController(IRepository<ProductModel> productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// Get products from the catalog
        /// </summary>
        /// <returns>An IEnumerable with catolog's products</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProducts()
        {
            return Ok(await _productRepository.GetAsync());
        }
    }
}
