using System;
using System.Collections.Generic;

namespace StoreApp
{
    class Program
    {
        static StoreDataLayer storeState = new StoreDataLayer();

        static void Main(string[] args)
        {

            int onlineState = 1;
            do
            {
                Console.WriteLine("Welcome to Targmart.\n\t1. Login\n\t2. Quit");
                onlineState = InputFunctions.ParseStringToInt(Console.ReadLine());
                if(onlineState != 1) 
                {
                    Console.Write("See you again soon!!!");
                    break;
                }

                Console.WriteLine("Enter a username to login or create a new account");
                Customer activeCustomer = storeState.LoginCustomer(Console.ReadLine());
                int shopState = 1;
                do
                {
                    Console.WriteLine($"Welcome {activeCustomer.CustomerFName}!\n\t1. Shop\n\t2. Logout\n\t3. Create Store\n\t4. Create Products\n\t5. Assign Inventory");
                    shopState = InputFunctions.ParseStringToInt(Console.ReadLine());
                    if(shopState == 1)
                    {
                        int cartState = 1;
                        do
                        {
                            Console.WriteLine("Welcome to TargMart! Type 1 to begin shopping, type 2 to view your cart, or press 3 to checkout.");
                            Console.WriteLine("\t1. Shop\n\t2.View Cart\n\t3. Checkout");
                            cartState = InputFunctions.ParseStringToInt(Console.ReadLine());
                            
                            // creating new cart if there is none, if it exists already, grabbing it.
                            Order currentOrder = storeState.CreateOrder(activeCustomer);
                            if(cartState == 1)
                            {
                                Console.WriteLine("Welcome to TargMart! Please select your store by name to being shopping:");   
                                StoreLocation currentStore = GetStore();
                                Console.WriteLine($"Logged into {currentStore.StoreLocationName} ID: {currentStore.StoreLocationId}");

                                

                                Console.WriteLine("Type the name of the product to select a quantity to buy, or press 1 to go back to store selection");
                                List<Inventory> inventory = storeState.DisplayProducts(currentStore);


                                foreach(Inventory i in inventory)
                                {
                                    Console.WriteLine($"{i.Product.ProductName} {i.ProductQuantity}");
                                }

                                string selectedStore = Console.ReadLine();

                                Inventory item = storeState.SelectInventory(selectedStore, currentStore);

                                Console.WriteLine($"How much {item.Product.ProductName} do you want to buy?");

                                int quantity = InputFunctions.ParseStringToInt(Console.ReadLine());

                                storeState.CreateOrderDetail(item, currentOrder, quantity );
                            }
                            
                            if(cartState == 2)
                            {
                                List<OrderDetails> cart = storeState.ShowCart(currentOrder);

                                foreach( OrderDetails o in cart )
                                {
                                    Console.WriteLine($"Product Name: {o.Item.Product.ProductName} Quantity: {o.QuantityOrdered} From: {o.Item.StoreLocation.StoreLocationName} Price: {o.Price}");
                                }
                            }
                            if(cartState == 3)
                            {
                               storeState.CheckoutOrder(currentOrder);

                            }

                        } while(cartState == 1);
                        


                    }
                    if(shopState == 2)
                    {
                        Console.WriteLine("Logging out...");
                        break;
                    } else if(shopState == 3)
                    {
                        StoreLocation currentStore = StoreCreation();
                        Console.WriteLine($"{currentStore.StoreLocationName} was created with and ID: {currentStore.StoreLocationId}");
                    } else if(shopState == 4)
                    {
                        Product newProduct = CreateProduct();
                        Console.WriteLine($"{newProduct.ProductName} created with a price of ${newProduct.ProductPrice}");

                    } else if (shopState == 5)
                    {
                        Console.WriteLine("Select store by name to add a product to it.");

                        StoreLocation currentStore = GetStore();
                        Console.WriteLine($"Logged into {currentStore.StoreLocationName} ID: {currentStore.StoreLocationId}");

                        Console.WriteLine($"Select a product by name to add it to {currentStore.StoreLocationName}:");
                        List<Product> products = storeState.GetProducts();
                        
                        foreach(Product p in products)
                        {
                            Console.WriteLine($"{p.ProductName}");
                        }

                        string selectedProduct = Console.ReadLine();
                        Product currentProduct = storeState.SelectProduct(selectedProduct);

                        Console.WriteLine($"Selected product: {currentProduct.ProductName}\n Please enter the quantity of {currentProduct.ProductName} to add to {currentStore.StoreLocationName}");
                        int quantityAdd = InputFunctions.ParseStringToInt(Console.ReadLine());

                        storeState.AssignInventory(currentProduct, currentStore, quantityAdd);
                    }

                } while (shopState != 2);
            } while(onlineState == 1);
        } // ends main method

        public static StoreLocation GetStore()
        {
            List<StoreLocation> stores = storeState.GetStores();
            foreach(StoreLocation s in stores)
            {
                Console.WriteLine(s.StoreLocationName);
            }
            string selectedStore = Console.ReadLine();
            return storeState.SelectStore(selectedStore);
        } 

        public static StoreLocation StoreCreation()
        {
            Console.WriteLine("To create new store please enter a name for it:");
            string storeName = Console.ReadLine();
            Console.WriteLine("To create new store please enter a city for it:");
            string storeAddress = Console.ReadLine();
            return storeState.CreateStore(storeName, storeAddress);
            
        }

        public static Product CreateProduct()
        {
            Console.WriteLine("To create new product please enter a name for it:");
            string productName = Console.ReadLine();
            Console.WriteLine("To create new product please enter a desc for it:");
            string productDesc = Console.ReadLine();
            Console.WriteLine("To create new product please enter a price for it:");
            decimal productPrice = InputFunctions.ParseStringToDecimal(Console.ReadLine());
            Console.WriteLine("Is this product age restricted?");
            bool isProductAgeRestricted = InputFunctions.ParseStringToBool(Console.ReadLine());
            return storeState.CreateProduct(productName, productDesc, productPrice, isProductAgeRestricted);
        }

        
    } // ends class
} // ends namescpace
