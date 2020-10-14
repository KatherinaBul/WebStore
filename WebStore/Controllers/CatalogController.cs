using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebStore.Domain;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData _productData;
        private readonly IConfiguration _configuration;
        private const string _pageSize = "PageSize";

        public CatalogController(IProductData productData, IConfiguration configuration)
        {
            _productData = productData;
            _configuration = configuration;
        }

        public IActionResult Shop(int? brandId, int? sectionId, int page = 1)
        {
            var pageSize = int.TryParse(_configuration[_pageSize], out var size)
                ? size
                : (int?) null;

            var filter = new ProductFilter
            {
                BrandId = brandId,
                SectionId = sectionId,
                Page = page,
                PageSize = pageSize
            };

            var products = _productData.GetProducts(filter);

            return View(new CatalogViewModel
            {
                SectionId = sectionId,
                BrandId = brandId,
                Products = products.Products.FromDto().ToView().OrderBy(p => p.Order),
                PageViewModel = new PageViewModel
                {
                    PageSize = pageSize ?? 0,
                    PageNumber = page,
                    TotalItems = products.TotalCount
                }
            });
        }

        public IActionResult Details(int id)
        {
            var product = _productData.GetProductById(id);

            if (product is null)
                return NotFound();

            return View(product.FromDto().ToView());
        }


        #region API

        public IActionResult GetCatalogHtml(int? brandId, int? sectionId, int page) =>
            PartialView("Partial/_FeaturesItems", GetProducts(brandId, sectionId, page));

        private IEnumerable<ProductViewModel> GetProducts(int? brandId, int? sectionId, in int page) =>
            _productData.GetProducts(
                    new ProductFilter
                    {
                        SectionId = sectionId,
                        BrandId = brandId,
                        Page = page,
                        PageSize = int.Parse(_configuration[_pageSize])
                    }).Products
                .FromDto()
                .ToView()
                .OrderBy(p => p.Order);

        #endregion
    }
}