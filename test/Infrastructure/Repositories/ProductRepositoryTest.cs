using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using NUnit.Framework;
using Products.Domain.Models;
using Products.Domain.Repositories;
using Products.Infrastructure.Exceptions;
using Products.Infrastructure.Providers;
using Products.Infrastructure.Repositories;

namespace Products.Tests.Infrastructure.Repositories
{
    public class ProductRepositoryTest {
        private IProductRepository productRepository;
        private Mock<IProvider<Product>> providerMock;

        [SetUp]
        public void SetUp() {
            this.providerMock = new Mock<IProvider<Product>>(MockBehavior.Strict);
            this.productRepository = new ProductRepository(this.providerMock.Object);
        }

        [Test]
        public void ShouldBeAbleToAddAProduct() {
            var stub = new Product("Product", 100);
            var insertedStub = new Product(1, "Product", 100);
            this.providerMock.Setup(m => m.InsertOne(stub)).Returns(insertedStub).Verifiable();

            var inserted = this.productRepository.Add(stub);

            Assert.AreSame(insertedStub, inserted);
            this.providerMock.Verify(m => m.InsertOne(stub), Times.Once());
        }

        [Test]
        public void ShouldBeAbleToEditAProduct() {
            var stub = new Product("5b6b72c4346f4f0918a31eed", "Product Updated", 200);
            this.providerMock.Setup(m => m.ReplaceOne("5b6b72c4346f4f0918a31eed", stub)).Returns(1).Verifiable();
            
            this.productRepository.Edit(stub);
            
            this.providerMock.Verify(m => m.ReplaceOne("5b6b72c4346f4f0918a31eed", stub), Times.Once());
        }

        [Test]
        public void ShouldFailWhenProductIsNotUpdated() {
            var stub = new Product(1, "Product", 1);
            this.providerMock.Setup(m => m.ReplaceOne(It.IsAny<object>(), It.IsAny<Product>())).Returns(0).Verifiable();
            
            Assert.Throws<ProductNotUpdatedException>(() => this.productRepository.Edit(stub));
            
            this.providerMock.Verify(m => m.ReplaceOne(It.IsAny<object>(), It.IsAny<Product>()), Times.Once());
        }

        [Test]
        [TestCase("5b6b72c4346f4f0918a31eed")]
        public void ShouldBeAbleToDeleteAProduct(object id) {
            this.providerMock.Setup(m => m.DeleteOne(id)).Verifiable();
            
            this.productRepository.Delete(id);
            
            this.providerMock.Verify(m => m.DeleteOne(id), Times.Once());
        }

        [Test]
        public void ShouldBeAbleToGetAllProducts() {
            var stub = new List<Product>().AsQueryable();
            this.providerMock.Setup(m => m.FindAll()).Returns(stub).Verifiable();
            
            var response = this.productRepository.All();
            
            Assert.AreSame(stub, response);
            this.providerMock.Verify(m => m.FindAll(), Times.Once());
        }

        [Test]
        [TestCase("5b6b7b6440c1762e547e62b3")]
        public void ShouldBeAbleToGetAProductById(object id) {
            var stub = new Product(id, "Product", 1);
            this.providerMock.Setup(m => m.FindById(id)).Returns(stub).Verifiable();

            var product = this.productRepository.GetById(id);

            Assert.AreSame(stub, product);
        }

        [Test]
        public void ShouldBeAbleToGetProductsAsPages() {
            var stub = new PageResult<Product>(5, 1, new List<Product>() {});
            this.providerMock.Setup(m => m.GetPage(1, 5)).Returns(stub).Verifiable();
            
            var pageResult = this.productRepository.GetPage(1, 5);
            
            Assert.AreSame(stub, pageResult);
            this.providerMock.Verify(m => m.GetPage(1, 5), Times.Once());            
        }
    }
}