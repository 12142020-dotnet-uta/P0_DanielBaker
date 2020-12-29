using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace StoreApp
{
    
    public class StoreDataLayer
    {
        StoreDbContext DbContext;
        static LoginLayer login = new LoginLayer();

        DbSet<Customer> customers; // = DbContext.customers;
        DbSet<StoreLocation> stores; // = DbContext.locations;
        DbSet<Product> products; // = DbContext.products;
        DbSet<Inventory> inventories; // = DbContext.inventories;
        DbSet<Order> orders;
        DbSet<OrderDetails> orderDetails;

        public StoreDataLayer(): this(new StoreDbContext()){}
        public StoreDataLayer(StoreDbContext context)
        {
            DbContext = context;
            customers = context.customers;
            stores = context.locations;
            products = context.products;
            inventories = context.inventories;
            orders = context.orders;
            orderDetails = context.orderDetails;
        }
        
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
        public Product CreateProduct (string name, string desc, decimal price, bool ageNeeded )
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

        public Order CreateOrder(Customer customer)
        {
            Order currentOrder = new Order();
            currentOrder = orders.Where(x => x.Customer == customer && x.isCart == true).FirstOrDefault();
            if(currentOrder == null)
            {
                Console.WriteLine($"Creating a new cart for {customer.CustomerFName}");
                currentOrder = new Order(customer, true);
                orders.Add(currentOrder);
                DbContext.SaveChanges();
            }
            else
            {
                Console.WriteLine($"Loading cart for {customer.CustomerFName}");
                // show cart here maybe
            }
            return currentOrder;
        }



        public Order CheckoutOrder(Order cartOrder)
        {
            List<OrderDetails> cart = orderDetails.Include(x => x.Order).Include(x => x.Item).ThenInclude(x => x.Product).Include(x => x.Item).ThenInclude(x => x.StoreLocation).Where(x => x.Order == cartOrder).ToList();
            foreach( OrderDetails item in cart)
            {
                SubtractInventoryOnOrder(item.Item, item.QuantityOrdered);
            }
            cartOrder.isOrdered = true;
            cartOrder.isCart = false;
            DbContext.SaveChanges();
            return cartOrder;
        }


        public void SubtractInventoryOnOrder(Inventory itemBought, int quantityBought)
        {
            itemBought.ProductQuantity -= quantityBought;
            DbContext.SaveChanges();
        }

        public void AddPriceToOrder(Order cartOrder, decimal price)
        {
            cartOrder.TotalPrice += price;
            DbContext.SaveChanges();
        }
        

        public OrderDetails CreateOrderDetail(Inventory item, Order currentOrder, int quantity)
        {
            OrderDetails itemOrder = new OrderDetails();
            itemOrder = orderDetails.Where(x => x.Item == item && x.Order == currentOrder).FirstOrDefault();
            if(itemOrder == null )
            {
                Inventory addedInventory = inventories.Include(x => x.StoreLocation).Include(x => x.Product).Where(x => x.Product.ProductName == item.Product.ProductName).FirstOrDefault();
                Console.WriteLine($"{addedInventory.Product.ProductName} is not in cart... adding {addedInventory.Product.ProductName}");
                itemOrder = new OrderDetails( item, currentOrder, quantity);
                Console.WriteLine($"Added {quantity} {addedInventory.Product.ProductName} to your cart at ${addedInventory.Product.ProductPrice} each");
                itemOrder.Price = itemOrder.TotalPrice(quantity, addedInventory.Product.ProductPrice);
                AddPriceToOrder(currentOrder, itemOrder.Price);
                orderDetails.Add(itemOrder);
            }
            else
            {
                itemOrder.QuantityOrdered += quantity;
            }
            DbContext.SaveChanges();
            return itemOrder;
        }

// RETURNING LISTS

        // can this return Order and loop through it later?????
        public List<OrderDetails> ShowCart(Order cartOrder)
        {
             return orderDetails.Include(x => x.Order).Include(x => x.Item).ThenInclude(x => x.Product).Include(x => x.Item).ThenInclude(x => x.StoreLocation).Where(x => x.Order == cartOrder).ToList();
        }





        


        public List<StoreLocation> GetStores()
        {
            return stores.ToList();
        }

        public StoreLocation SelectStore(string name)
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

        public List<Inventory> DisplayProducts (StoreLocation store)
        {
            List<Inventory> inventory = inventories.Include(x => x.StoreLocation).Include(x => x.Product).Where(x => x.StoreLocation.StoreLocationName == store.StoreLocationName).ToList();
            return inventory;
        }

        public Inventory SelectInventory(string name, StoreLocation store)
        {
            Inventory selectedInventory = new Inventory();
            selectedInventory = inventories.Include(x => x.StoreLocation).Include(x => x.Product).Where(x => x.StoreLocation.StoreLocationName == store.StoreLocationName && x.Product.ProductName == name).FirstOrDefault();
            
            if(selectedInventory == null)
            {
                throw new Exception("This item does not exist at this store");
            }
         
            return selectedInventory;
        }
       

    }
}