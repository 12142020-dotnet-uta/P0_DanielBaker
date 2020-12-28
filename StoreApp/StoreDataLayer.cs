using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace StoreApp
{
    
    public class StoreDataLayer
    {
        static StoreDBContext DbContext = new StoreDBContext();
        static InputFunctions inputEdit = new InputFunctions();
        static LoginLayer login = new LoginLayer();

        DbSet<Customer> customers = DbContext.customers;
        DbSet<StoreLocation> stores = DbContext.locations;
        DbSet<Product> products = DbContext.products;
        DbSet<Inventory> inventories = DbContext.inventories;
        
        /// <summary>
        /// Checks to see if user name exists. If it does, prompt for password and loop until correct password, if not create new user.
        /// </summary>
        /// <param name="userName">User input for username.</param>
        /// <returns></returns>
        public Customer LoginCustomer(string userName)
        {
            Customer activeCustomer = new Customer();
            activeCustomer = customers.Where(x => x.CustomerUserName == userName).FirstOrDefault();
            if(activeCustomer == null)
            {
                Console.WriteLine("Username not found...\nCreating new customer...");
                activeCustomer = login.CreateCustomer(userName);
                customers.Add(activeCustomer);
                DbContext.SaveChanges();
            }
            else
            {
                bool correctPassword;
                do
                {
                    Console.WriteLine($"Please enter the correct password for {activeCustomer.CustomerUserName}");
                    correctPassword = login.PasswordCheck(activeCustomer);
                } while( correctPassword == false);
            }
            return activeCustomer;
        }

        public StoreLocation CreateStore (string storeName, string storeAddress)
        {
            StoreLocation newStore = new StoreLocation();
            newStore = stores.Where(x => x.StoreLocationName == storeName && x.StoreLocationAddress == storeAddress ).FirstOrDefault();
            if(newStore == null)
            {
                Console.WriteLine("Store not found...\nCreating new store...");
                newStore = new StoreLocation(storeName, storeAddress);
                stores.Add(newStore);
                DbContext.SaveChanges();
            }
            else
            {
                Console.WriteLine($"Store ID: {newStore.StoreLocationId} Name: {newStore.StoreLocationName} Location: {newStore.StoreLocationName} already exists");
            }
            return newStore;
        }









        // TODO: add update product.
        public Product CreateProduct (string name, string desc, double price, bool ageNeeded )
        {
            Product newProduct = new Product();
            newProduct = products.Where(x => x.ProductName == name).FirstOrDefault();
            if(newProduct == null)
            {
                newProduct = new Product(name, desc, price, ageNeeded);
                products.Add(newProduct);
                DbContext.SaveChanges();
            } 
            return newProduct;
        }

        public Inventory AssignInventory( Product chosenProduct, StoreLocation chosenStore, int quantity)
        {
            Inventory newInventory = new Inventory();
            newInventory = inventories.Where(x => x.Product.ProductID == chosenProduct.ProductID && x.StoreLocation.StoreLocationId == chosenStore.StoreLocationId).FirstOrDefault();
            if(newInventory == null)
            {
                newInventory = new Inventory(chosenProduct, chosenStore, quantity);
                inventories.Add(newInventory);
            } else
            {
                newInventory.ProductQuantity += quantity;
            }
            DbContext.SaveChanges();
            return newInventory;
        }

        public List<StoreLocation> GetStores()
        {
            return stores.ToList();
        }

        public StoreLocation SelectStore( string name )
        {
            StoreLocation activeStore = new StoreLocation();
            foreach(StoreLocation s in stores)
            {
                if(s.StoreLocationName == name)
                {
                    activeStore = s;
                }
            }
            return activeStore;
        }

        public List<Product> GetProducts()
        {
            return products.ToList();
        }

        public Product SelectProduct (string name)
        {
            Product selectedProduct = new Product();
            selectedProduct = products.Where(x => x.ProductName == name).FirstOrDefault();
            if(selectedProduct == null)
            {
                throw new Exception("Please enter a valid product");
            }
            return selectedProduct;
        }

        // public List<Product> GetProducts()
        // {  
        //     List<Product> listProducts = new List<Product>();
        //     foreach(Product p in products)
        //     {
        //         if(store.StoreLocationId == p.StoreLocation.StoreLocationId)
        //         {
        //             listProducts.Add(p);
        //         }
        //     }
        //     return listProducts;



        //     // return stores.Include(store => store.Products).ToList();

        // }

       

    }
}