using System;

namespace StoreApp
{
    public class OrderDetails
    {
        public Guid OrderDetailsId { get; set; } = Guid.NewGuid();
        public Inventory Item { get; set; }
        public int QuantityOrdered { get; set; }
        public Order Order { get; set; }

        public decimal Price { get; set; } = 0;

        public decimal TotalPrice ( int quantityOrdered, decimal price )
        {
            return quantityOrdered * price;
        }

        public OrderDetails(){}

        public OrderDetails(Inventory item, Order cart, int quantity)
        {
            this.Item = item;
            this.Order = cart;
            this.QuantityOrdered = quantity;
        }
    }
}