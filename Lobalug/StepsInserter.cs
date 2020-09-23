using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StepRepository;

namespace Lobalug
{
     public static class StepsInserter
    {
        public static void StepInsertMain(CommonProperty commonProperty)
        {
            string fileId = Guid.NewGuid().ToString().Split('-')[0];

            DateTime lastDate = InputLastDate();
            int steps = 0;
            List<StepAtDay> data = new List<StepAtDay>();

            for (; ; )
            {
                Console.Write("Input steps at " + lastDate.ToString("dd.MM.yyyy") + " ");

                if (ReadSteps(ref steps))
                {
                    data.Add(new StepAtDay(lastDate,steps));
                    lastDate = lastDate.AddDays(-1);
                }
                else
                {          
                    commonProperty._repository.Insert(data);
                }
            }
        }

        private static bool ReadSteps(ref int step)
        {

            for (; ; )
            {
                string val = Console.ReadLine();

                if (val.Trim().ToLower() == "write")
                {
                    step = 0;
                    return false;
                }

                if (Int32.TryParse(val, out step))
                {
                    return true;
                }
                Console.WriteLine("Input again!");
            }
        }

        private static DateTime InputLastDate()
        {
            Console.WriteLine("Input last date in format dd.MM.yyyy");
            string date = Console.ReadLine();

            DateTime result = DateTime.Parse(date);

            return result;
        }
    }
}
