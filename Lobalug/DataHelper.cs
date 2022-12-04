using System;
using System.Collections.Generic;
using System.Linq;
using StepRepository;

namespace Lobalug
{
    public static class DataHelper
    {
        public static DateTime FirstDate(List<StepAtDay> data)
        {
            if (data != null && data.Count > 0)
            {
                return data.Select(d => d.TargetDate).Min();
            }

            return DateTime.Now;
        }

        public static DateTime LastDate(List<StepAtDay> data)
        {
            if (data != null && data.Count > 0)
            {
                return data.Select(d => d.TargetDate).Max();
            }

            return DateTime.Now;
        }



    }
}