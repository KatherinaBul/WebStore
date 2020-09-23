using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.ServicesHosting.Controllers
{
    [Produces("application/json")]
    [Route("api/products")]
    [ApiController]
    public class ProductsApiController : Controller, IProductData
    {
        private readonly IProductData _productData;

        public ProductsApiController(IProductData productData) => _productData = productData;

        [HttpGet("sections")]
        public IEnumerable<Section> GetSections() => _productData.GetSections();

        [HttpGet("brands")]
        public IEnumerable<Brand> GetBrands() => _productData.GetBrands();

        [HttpPost("products")]
        public IEnumerable<Product> GetProducts(ProductFilter filter = null) => _productData.GetProducts(filter);

        [HttpGet("products/{id:int}")]
        public Product GetProductById(int id) => _productData.GetProductById(id);
    }
}