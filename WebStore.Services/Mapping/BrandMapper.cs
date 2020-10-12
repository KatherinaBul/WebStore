using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.Dto;
using WebStore.Domain.Entities;

namespace WebStore.Services.Mapping
{
    public static class BrandMapper
    {
        public static BrandDto ToDto(this Brand b) => new BrandDto()
        {
            Id = b.Id,
            Name = b.Name,
            ProductsCount = b.Products.Count,
        };
        
        public static IEnumerable<BrandDto> ToDto(this IEnumerable<Brand> brands) => brands.Select(ToDto);

        public static Brand FromDto(this BrandDto b) => new Brand()
        {
            Id = b.Id,
            Name = b.Name
        };
        
        public static IEnumerable<Brand> FromDto(this IEnumerable<BrandDto> brands) => brands.Select(FromDto);
    }
}