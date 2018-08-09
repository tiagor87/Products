using System;
using Moq;
using NUnit.Framework;
using Products.Domain.Models;

namespace Products.Domain.Tests.Models
{
    [TestFixture]
    public class ProductTest
    {
        [Test]
        [TestCase(null, 10, typeof(ProductNameIsInvalidException))]
        [TestCase("Product", 0, typeof(ProductValueIsInvalidException))]
        [TestCase("Product", -1, typeof(ProductValueIsInvalidException))]
        public void ShouldThrowExceptionWhenProductIsInvalid(string name, decimal value, Type exceptionType)
        {
            var product = new Product(name, value);
            Assert.Throws(exceptionType, () => product.Validate());
        }

        [Test]
        [TestCase("Product 1", 10)]
        public void ShouldBeAbleToValidadeAProduct(string name, decimal value) {
            var product = new Product(name, value);
            Assert.DoesNotThrow(() => product.Validate());
        } 
    }
}