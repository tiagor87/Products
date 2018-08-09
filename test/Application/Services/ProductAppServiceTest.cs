using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Products.Application.Adapters;
using Products.Application.Contracts;
using Products.Application.Services;
using Products.Domain.Models;
using Products.Domain.Repositories;

namespace Products.Application.Tests.Services
{
    [TestFixture]
    public class ProductAppServiceTest {
        private Mock<IProductRepository> productRepositoryMock;
        private Mock<IAdapter<ProductContract, Product>> adapterMock;
        private ProductAppService service;

        [SetUp]
        public void SetUp() {
            this.productRepositoryMock = new Mock<IProductRepository>(MockBehavior.Strict);
            this.adapterMock = new Mock<IAdapter<ProductContract, Product>>(MockBehavior.Strict);
            this.service = new ProductAppService(this.productRepositoryMock.Object, this.adapterMock.Object);
        }

        [TearDown]
        public void TearDown() {
        }

        [Test]
        public void ShouldFailToAddAProductWhenNoContractIsGiven() {
            Assert.Throws(typeof(ProductIsNullException), () => this.service.Add(null));
        }

        [Test]
        [TestCase(null, 10, typeof(ProductNameIsInvalidException))]
        [TestCase("Product", 0, typeof(ProductValueIsInvalidException))]
        [TestCase("Product", -1, typeof(ProductValueIsInvalidException))]
        public void ShouldValidateProductBeforeAdd(string name, decimal value, Type exceptionType) {
            var contract = new ProductContract {
                Name = name,
                Value = value
            };
            this.adapterMock.Setup(m => m.Convert(contract)).Returns(new Product(name, value)).Verifiable();
            
            Assert.Throws(exceptionType, () => this.service.Add(contract));

            this.adapterMock.Verify(m => m.Convert(contract), Times.Once());
        }

        [Test]
        [TestCase("Product", 10)]
        public void ShouldBeAbleToAddAProduct(string name, decimal value) {
            var contract = new ProductContract {
                Name = name,
                Value = value
            };
            this.adapterMock.Setup(m => m.Convert(contract)).Returns(new Product(name, value)).Verifiable();
            this.productRepositoryMock.Setup(m => m.Add(It.IsAny<Product>())).Returns(new Product(1, name, value)).Verifiable();
            
            var id = this.service.Add(contract);
            Assert.AreEqual(1, id);

            this.adapterMock.Verify(m => m.Convert(contract), Times.Once());
            this.productRepositoryMock.Verify(m => m.Add(It.IsAny<Product>()), Times.Once());
        }

        [Test]
        [TestCase("Product", 10)]
        public void ShouldBeAbleToEditAProduct(string name, decimal value) {
            var contract = new ProductContract {
                Name = name,
                Value = value
            };
            this.productRepositoryMock.Setup(m => m.GetById(1)).Returns(new Product(1, "name", 20)).Verifiable();
            this.productRepositoryMock.Setup(m => m.Edit(It.IsAny<Product>())).Returns(new Product(1, name, value)).Verifiable();
            
            this.service.Edit(1, contract);

            this.productRepositoryMock.Verify(m => m.GetById(1), Times.Once());
            this.productRepositoryMock.Verify(m => m.Edit(It.Is<Product>(p => p.Id.Equals(1) && p.Name.Equals(name) && p.Value.Equals(value))), Times.Once());
        }

        [Test]
        [TestCase(null)]
        public void ShouldFailToGetAProductWithAnInvalidId(object id) {
            Assert.Throws<ProductIdIsInvalidException>(() => this.service.GetById(id));
        }

        [Test]
        [TestCase(1)]
        public void ShouldFailToGetAProductThatNotExists(object id) {
            this.productRepositoryMock.Setup(m => m.GetById(id)).Returns((Product)null).Verifiable();
            
            Assert.Throws<ProductNotFoundException>(() => this.service.GetById(id));
            
            this.productRepositoryMock.Verify(m => m.GetById(id), Times.Once());
        }
        
        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void ShouldBeAbleToGetAProductById(object id) {
            var product = new Product(id, "name", 20);
            var contract = new ProductContract();
            this.productRepositoryMock.Setup(m => m.GetById(id)).Returns(product).Verifiable();
            this.adapterMock.Setup(m => m.Convert(product)).Returns(contract).Verifiable();
            
            var response = this.service.GetById(id);
            Assert.AreSame(contract, response);

            this.productRepositoryMock.Verify(m => m.GetById(id), Times.Once());
            this.adapterMock.Verify(m => m.Convert(product), Times.Once());
        }

        [Test]
        [TestCase(null)]
        public void ShoulFailTryToDeleteWithAnInvalidId(object id) {
            Assert.Throws<ProductIdIsInvalidException>(() => this.service.Delete(id));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void ShouldBeAbleToDeleteAProductById(object id) {
            this.productRepositoryMock.Setup(m => m.Delete(id)).Verifiable();

            Assert.DoesNotThrow(() => this.service.Delete(id));

            this.productRepositoryMock.Verify(m => m.Delete(id), Times.Once());
        }

        [Test]
        public void ShouldBeAbleToGetAllProducts() {
            var products = new List<Product> {
                new Product(1, "Product 1", 10),
                new Product(2, "Product 2", 20)
            };
            var contract = new ProductContract();
            this.productRepositoryMock.Setup(m => m.All()).Returns(products.AsQueryable()).Verifiable();
            this.adapterMock.Setup(m => m.Convert(It.IsAny<Product>())).Returns(contract).Verifiable();
            
            var response = this.service.All();
            Assert.AreEqual(2, response.Count());
            Assert.AreSame(contract, response.First());
            Assert.AreSame(contract, response.Last());

            this.productRepositoryMock.Verify(m => m.All(), Times.Once());
            this.adapterMock.Verify(m => m.Convert(It.IsAny<Product>()), Times.Exactly(2));
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void ShouldFailToGetAnInvalidPage(int page) {
            Assert.Throws<PageInvalidException>(() => this.service.GetPage(page));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(10)]
        public void ShouldBeAbleToGetProductsByPage(int page) {
            var pageResult = new PageResult<Product>(10, page, new List<Product>{ new Product(1, "Product", 10) });
            var contract = new ProductContract();
            this.productRepositoryMock.Setup(m => m.GetPage(page)).Returns(pageResult).Verifiable();
            this.adapterMock.Setup(m => m.Convert(It.IsAny<Product>())).Returns(contract).Verifiable();
            
            var result = this.service.GetPage(page);
            Assert.AreEqual(pageResult.CurrentPage, result.CurrentPage);
            Assert.AreEqual(pageResult.PageCount, result.PageCount);
            Assert.AreEqual(pageResult.NextPage, result.NextPage);
            Assert.AreEqual(pageResult.Items.Count, result.Items.Count());
            Assert.AreEqual(contract, result.Items.First());

            this.productRepositoryMock.Verify(m => m.GetPage(page), Times.Once());
            this.adapterMock.Verify(m => m.Convert(It.IsAny<Product>()), Times.Once());
        }
    }
}