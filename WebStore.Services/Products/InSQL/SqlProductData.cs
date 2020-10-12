using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain;
using WebStore.Domain.Dto;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Services.Products.InSQL
{
    public class SqlProductData : IProductData
    {
        private readonly WebStoreDB _db;

        public SqlProductData(WebStoreDB db) => _db = db;

        public IEnumerable<SectionDto> GetSections() => _db.Sections.ToDto();

        public SectionDto GetSectionById(int id) => _db.Sections.Find(id).ToDto();

        public IEnumerable<BrandDto> GetBrands() => _db.Brands.Include(b => b.Products).ToDto();

        public BrandDto GetBrandById(int id) => _db.Brands
            .Include(b => b.Products)
            .FirstOrDefault(b => b.Id == id)
            .ToDto();

        public PageProductsDto GetProducts(ProductFilter Filter = null)
        {
            IQueryable<Product> query = _db.Products
                .Include(product => product.Brand)
                .Include(product => product.Section);

            if (Filter?.Ids?.Length > 0)
                query = query.Where(product => Filter.Ids.Contains(product.Id));
            else
            {
                if (Filter?.BrandId != null)
                    query = query.Where(product => product.BrandId == Filter.BrandId);

                if (Filter?.SectionId != null)
                    query = query.Where(product => product.SectionId == Filter.SectionId);
            }

            var totalCount = query.Count();

            if (Filter?.PageSize > 0)
                query = query
                    .Skip((Filter.Page - 1) * (int) Filter.PageSize)
                    .Take((int) Filter.PageSize);


            return new PageProductsDto {Products = query.AsEnumerable().ToDto(), TotalCount = totalCount};
        }

        public ProductDto GetProductById(int id) => _db.Products
            .Include(product => product.Brand)
            .Include(product => product.Section)
            .FirstOrDefault(product => product.Id == id).ToDto();
    }
}