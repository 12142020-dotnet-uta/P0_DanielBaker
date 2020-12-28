using System;
using System.Collections.Generic;

namespace StoreApp
{
    class Program
    {
        static StoreDataLayer storeState = new StoreDataLayer();
        // static InputFunctions input = new InputFunctions();
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
                    if(shopState == 2)
                    {
                        Console.WriteLine("Logging out...");
                        break;
                    } else if(shopState == 3)
                    {
                        Console.WriteLine("To create new store please enter a name for it:");
                        string storeName = Console.ReadLine();
                        Console.WriteLine("To create new store please enter a city for it:");
                        string storeAddress = Console.ReadLine();
                        StoreLocation currentStore = storeState.CreateStore(storeName, storeAddress);
                    } else if(shopState == 4)
                    {
                        Console.WriteLine("To create new product please enter a name for it:");
                        string productName = Console.ReadLine();
                        Console.WriteLine("To create new product please enter a desc for it:");
                        string productDesc = Console.ReadLine();

                        storeState.CreateProduct(productName, productDesc, 22.0, false);

                        // List<Product> products = storeState.GetProducts( currentStore );
                       
                        // foreach( Product p in products )
                        // {
                        //     // Console.WriteLine($"{p.ProductName} {p.ProductDesc} {p.ProductQuantity} {p.IsAgeRestricted}");
                        // }

                    } else if (shopState == 5)
                    {
                        Console.WriteLine("Select store by name to add a product to it.");
                        List<StoreLocation> stores = storeState.GetStores();
                        foreach(StoreLocation s in stores)
                        {
                            Console.WriteLine(s.StoreLocationName);
                        }
                        string selectedStore = Console.ReadLine();
                        StoreLocation currentStore = storeState.SelectStore(selectedStore);
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
                    // Console.WriteLine("To create new store please enter a name for it:");
                    // string storeName = Console.ReadLine();
                    // Console.WriteLine("To create new store please enter a city for it:");
                    // string storeAddress = Console.ReadLine();
                    // StoreLocation currentStore = storeState.CreateStore(storeName, storeAddress);


                    // Console.WriteLine("To create new product please enter a name for it:");
                    // string productName = Console.ReadLine();
                    // Console.WriteLine("To create new product please enter a desc for it:");
                    // string productDesc = Console.ReadLine();

                    // storeState.CreateProduct(productName, productDesc, 22.0, currentStore, false);

                    // List<Product> products = storeState.GetProducts( currentStore );
                    // // Console.WriteLine(products);

                    // foreach( Product p in products )
                    // {
                    //     Console.WriteLine($"{p.ProductName} {p.ProductDesc} {p.IsAgeRestricted} {p.ProductID}  {p.StoreLocation} {p.StoreLocation.StoreLocationId}");
                    // }



                } while (shopState != 2);
            } while(onlineState == 1);
        }
    }
}
