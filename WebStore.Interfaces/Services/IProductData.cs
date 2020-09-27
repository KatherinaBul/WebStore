using System.Collections.Generic;
using WebStore.Domain;
using WebStore.Domain.Dto;
using WebStore.Domain.Entities;

namespace WebStore.Interfaces.Services
{
    public interface IProductData
    {
        IEnumerable<SectionDto> GetSections();

        IEnumerable<BrandDto> GetBrands();

        IEnumerable<ProductDto> GetProducts(ProductFilter Filter = null);

        ProductDto GetProductById(int id);
    }
}
