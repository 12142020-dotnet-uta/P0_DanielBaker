using System;

namespace StoreApp
{
    public class LoginLayer
    {
        static InputFunctions inputEdit = new InputFunctions();
        // public Customer Login()
        // {

        // }
        public Customer CreateCustomer( string uName )
        {
            var info = GetLoginInfo(uName);
            int age = inputEdit.ParseStringToInt( info[3] );
            Customer newUser = new Customer( uName, info[0], info[1], info[2], age );
            return newUser;
        }

        public string[] GetLoginInfo(string userName)
        {
            string[] loginInfo = new string[4];

            Console.WriteLine($"Please enter a password for {userName}:");
            loginInfo[0] = Console.ReadLine();

            Console.WriteLine("Please enter your first name:");
            loginInfo[1] = Console.ReadLine();

            Console.WriteLine("Please enter your last name:");
            loginInfo[2] = Console.ReadLine();

            Console.WriteLine("How old are you?");
            loginInfo[3] = Console.ReadLine();

            return loginInfo;
        }
        public bool PasswordCheck(Customer c)
        {
            string pw = Console.ReadLine();
            if(pw != c.CustomerPassword)
            {
                return false;
            }
            return true;
        }
    }
}