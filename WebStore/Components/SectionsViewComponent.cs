using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Components
{
    //[ViewComponent(Name = "Section")]
    public class SectionsViewComponent : ViewComponent
    {
        private readonly IProductData _productData;

        public SectionsViewComponent(IProductData ProductData) => _productData = ProductData;

        public IViewComponentResult Invoke(string SectionId)
        {
            var sectionId = int.TryParse(SectionId, out var id) ? id : (int?) null;

            var sections = GetSections(sectionId, out var parentSectionId);

            return View(new SelectableSectionsViewModel
            {
                Sections = sections,
                CurrentSectionId = sectionId,
                ParentSectionId = parentSectionId
            });
        }

        private IEnumerable<SectionViewModel> GetSections(int? SectionId, out int? ParentSectionId)
        {
            ParentSectionId = null;

            var sections = _productData.GetSections().ToArray();

            var parentSections = sections.Where(s => s.ParentId is null);

            var parentSectionsViews = parentSections
                .Select(s => new SectionViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Order = s.Order
                })
                .ToList();

            foreach (var parentSection in parentSectionsViews)
            {
                var childs = sections.Where(s => s.ParentId == parentSection.Id);

                foreach (var childSection in childs)
                {
                    if (childSection.Id == SectionId)
                        ParentSectionId = childSection.ParentId;

                    parentSection.ChildSections.Add(new SectionViewModel
                    {
                        Id = childSection.Id,
                        Name = childSection.Name,
                        Order = childSection.Order,
                        ParentSection = parentSection
                    });
                }

                parentSection.ChildSections.Sort((a, b) => Comparer<double>.Default.Compare(a.Order, b.Order));
            }

            parentSectionsViews.Sort((a, b) => Comparer<double>.Default.Compare(a.Order, b.Order));
            return parentSectionsViews;
        }
    }
}