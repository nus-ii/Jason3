using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lobalug
{
    public class StepsInDay
    {
        public DateTime Day { get; set; }

        public int Steps { get; set; }

        public override string ToString()
        {
            return this.Day.ToString("dd.MM.yyyy") + ";" + this.Steps.ToString();
        }
    }
}
