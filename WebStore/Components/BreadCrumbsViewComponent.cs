using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Components
{
    public class BreadCrumbsViewComponent:ViewComponent
    {
        private readonly IProductData _productData;

        public BreadCrumbsViewComponent(IProductData productData) => _productData = productData;

        public IViewComponentResult Invoke()
        {
            var model = new BreadCrumbViewModel();

            // todo:Извлечение данных по модели и по секции по их идентификаторам


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