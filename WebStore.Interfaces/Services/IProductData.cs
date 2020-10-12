using System.Collections.Generic;
using WebStore.Domain;
using WebStore.Domain.Dto;
using WebStore.Domain.Entities;

namespace WebStore.Interfaces.Services
{
    public interface IProductData
    {
        IEnumerable<SectionDto> GetSections();
        SectionDto GetSectionById(int id);

        IEnumerable<BrandDto> GetBrands();

        BrandDto GetBrandById(int id);

        PageProductsDto GetProducts(ProductFilter Filter = null);

        ProductDto GetProductById(int id);
    }
}