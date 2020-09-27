using System.Collections.Generic;
using System.Linq;
using WebStore.Domain;
using WebStore.Domain.Dto;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.Services.Data;
using WebStore.Services.Mapping;

namespace WebStore.Services.Products.InMemory
{
    public class InMemoryProductData : IProductData
    {
        public IEnumerable<SectionDto> GetSections() => TestData.Sections.ToDto();

        public IEnumerable<BrandDto> GetBrands() => TestData.Brands.ToDto();

        public IEnumerable<ProductDto> GetProducts(ProductFilter Filter = null)
        {
            var query = TestData.Products;

            if (Filter?.SectionId != null)
                query = query.Where(product => product.SectionId == Filter.SectionId);

            if (Filter?.BrandId != null)
                query = query.Where(product => product.BrandId == Filter.BrandId);

            return query.ToDto();
        }

        public ProductDto GetProductById(int id) => TestData.Products.FirstOrDefault(p => p.Id == id).ToDto();
    }
}
