using System.Collections.Generic;
using NUnit.Framework;
using Products.Domain.Exceptions;
using Products.Domain.Models;

namespace Products.Domain.Tests.Models
{
    [TestFixture]
    public class PageResultTest {
        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void ShouldFailToCreateWithInvalidPageCount(int pageCount) {
            Assert.Throws<PageCountInvalidException>(() => new PageResult<object>(pageCount, 1, new List<object>() {}));
        }

        [Test]
        [TestCase(1, -1)]
        [TestCase(1, 0)]
        [TestCase(1, 2)]
        public void ShouldFailToCreateWithInvalidCurrentPage(int pageCount, int currentPage) {
            Assert.Throws<CurrentPageInvalidException>(() => new PageResult<object>(pageCount, currentPage, new List<object>() {}));
        }

        [Test]
        public void ShouldFailToCreateWithInvalidItems() {
            Assert.Throws<ItemsInvalidException>(() => new PageResult<object>(1, 1, null));
        }

        [Test]
        [TestCase(5, 1, 2)]
        [TestCase(4, 4, null)]
        public void ShoulBeAbleToGetNextPage(int pageCount, int currentPage, int? nextPage) {
            var pageResult = new PageResult<object>(pageCount, currentPage, new List<object>());
            Assert.AreEqual(nextPage, pageResult.NextPage);
        }

        [Test]
        public void ShouldNotBeAbleToModifyItems() {
            var items = new List<int> { 1, 2, 3 };
            var pageResult = new PageResult<int>(1, 1, items);
            items.Add(4);
            Assert.AreEqual(3, pageResult.Items.Count);
        }
    }
}