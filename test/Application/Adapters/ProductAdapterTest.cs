using NUnit.Framework;
using Products.Application.Adapters;
using Products.Application.Contracts;
using Products.Domain.Models;

namespace Products.Tests.Application.Adapters
{
    [TestFixture]
    public class ProductAdapterTest {
        private ProductAdapter adapter;

        [SetUp]
        public void SetUp() {
            this.adapter = new ProductAdapter();
        }

        [Test]
        [TestCase(1, "Product", 10)]
        public void ShouldBeAbleToConvertAProductContractToModel(int id, string name, decimal value) {
            var contract = new ProductContract { Id = id, Name = name, Value = value };
            var model = this.adapter.Convert(contract);
            Assert.IsNotNull(model);
            Assert.AreEqual(contract.Id, model.Id);
            Assert.AreEqual(contract.Name, model.Name);
            Assert.AreEqual(contract.Value, model.Value);
        }

        [Test]
        [TestCase(1, "Product", 10)]
        public void ShouldBeAbleToConvertAProductModelToContract(int id, string name, decimal value) {
            var contract = new Product(id, name, value);
            var model = this.adapter.Convert(contract);
            Assert.IsNotNull(model);
            Assert.AreEqual(contract.Id, model.Id);
            Assert.AreEqual(contract.Name, model.Name);
            Assert.AreEqual(contract.Value, model.Value);
        }
    }
}