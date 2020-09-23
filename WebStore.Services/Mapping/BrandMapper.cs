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
        
        public static Brand FromDto(this BrandDto p) => new Brand()
        {
            Id = p.Id,
            Name = p.Name
        };
    }
}