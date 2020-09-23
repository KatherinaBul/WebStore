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
        
        public static Section FromDto(this SectionDto p) => new Section()
        {
            Id = p.Id,
            Name = p.Name
        };
    }
}