using System;

namespace StoreApp
{
    public class InputFunctions
    {
        public static int ParseStringToInt(string numberString)
        {
            int number = -1;
            bool isNumber = Int32.TryParse( numberString, out number );
            if(isNumber == false)
            {
                return -1;
            }
            else
            {
                return number;
            }
        }

        public static decimal ParseStringToDecimal(string numberString)
        {
            decimal number = -1;
            bool isNumber = Decimal.TryParse( numberString, out number );
            if(isNumber == false)
            {
                return -1;
            }
            else
            {
                return number;
            }
        }

        public static bool ParseStringToBool(string s)
        {
            bool boolString = false;
            bool isBool = Boolean.TryParse( s, out boolString );
            if(isBool == false)
            {
                throw new Exception("You did not enter true or false");
            }
            else
            {
                return boolString;
            }
        }


    }
}