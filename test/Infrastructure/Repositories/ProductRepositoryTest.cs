using System.Linq;
using MongoDB.Bson;
using NUnit.Framework;
using Products.Domain.Models;
using Products.Domain.Repositories;
using Products.Infrastructure.Repositories;

namespace Products.Infrastructure.Tests.Repositories
{
    public class ProductRepositoryTest {
        private IProductRepository productRepository;

        [SetUp]
        public void SetUp() {
            this.productRepository = new ProductRepository();
        }

        [Test]
        public void ShouldBeAbleToAddAProduct() {
            var product = new Product("Product", 100);
            var inserted = this.productRepository.Add(product);
            Assert.IsNotNull(inserted.Id);
        }

        [Test]
        public void ShouldBeAbleToEditAProduct() {
            var product = new Product("5b6b72c4346f4f0918a31eed", "Product Updared", 200);
            var edited = this.productRepository.Edit(product);
            Assert.AreEqual(ObjectId.Parse("5b6b72c4346f4f0918a31eed"), edited.Id);
            Assert.AreEqual(product.Name, edited.Name);
            Assert.AreEqual(product.Value, edited.Value);
        }

        [Test]
        [TestCase("5b6b72c4346f4f0918a31eed")]
        public void ShouldBeAbleToDeleteAProduct(object id) {
            this.productRepository.Delete(id);
        }

        [Test]
        public void ShouldBeAbleToGetAllProducts() {
            var products = this.productRepository.All();
            Assert.IsTrue(products.Any());
        }

        [Test]
        [TestCase("5b6b7b6440c1762e547e62b3")]
        public void ShouldBeAbleToGetAProductById(object id) {
            var product = this.productRepository.GetById(id);
            Assert.NotNull(product);
        }

        [Test]
        [TestCase(1, 2, 3)]
        [TestCase(1, 4, 2)]
        [TestCase(1, 1, 6)]
        [TestCase(1, 10, 1)]
        public void ShouldBeAbleToGetProductsAsPages(int page, int maxItemsPerPage, int pageCount) {
            var pageResult = this.productRepository.GetPage(page, maxItemsPerPage);
            Assert.IsNotNull(pageResult);
            Assert.AreEqual(page, pageResult.CurrentPage);
            Assert.LessOrEqual(pageResult.Items.Count, maxItemsPerPage);
            Assert.AreEqual(pageCount, pageResult.PageCount);
        }
    }
}