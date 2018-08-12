using System;
using System.Runtime.Serialization;
using Products.Domain.Exceptions;

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
                throw new ProductNameInvalidException();
            }
            if (this.Value <= 0) {
                throw new ProductValueInvalidException();
            }
        }
    }
}