using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lobalug
{
     public static class StepsInserter
    {
        public static void StepInsertMain(CommonProperty settings)
        {
            string fileId = Guid.NewGuid().ToString().Split('-')[0];

            DateTime lastDate = InputLastDate();
            int steps = 0;
            List<StepsInDay> data = new List<StepsInDay>();

            for (; ; )
            {
                Console.Write("Input steps at " + lastDate.ToString("dd.MM.yyyy") + " ");

                if (ReadSteps(ref steps))
                {
                    data.Add(new StepsInDay
                    {
                        Day = lastDate,
                        Steps = steps
                    });
                    lastDate = lastDate.AddDays(-1);
                }
                else
                {
                    List<string> resultData = new List<string>();

                    foreach (var ds in data)
                    {
                        resultData.Add(ds.ToString());
                    }

                    File.WriteAllLines(@"C:\steps\" + fileId + ".csv", resultData);
                    //break;
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
            Console.WriteLine("Input last date in format dd-MM-yyyy");
            string date = Console.ReadLine();

            DateTime result = DateTime.Parse(date);

            return result;
        }
    }
}
