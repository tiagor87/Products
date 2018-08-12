using System;
using Moq;
using NUnit.Framework;
using Products.Domain.Exceptions;
using Products.Domain.Models;

namespace Products.Tests.Domain.Models
{
    [TestFixture]
    public class ProductTest
    {
        [Test]
        [TestCase(null, 10, typeof(ProductNameInvalidException))]
        [TestCase("Product", 0, typeof(ProductValueInvalidException))]
        [TestCase("Product", -1, typeof(ProductValueInvalidException))]
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