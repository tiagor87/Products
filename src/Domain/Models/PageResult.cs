using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Products.Domain.Exceptions;

namespace Products.Domain.Models
{
    public sealed class PageResult<T> {
        private List<T> items;

        public PageResult(long pageCount, long currentPage, IEnumerable<T> items) {
            if (pageCount <= 0) {
                throw new PageCountInvalidException();
            }
            this.PageCount = pageCount;
            if (currentPage <= 0 || currentPage > pageCount) {
                throw new CurrentPageInvalidException();
            }
            this.CurrentPage = currentPage;
            if (items == null) {
                throw new ItemsInvalidException();
            }
            this.items = items.ToList();
        }

        public long PageCount { get; private set; }
        public long CurrentPage { get; private set; }
        public long? NextPage {
            get
            {
                if (this.CurrentPage < this.PageCount) {
                    return this.CurrentPage + 1;
                }
                return null;
            }
        }
        public IReadOnlyCollection<T> Items {
            get {
                return this.items.AsReadOnly();
            }
        }
    }
}