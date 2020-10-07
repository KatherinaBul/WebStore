using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Components
{
    public class BreadCrumbsViewComponent:ViewComponent
    {
        private readonly IProductData _productData;

        public BreadCrumbsViewComponent(IProductData productData) => _productData = productData;

        public IViewComponentResult Invoke()
        {
            var model = new BreadCrumbViewModel();

            if (int.TryParse(Request.Query["SectionId"], out var sectionId))
                model.Section = _productData.GetSectionById(sectionId).FromDto();

            if (int.TryParse(Request.Query["BrandId"], out var brandId))
                model.Brand = _productData.GetBrandById(brandId).FromDto();

            if (int.TryParse(ViewContext.RouteData.Values["id"]?.ToString(), out var productId))
            {
                var product = _productData.GetProductById(productId);
                if (product != null)
                    model.Product = product.Name;
            }

            return View(model);
        }
    }
}