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

        public static double ParseStringToDouble(string numberString)
        {
            double number = -1;
            bool isNumber = Double.TryParse( numberString, out number );
            if(isNumber == false)
            {
                return -1;
            }
            else
            {
                return number;
            }
        }
    }
}