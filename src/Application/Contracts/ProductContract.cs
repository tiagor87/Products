using System;

namespace Products.Application.Contracts
{
    public class ProductContract {
        public object Id { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
    }
}