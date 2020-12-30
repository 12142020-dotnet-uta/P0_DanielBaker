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
        /// <returns>Current customer</returns>
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

        /// <summary>
        /// Checks to see if a store exists. If it does, prompt for password and loop until correct password, if not create new store.
        /// </summary>
        /// <param name="storeName">String that user inputs for a store name</param>
        /// <param name="storeAddress">String that user inputs for a store city</param>
        /// <returns>Current store</returns>
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

        /// <summary>
        /// Checks to see if a product exists, if it does not create a new product.
        /// </summary>
        /// <param name="name">String that user inputs for product name</param>
        /// <param name="desc">String that user inputs that gives description of the product</param>
        /// <param name="price">Decimal that user inputs to set the products price</param>
        /// <param name="ageNeeded">Boolean that user inputs to see if the item should be age restricted</param>
        /// <returns>New product object</returns>
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

        /// <summary>
        /// Checks to see if an inventory for a product exists, if not creates new inventory.
        /// </summary>
        /// <param name="chosenProduct">Object product that will be set to inventory</param>
        /// <param name="chosenStore">Object StoreLocation that assigns what store the inventory will be at</param>
        /// <param name="quantity">How much inventory fothe product should there be</param>
        /// <returns>New inventory object</returns>
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

        /// <summary>
        /// Checks to see if the current customer has an open order, if not it creates it
        /// </summary>
        /// <param name="customer">Customer object that the order will be assigned to</param>
        /// <returns>New order object</returns>
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
            }
            return currentOrder;
        }

        /// <summary>
        /// Creating an orderdetail line item for the order
        /// </summary>
        /// <param name="item">Inventory object that is going to be added to the order</param>
        /// <param name="currentOrder">Order object that the order detail will be added to</param>
        /// <param name="quantity">Int how much of the inventory item will be added to the orderdetail line</param>
        /// <returns>New orderdetail object</returns>
        public OrderDetails CreateOrderDetail(Inventory item, Order currentOrder, int quantity)
        {
            OrderDetails itemOrder = new OrderDetails();
            itemOrder = orderDetails.Where(x => x.Item == item && x.Order == currentOrder).FirstOrDefault();
            if(itemOrder == null )
            {
                Inventory addedInventory = inventories.Include(x => x.StoreLocation).Include(x => x.Product).Where(x => x.Product.ProductName == item.Product.ProductName).FirstOrDefault();
                Console.WriteLine($"{addedInventory.Product.ProductName} is not in cart... adding {addedInventory.Product.ProductName}");
                itemOrder = new OrderDetails( item, currentOrder, quantity);
                if(itemOrder.QuantityOrdered > addedInventory.ProductQuantity)
                {
                     throw new Exception($"Cannot order {itemOrder.QuantityOrdered } there are only {item.ProductQuantity}");
                }
                decimal total = addedInventory.Product.ProductPrice * quantity;
                Console.WriteLine($"Added {quantity} {addedInventory.Product.ProductName} to your cart at ${addedInventory.Product.ProductPrice} each. For a total of ${total}");
                itemOrder.Price = itemOrder.TotalPrice(quantity, addedInventory.Product.ProductPrice);
                AddPriceToOrder(currentOrder, itemOrder.Price);
                orderDetails.Add(itemOrder);
            }
            else
            {
                itemOrder.QuantityOrdered += quantity;
                if(itemOrder.QuantityOrdered > item.ProductQuantity)
                {
                    throw new Exception($"Cannot order {itemOrder.QuantityOrdered } there are only {item.ProductQuantity}");
                }
            }
            DbContext.SaveChanges();
            return itemOrder;
        }

        /// <summary>
        /// Checks out the order, changed the order from a cart to ordered. Subtracts inventory.
        /// </summary>
        /// <param name="cartOrder">Cart object that is being checked out</param>
        /// <returns>Checkout out order object</returns>
        public Order CheckoutOrder(Order cartOrder)
        {
            List<OrderDetails> cart = orderDetails.Include(x => x.Order).Include(x => x.Item).ThenInclude(x => x.Product).Include(x => x.Item).ThenInclude(x => x.StoreLocation).Where(x => x.Order == cartOrder).ToList();

            foreach( OrderDetails item in cart)
            {
                if((item.Item.ProductQuantity - item.QuantityOrdered) < 0)
                {
                    throw new Exception($"Can not checkout. You are odering too much {item.Item.Product.ProductName}.");
                }

                SubtractInventoryOnOrder(item.Item, item.QuantityOrdered);
            }
            cartOrder.isOrdered = true;
            cartOrder.isCart = false;
            DbContext.SaveChanges();
            return cartOrder;
        }

        /// <summary>
        /// Subtracts inventory when a cart is checked out
        /// </summary>
        /// <param name="itemBought">Inventory object of the item being bought</param>
        /// <param name="quantityBought">Int the amount of quantity being bought</param>
        public void SubtractInventoryOnOrder(Inventory itemBought, int quantityBought)
        {
            itemBought.ProductQuantity -= quantityBought;
            DbContext.SaveChanges();
        }

        /// <summary>
        /// Adding price of line item to order
        /// </summary>
        /// <param name="cartOrder">Cart object that is being updated</param>
        /// <param name="price">Decimal the price being added to the object</param>
        public void AddPriceToOrder(Order cartOrder, decimal price)
        {
            cartOrder.TotalPrice += price;
            DbContext.SaveChanges();
        }
        

        /// <summary>
        /// Selects Product object based on name compared to inputted string
        /// </summary>
        /// <param name="name">Name of the orbject beign selected</param>
        /// <returns>Selected product object</returns>
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

        /// <summary>
        /// Selects Inventory object based on name compared to inputted string name, and a StoreLocation object
        /// </summary>
        /// <param name="name">Name of the orbject beign selected</param>
        /// <param name="store">Store object where the inventory is located</param>
        /// <returns>Selected inventory object</returns>
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

        /// <summary>
        /// Selecting a customer based on their usename
        /// </summary>
        /// <param name="username">String username inputed</param>
        /// <returns>Customer object with that username</returns>
        public Customer SelectCustomer(string username)
        {
            Customer selectedCustomer = new Customer();
            foreach(Customer c in customers)
            {
                if(c.CustomerUserName == username)
                {
                    selectedCustomer = c;
                }
            }
            return selectedCustomer;
        }

        /// <summary>
        /// Selecting a Store based on an inputed name
        /// </summary>
        /// <param name="name">String name of the store that is being inputted</param>
        /// <returns>StoreLocation object that has the same name</returns>
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

        

        // RETURNING LISTS

        /// <summary>
        /// Creating a list of orders that are assigned to a Customer
        /// </summary>
        /// <param name="c">Customer object that is being checked for orders</param>
        /// <returns>List of orders for the Customer object</returns>
        public List<Order> ShowOrders(Customer c)
        {
            return orders.Where(x => x.Customer == c && x.isOrdered == true).ToList();
        }

        /// <summary>
        /// Creating a list of orders that are assigned to a StoreLocation
        /// </summary>
        /// <param name="s">StoreLocation object that is being checked for orders</param>
        /// <returns>List of orders for the Customer object</returns>
        public List<Order> ShowOrders(StoreLocation s)
        {
            List<Order> orderList = new List<Order>();       
            List<OrderDetails> details = orderDetails.Include(x => x.Order).Include(x => x.Item).ThenInclude(x => x.Product).Include(x => x.Item).ThenInclude(x => x.StoreLocation).Where(x => x.Item.StoreLocation == s && x.Order.isOrdered == true).ToList();     
            foreach(OrderDetails o in details)
            {
                orderList.Add(o.Order);
            }
            return orderList;
        }

        /// <summary>
        /// Creating a list of Inventory at a store location
        /// </summary>
        /// <param name="store">Store object checked for inventory</param>
        /// <returns>List of Inventory at a Store</returns>
        public List<Inventory> DisplayProducts (StoreLocation store)
        {
            List<Inventory> inventory = inventories.Include(x => x.StoreLocation).Include(x => x.Product).Where(x => x.StoreLocation.StoreLocationName == store.StoreLocationName).ToList();
            return inventory;
        }

        /// <summary>
        /// Creating a list of OrderDetails for a given Order object
        /// </summary>
        /// <param name="cartOrder">Order object that is being checked for its OrderDetails items</param>
        /// <returns>List of OrderDetail line items</returns>
        public List<OrderDetails> ShowCart(Order cartOrder)
        {
             return orderDetails.Include(x => x.Order).Include(x => x.Item).ThenInclude(x => x.Product).Include(x => x.Item).ThenInclude(x => x.StoreLocation).Where(x => x.Order == cartOrder).ToList();
        }

        /// <summary>
        /// Creating a list of OrderDetails for a given order based on store location
        /// </summary>
        /// <param name="cartOrder">Order object that is being checked for its OrderDetails items</param>
        /// <param name="store">StoreLocation object that is being compared against OrderDetails</param>
        /// <returns>List of OrderDetails for a store location</returns>
        public List<OrderDetails> ShowCart(Order cartOrder, StoreLocation store)
        {
             return orderDetails.Include(x => x.Order).Include(x => x.Item).ThenInclude(x => x.Product).Include(x => x.Item).ThenInclude(x => x.StoreLocation).Where(x => x.Order == cartOrder && x.Item.StoreLocation == store).ToList();
        }

        /// <summary>
        /// Creating a list of customers
        /// </summary>
        /// <returns>List of Customers</returns>
        public List<Customer> GetCustomers()
        {
            return customers.ToList();
        }

        /// <summary>
        /// Creating a list of store locations
        /// </summary>
        /// <returns>List of StoreLocations</returns>
        public List<StoreLocation> GetStores()
        {
            return stores.ToList();
        }

        /// <summary>
        /// Creating a list of products
        /// </summary>
        /// <returns>List of Products</returns>
        public List<Product> GetProducts()
        {
            return products.ToList();
        }
        
        /// <summary>
        /// Makeing a user an admin
        /// </summary>
        /// <param name="c">Customer object to be made admin</param>
        public void MakeUserAdmin(Customer c)
        {
            c.MakeAdmin();
            DbContext.SaveChanges();
        }


    }
}