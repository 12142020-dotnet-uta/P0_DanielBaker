using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StoreApp
{
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; } = Guid.NewGuid();
        public List<Product> ProductsInOrder = new List<Product>();
        public Customer Customer { get; set; }
    }
}