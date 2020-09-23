using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepRepository
{
    public class StepAtDay
    {
        public DateTime TargetDate { get; set; }

        private readonly string lazyValue;

        private int? realValue;

        public int Value
        {
            get
            {
                if (!realValue.HasValue)
                {
                    realValue = Int32.Parse(lazyValue);
                }
                return realValue.Value;
            }
            set
            {
                realValue = value;
            }
        }

        public StepAtDay(DateTime day,string steps)
        {
            TargetDate = day;
            lazyValue = steps;
        }

        public StepAtDay(DateTime day, int steps)
        {
            TargetDate = day;
            realValue = steps;
        }
    }
}
