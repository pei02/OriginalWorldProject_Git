using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OriginalWorldProject.App_Code
{
    public class RandomPassword
    {
        public string RandomPwd()
        {
            string[] en = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "m", "n", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" ,
                            "A","B","C","D","E","F","G","H","I","J","K","L","M","N","P","Q","R","S","T","U","V","W","X","Y","Z"};
            string[] num = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            string[] special_symbol = { "~", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "-", "+", "-", "/", "?" };

            Random pwd = new Random();
            int j = 4, k;
            string get_pwd = "";
            do
            {
                int a = pwd.Next(1, j);
                switch (a)
                {
                    case 1:
                        k = pwd.Next(0, en.Length);
                        get_pwd += en[k];
                        break;
                    case 2:
                        k = pwd.Next(0, num.Length);
                        get_pwd += num[k];
                        break;
                    case 3:
                        k = pwd.Next(0, special_symbol.Length);
                        get_pwd += special_symbol[k];
                        j--;
                        break;
                }
            } while (get_pwd.Length < 10);

            return get_pwd;
        }
    }
}