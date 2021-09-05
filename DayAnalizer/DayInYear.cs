using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayAnalizer
{
    public class DayInYear
    {
        /// <summary>
        /// Положение данного дня недели в месяце
        /// </summary>
        public int CountertDayInWeek;

        /// <summary>
        /// Номер месяца
        /// </summary>
        public int Month;

        /// <summary>
        /// День недели
        /// </summary>
        public readonly DayOfWeek DayOfWeek;

        public DayInYear(DateTime date)
        {
            DayOfWeek = date.DayOfWeek;
            Month = date.Month;
            CountertDayInWeek = 0;

            DateTime cart = new DateTime(date.Year, date.Month, 1);
            while (cart.Day <= date.Day && cart.Month == date.Month)
            {
                if (cart.DayOfWeek == DayOfWeek)
                {
                    CountertDayInWeek++;
                }
                cart = cart.AddDays(1);
            }
        }
    }
}
