using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Dto
{
    public class SectionDto:INamedEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}