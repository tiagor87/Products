using System;
using System.Runtime.Serialization;

namespace Products.Domain.Models
{
    public class Product : IDisposable {
        private bool disposed;
        
        public Product(object id, string name, decimal value) {
            this.Id = id;
            this.Name = name;
            this.Value = value;
        }

        public Product(string name, decimal value) : this(null, name, value)
        {
        }
        
        public object Id { get; private set; }
        public string Name { get; set; }
        public decimal Value { get; set; }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void BeforeDispose() {}

        protected void Dispose(bool disposing) {
            if (this.disposed) return;
            if (disposing) {
                this.BeforeDispose();
            }
            this.disposed = true;
        }

        public void Validate() {
            if (string.IsNullOrWhiteSpace(this.Name)) {
                throw new ProductNameIsInvalidException();
            }
            if (this.Value <= 0) {
                throw new ProductValueIsInvalidException();
            }
        }
    }

    [Serializable]
    public class ProductValueIsInvalidException : Exception
    {
        private const string MESSAGE = "The product's value must be greater than zero.";

        public ProductValueIsInvalidException() : base(MESSAGE)
        {
        }
    }

    [Serializable]
    public class ProductNameIsInvalidException : Exception
    {
        private const string MESSAGE = "The product's name must be defined.";
        
        public ProductNameIsInvalidException() : base(MESSAGE)
        {
        }
    }
}