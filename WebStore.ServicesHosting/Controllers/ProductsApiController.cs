using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;
using WebStore.Domain.Dto;
using WebStore.Interfaces.Services;

namespace WebStore.ServicesHosting.Controllers
{
    [Produces("application/json")]
    [Route(WebApi.Products)]
    [ApiController]
    public class ProductsApiController : Controller, IProductData
    {
        private readonly IProductData _productData;

        public ProductsApiController(IProductData productData) => _productData = productData;

        [HttpGet("sections")]
        public IEnumerable<SectionDto> GetSections() => _productData.GetSections();

        [HttpGet("brands")]
        public IEnumerable<BrandDto> GetBrands() => _productData.GetBrands();

        [HttpPost]
        public IEnumerable<ProductDto> GetProducts(ProductFilter filter = null) => _productData.GetProducts(filter);

        [HttpGet("{id:int}")]
        public ProductDto GetProductById(int id) => _productData.GetProductById(id);
    }
}