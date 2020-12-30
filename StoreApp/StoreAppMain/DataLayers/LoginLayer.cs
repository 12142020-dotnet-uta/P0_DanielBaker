using System;

namespace StoreApp
{
    public class LoginLayer
    {

        /// <summary>
        /// Takes in a string to create a new customer username, only is called if customer does not exist.
        /// </summary>
        /// <param name="uName">String inputted username</param>
        /// <returns></returns>
        public Customer CreateCustomer( string uName )
        {
            var info = GetLoginInfo(uName);
            int age = InputFunctions.ParseStringToInt( info[3] );
            Customer newUser = new Customer( uName, info[0], info[1], info[2], age );
            return newUser;
        }
        /// <summary>
        /// Parses the information to create a new user
        /// </summary>
        /// <param name="userName">String inputted username</param>
        /// <returns></returns>
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
        /// <summary>
        /// Checks the users password against the database to verify the password. Returns false if the wrong password, returns true with correct password.
        /// </summary>
        /// <param name="c">Customer object trying to log in</param>
        /// <returns></returns>
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