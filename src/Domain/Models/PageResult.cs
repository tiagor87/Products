using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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

    public class PageCountInvalidException : ArgumentException {
        private const string MESSAGE = "Page count must be greater than zero";
        
        public PageCountInvalidException() : base(MESSAGE)
        {
        }
    }

    public class CurrentPageInvalidException : ArgumentException {
        private const string MESSAGE = "Current page must be less or equal page count";
        
        public CurrentPageInvalidException() : base(MESSAGE)
        {
        }
    }

    public class ItemsInvalidException : ArgumentException {
        private const string MESSAGE = "Items should be defined";
        
        public ItemsInvalidException() : base(MESSAGE)
        {
        }
    }
}