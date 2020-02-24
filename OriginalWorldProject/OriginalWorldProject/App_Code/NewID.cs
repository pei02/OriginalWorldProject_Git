using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OriginalWorldProject.App_Code
{
    public class NewID
    {
        string New_ID, Cn = "";
        int sum, Cn_num = 1;
        public string NewID_fuction(string Old_ID, string First_en, int First_num, int ID_length)
        {
            do
            {
                int i = int.Parse(Old_ID.Substring(First_num, 1));
                First_num++;
                sum = sum * 10;
                sum += i;
                Cn += "0";
            } while (First_num <= ID_length);

            sum++;
            string New_id_str = Convert.ToString(sum);
            int j = ID_length;

            for (int a = 0; a < ID_length; a++)
            {
                j--;
                Cn = Cn.Substring(0, j);

                if (sum >= 1 * Cn_num && sum < Cn_num * 10)
                {
                    New_ID = First_en + Cn + New_id_str;
                }
                Cn_num = Cn_num * 10;

            }
            return New_ID;
        }
    }
}