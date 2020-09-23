using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.Dto;
using WebStore.Domain.Entities;

namespace WebStore.Services.Mapping
{
    public static class BrandMapper
    {
        public static BrandDto ToDto(this Brand p) => new BrandDto()
        {
            Id = p.Id,
            Name = p.Name
        };
        
        public static IEnumerable<BrandDto> ToDto(this IEnumerable<Brand> brands) => brands.Select(ToDto);

        public static Brand FromDto(this BrandDto p) => new Brand()
        {
            Id = p.Id,
            Name = p.Name
        };
        
        public static IEnumerable<Brand> FromDto(this IEnumerable<BrandDto> brands) => brands.Select(FromDto);
    }
}