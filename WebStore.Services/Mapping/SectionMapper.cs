using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.Dto;
using WebStore.Domain.Entities;

namespace WebStore.Services.Mapping
{
    public static class SectionMapper
    {
        public static SectionDto ToDto(this Section p) => new SectionDto()
        {
            Id = p.Id,
            Name = p.Name
        };
        public static IEnumerable<SectionDto> ToDto(this IEnumerable<Section> products) => products.Select(ToDto);
        
        public static Section FromDto(this SectionDto p) => new Section()
        {
            Id = p.Id,
            Name = p.Name
        };
        
        public static IEnumerable<Section> FromDto(this IEnumerable<SectionDto> products) => products.Select(FromDto);
    }
}