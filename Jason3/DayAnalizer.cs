using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jason3
{
    /// <summary>
    /// Класс анализа положения дня в неделе
    /// </summary>
    public static class DayAnalizer
    { 
        /// <summary>
        /// Возвращает дни аналогичные переданному в нужных годах
        /// </summary>
        /// <param name="years">Список годов дни из которых надо найти</param>
        /// <param name="date">Дата аналоги которой нужно искать</param>
        /// <param name="getAlternative">Если нет аналогичного дня в месяце (например 5го воскресенья) возвращать предидущий (4е воскресенье)</param>
        /// <returns>Если передана дата соответсвующая 2ой субботе февраля, то вернутся 2ые субботы февраля выбранных годов</returns>
        public static IEnumerable<DateTime> GetDaysLike(IEnumerable<int> years,DateTime date,bool getAlternative=false)
        {
            DayInYear dayInYear = new DayInYear(date);
            return years.Select(y => GetDayForYear(y, dayInYear, getAlternative)).Where(d=>d.HasValue).Select(i=>i.Value);
        }

        private static DateTime? GetDayForYear(int year, DayInYear dayInYear, bool getAlternative)
        {
            DateTime cart = new DateTime(year, dayInYear.Month, 1);
            DateTime alternative = new DateTime();
            int CountertDayInWeek = 0;
            while (CountertDayInWeek <= dayInYear.CountertDayInWeek&&cart.Month==dayInYear.Month)
            {
                if (cart.DayOfWeek == dayInYear.DayOfWeek)
                {
                    CountertDayInWeek++;
                    alternative = cart;
                }

                if(CountertDayInWeek!= dayInYear.CountertDayInWeek)
                {
                    cart = cart.AddDays(1);                    
                }
                else
                {
                    break;
                }                
            }

            if (CountertDayInWeek != dayInYear.CountertDayInWeek)
            {
                if (getAlternative)
                {
                    return alternative;
                }
                return null;
            }
            return cart;
        }
    }
}
