using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StoreApp
{
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; } = Guid.NewGuid();
        public List<OrderDetails> ProductsInOrder = new List<OrderDetails>();
        public Customer Customer { get; set; }
        public StoreLocation Store { get; set; }
        public decimal TotalPrice { get; set; } = 0;
        public bool isOrdered { get; set; } = false;
        public bool isCart { get; set; } = false;
        public void AddToPrice( decimal price )
        {
            TotalPrice += price;
        }

        public Order(){}

        public Order( Customer newCustomer, bool cart)
        {
            this.Customer = newCustomer;
            this.isCart = cart;
        }
        public Order( Customer newCustomer, StoreLocation store, bool cart)
        {
            this.Customer = newCustomer;
            this.Store = store;
            this.isCart = cart;
        }
    }
}