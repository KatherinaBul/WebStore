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

        /// <summary>
        /// Получить категории товаров
        /// </summary>
        /// <returns>Список категорий товаров</returns>
        [HttpGet("sections")]
        public IEnumerable<SectionDto> GetSections() => _productData.GetSections();

        [HttpGet("sections/{id}")]
        public SectionDto GetSectionById(int id) => _productData.GetSectionById(id);

        /// <summary>
        /// Получить список брендов
        /// </summary>
        /// <returns>Список брендов</returns>
        [HttpGet("brands")]
        public IEnumerable<BrandDto> GetBrands() => _productData.GetBrands();

        [HttpGet("brands/{id}")]
        public BrandDto GetBrandById(int id) => _productData.GetBrandById(id);

        /// <summary>
        /// Получить товары в соответствии с фильтром
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Список товаров</returns>
        [HttpPost]
        public IEnumerable<ProductDto> GetProducts(ProductFilter filter = null) => _productData.GetProducts(filter);

        /// <summary>
        /// Получить информацию о товаре по его идентифиакатору
        /// </summary>
        /// <param name="id">Идентификатор товара</param>
        /// <returns>Информация о товаре</returns>
        [HttpGet("{id:int}")]
        public ProductDto GetProductById(int id) => _productData.GetProductById(id);
    }
}