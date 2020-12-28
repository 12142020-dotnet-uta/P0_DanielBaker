using System;

namespace StoreApp
{
    public class Inventory
    {
        public Guid InventoryId { get; set; } = Guid.NewGuid();
        public Product Product { get; set; }
        public StoreLocation StoreLocation { get; set; }
        public int ProductQuantity { get; set; } = 0;
        
        

        public Inventory(){}

        public Inventory( Product product, StoreLocation store, int quantity )
        {
            this.Product = product;
            this.StoreLocation = store;
            this.ProductQuantity = quantity;
        }
    }
}