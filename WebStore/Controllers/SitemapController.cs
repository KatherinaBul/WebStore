using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SimpleMvcSitemap;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    public class SitemapController : Controller
    {
        IActionResult Index([FromServices] IProductData productData)
        {
            var nodes = new List<SitemapNode>
            {
                new SitemapNode(Url.Action("Index", "Home")),
                new SitemapNode(Url.Action("ContactUs", "Home")),
                new SitemapNode(Url.Action("Blogs", "Home")),
                new SitemapNode(Url.Action("BlogSingle", "Home")),
                new SitemapNode(Url.Action("Shop", "Catalog")),
                new SitemapNode(Url.Action("Index", "WebAPITest")),
            };

            nodes.AddRange(productData.GetSections()
                .Select(s => new SitemapNode(Url.Action("Shop", "Catalog", new {SectionId = s.Id}))));

            nodes.AddRange(productData.GetBrands().Select(brand =>
                new SitemapNode(Url.Action("Shop", "Catalog", new {BrandId = brand.Id}))));

            nodes.AddRange(productData.GetProducts().Products
                .Select(product => new SitemapNode(Url.Action("Details", "Catalog", new {product.Id}))));

            return new SitemapProvider().CreateSitemap(new SitemapModel(nodes));
        }
    }
}