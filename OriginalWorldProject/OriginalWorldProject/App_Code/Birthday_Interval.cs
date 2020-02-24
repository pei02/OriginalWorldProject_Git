using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OriginalWorldProject.App_Code
{
    public class Birthday_Interval
    {
        public int Leapyear_sum(DateTime star, DateTime end)
        {

            var star_year = star.Year;
            var end_year = end.Year;
            int difference = end_year - star_year;
            int leapyear_sum = 0;
            for (int i = 0; i <= difference; i++)
            {
                if (DateTime.IsLeapYear(star_year + i))
                {
                    leapyear_sum++;
                }
            }

            return leapyear_sum;
        }

        public DateTime Birthday_interval(int age)
        {

            int age_sum = age * 365;
            DateTime birthday = DateTime.Today.AddDays(-age_sum);
            int leapyear_sum = Leapyear_sum(birthday, DateTime.Today);
            leapyear_sum--;
            birthday = birthday.AddDays(-leapyear_sum);

            return birthday;
        }

    }
}