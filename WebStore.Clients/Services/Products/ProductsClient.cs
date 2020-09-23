using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using WebStore.Clients.Base;
using WebStore.Domain;
using WebStore.Domain.Dto;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Services.Products
{
    public class ProductsClient : BaseClient, IProductData
    {
        public ProductsClient(IConfiguration configuration) : base(configuration, "api/products")
        {
        }

        public IEnumerable<SectionDto> GetSections() => Get<IEnumerable<SectionDto>>($"{ServiceAddress}/sections");


        public IEnumerable<BrandDto> GetBrands() => Get<IEnumerable<BrandDto>>($"{ServiceAddress}/brands");

        public IEnumerable<ProductDto> GetProducts(ProductFilter Filter = null) =>
            Post(ServiceAddress, Filter ?? new ProductFilter())
                .Content
                .ReadAsAsync<IEnumerable<ProductDto>>()
                .Result;

        public ProductDto GetProductById(int id) => Get<ProductDto>($"{ServiceAddress}/{id}");
    }
}