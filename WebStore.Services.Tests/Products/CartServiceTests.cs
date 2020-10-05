using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Domain;
using WebStore.Domain.Dto;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Products;
using Assert = Xunit.Assert;

namespace WebStore.Services.Tests.Products
{
    [TestClass]
    public class CartServiceTests
    {
        private Cart _cart;

        private Mock<IProductData> _productDataMock;
        private Mock<ICartStore> _cartStoreMock;

        /// <summary>Тестируемый сервис</summary>
        private ICartService _cartService;

        [TestInitialize]
        public void TestInitialize()
        {
            _cart = new Cart
            {
                Items = new List<CartItem>
                {
                    new CartItem {ProductId = 1, Quantity = 1},
                    new CartItem {ProductId = 2, Quantity = 3}
                }
            };

            _productDataMock = new Mock<IProductData>();
            _productDataMock
                .Setup(c => c.GetProducts(It.IsAny<ProductFilter>()))
                .Returns(new List<ProductDto>
                {
                    new ProductDto
                    {
                        Id = 1,
                        Name = "Product 1",
                        Price = 1.1m,
                        Order = 0,
                        ImageUrl = "Product1.png",
                        Brand = new BrandDto {Id = 1, Name = "Brand 1"},
                        Section = new SectionDto {Id = 1, Name = "Section 1"}
                    },
                    new ProductDto
                    {
                        Id = 2,
                        Name = "Product 2",
                        Price = 2.2m,
                        Order = 0,
                        ImageUrl = "Product2.png",
                        Brand = new BrandDto {Id = 2, Name = "Brand 2"},
                        Section = new SectionDto {Id = 2, Name = "Section 2"}
                    },
                });

            _cartStoreMock = new Mock<ICartStore>();
            _cartStoreMock.Setup(c => c.Cart).Returns(_cart);
            _cartService = new CartService(_productDataMock.Object, _cartStoreMock.Object);
        }

        [TestMethod]
        public void Cart_Class_ItemsCount_returns_Correct_Quantity()
        {
            var cart = new Cart
            {
                Items = new List<CartItem>
                {
                    new CartItem {ProductId = 1, Quantity = 1},
                    new CartItem {ProductId = 2, Quantity = 3},
                }
            };

            const int expectedCount = 4;

            var actualCount = cart.ItemsCount;

            Assert.Equal(expectedCount, actualCount);
        }

        [TestMethod]
        public void CartViewModel_Returns_Correct_ItemsCount()
        {
            var cartViewModel = new CartViewModel
            {
                Items = new[]
                {
                    (new ProductViewModel {Id = 1, Name = "Product 1", Price = 0.5m}, 1),
                    (new ProductViewModel {Id = 2, Name = "Product 2", Price = 1.5m}, 3),
                }
            };

            const int expectedCount = 4;

            var actualCount = cartViewModel.ItemsCount;

            Assert.Equal(expectedCount, actualCount);
        }

        [TestMethod]
        public void CartService_AddToCart_WorkCorrect()
        {
            _cart.Items.Clear();

            const int expectedId = 5;

            _cartService.AddToCart(expectedId);

            Assert.Equal(1, _cart.ItemsCount);

            Assert.Single(_cart.Items);
            Assert.Equal(expectedId, _cart.Items.First().ProductId);
        }

        [TestMethod]
        public void CartService_RemoveFromCart_Remove_Correct_Item()
        {
            const int itemId = 1;

            _cartService.RemoveFromCart(itemId);

            Assert.Single(_cart.Items);
            Assert.Equal(2, _cart.Items.Single().ProductId);
        }

        [TestMethod]
        public void CartService_Clear_ClearCart()
        {
            _cartService.Clear();

            Assert.Empty(_cart.Items);
        }

        [TestMethod]
        public void CartService_Decrement_Correct()
        {
            const int itemId = 2;

            _cartService.DecrementFromCart(itemId);

            Assert.Equal(3, _cart.ItemsCount);
            Assert.Equal(2, _cart.Items.Count);
            var items = _cart.Items.ToArray();
            Assert.Equal(itemId, items[1].ProductId);
            Assert.Equal(2, items[1].Quantity);
        }

        [TestMethod]
        public void CartService_Remove_Item_When_Decrement_to_0()
        {
            const int itemId = 1;

            _cartService.DecrementFromCart(itemId);

            Assert.Equal(3, _cart.ItemsCount);
            Assert.Single(_cart.Items);
        }

        [TestMethod]
        public void CartService_TransformFromCart_WorkCorrect()
        {
            var result = _cartService.TransformFromCart();

            Assert.Equal(4, result.ItemsCount);
            Assert.Equal(1.1m, result.Items.First().Product.Price);
        }
    }
}