using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StoreApp
{
    public class Customer
    {
        [Key]
        public Guid CustomerID { get; set; } = Guid.NewGuid();
        public string CustomerUserName { get; set; }
        public string CustomerPassword { get; set; }

        public string CustomerFName { get; set; }

        public string CustomerLName { get; set; }

        public List<Product> CustomerCart = new List<Product>();

        public List<Order> PastOrder = new List<Order>();

        // age verification for purchases?
        public int CustomerAge { get; set; }

        public StoreLocation PerferedStore { get; set; }

        public Customer(){}

        public Customer(string userName, string userPassword, string fName, string lName, int age)
        {
            this.CustomerUserName = userName;
            this.CustomerPassword = userPassword;
            this.CustomerFName = fName;
            this.CustomerLName = lName;
            this.CustomerAge = age;
        }
    }
}