using System.Collections.Generic;

namespace WebStore.Domain.Dto
{
    public class PageProductsDto
    {
        public IEnumerable<ProductDto> Products { get; set; }

        public int TotalCount { get; set; }
    }
}