using System;
using System.ComponentModel.DataAnnotations;

namespace StoreApp
{
    public class Product
    {
        [Key]
        public Guid ProductID { get; set ; } = Guid.NewGuid();
        public string ProductName { get; set; }
        public string ProductDesc { get; set; }
        public decimal ProductPrice { get; set; }
        public bool IsAgeRestricted { get; set; }

        public Product(){}

        public Product( string name, string desc, decimal price, bool ageRestricted )
        {
            this.ProductName = name;
            this.ProductDesc = desc;
            this.ProductPrice = price;
            this.IsAgeRestricted = ageRestricted;
        }
    }
}