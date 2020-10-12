using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Controllers;
using WebStore.Domain;
using WebStore.Domain.Dto;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class CatalogControllerTests
    {
        [TestMethod]
        public void Details_Returns_with_Correct_View()
        {
            // A - A - A = Arrange - Act - Assert

            #region Arrange

            const int expectedProductId = 1;
            const decimal expectedPrice = 10m;

            var expectedName = $"Product id {expectedProductId}";
            var expectedBrandName = $"Brand of product {expectedProductId}";

            var productDataMock = new Mock<IProductData>();
            productDataMock
                .Setup(p => p.GetProductById(It.IsAny<int>()))
                .Returns<int>(id => new ProductDto
                {
                    Id = id,
                    Name = $"Product id {id}",
                    ImageUrl = $"img{id}.png",
                    Order = 1,
                    Price = expectedPrice,
                    Brand = new BrandDto
                    {
                        Id = 1,
                        Name = $"Brand of product {id}"
                    },
                    Section = new SectionDto
                    {
                        Id = 1,
                        Name = $"Section of product {id}"
                    }
                });

            var configurationMock = new Mock<IConfiguration>();
            configurationMock
                .Setup(cfg => cfg[It.IsAny<string>()])
                .Returns("3");

            var controller = new CatalogController(productDataMock.Object, configurationMock.Object);

            #endregion

            #region Act

            var result = controller.Details(expectedProductId);

            #endregion

            #region Assert

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ProductViewModel>(viewResult.Model);

            Assert.Equal(expectedProductId, model.Id);
            Assert.Equal(expectedName, model.Name);
            Assert.Equal(expectedPrice, model.Price);
            Assert.Equal(expectedBrandName, model.Brand);

            #endregion
        }

        [TestMethod]
        public void Shop_Returns_CorrectView()
        {
            var products = new[]
            {
                new ProductDto
                {
                    Id = 1,
                    Name = "Product 1",
                    Order = 0,
                    Price = 10m,
                    ImageUrl = "Product1.png",
                    Brand = new BrandDto
                    {
                        Id = 1,
                        Name = "Brand of product 1"
                    },
                    Section = new SectionDto
                    {
                        Id = 1,
                        Name = "Section of product 1"
                    }
                },
                new ProductDto
                {
                    Id = 2,
                    Name = "Product 2",
                    Order = 0,
                    Price = 20m,
                    ImageUrl = "Product2.png",
                    Brand = new BrandDto
                    {
                        Id = 2,
                        Name = "Brand of product 2"
                    },
                    Section = new SectionDto
                    {
                        Id = 2,
                        Name = "Section of product 2"
                    }
                },
            };

            var productDataMock = new Mock<IProductData>();
            productDataMock
                .Setup(p => p.GetProducts(It.IsAny<ProductFilter>()))
                .Returns(new PageProductsDto
                {
                    Products = products,
                    TotalCount = products.Length
                });

            const int expectedSectionId = 1;
            const int expectedBrandId = 5;

            var configurationMock = new Mock<IConfiguration>();
            configurationMock
                .Setup(cfg => cfg[It.IsAny<string>()])
                .Returns("3");

            var controller = new CatalogController(productDataMock.Object, configurationMock.Object);

            var result = controller.Shop(expectedBrandId, expectedSectionId);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CatalogViewModel>(viewResult.ViewData.Model);

            Assert.Equal(products.Length, model.Products.Count());
            Assert.Equal(expectedBrandId, model.BrandId);
            Assert.Equal(expectedSectionId, model.SectionId);
        }
    }
}