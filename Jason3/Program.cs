using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StepRepository;
using DayAnalizer;

namespace Jason3
{
    class Program
    {
        static void Main(string[] args)
        {
            //var inData = File.ReadAllLines(@"C:\stepsDataClean\cleanDataString.csv");

            //var c = CultureInfo.GetCultureInfo("ru-Ru");

            StepRepository.StepRepositoryFile stepRepository = new StepRepository.StepRepositoryFile(@"C:\stepsDataClean\cleanDataString.csv");

            var Dataparsed = stepRepository.GetAll();//inData.Select(s => s.Split(';')).Select(i => new Tuple<DateTime, LazyItem>(DateTime.Parse(i[0], c), new LazyItem(i[1])));

            var years = Dataparsed.Select(i => i.TargetDate.Year).Distinct().Where(i => i != 2015 && i != DateTime.Now.Year);

            var targetDays = DayAnalizer.DayAnalizer.GetDaysLike(years, DateTime.Now, true).ToList();

            var month = targetDays.Select(d => d.Month).Distinct().First();

            List<StepAtDay> preSelected = new List<StepAtDay>();

            foreach (var i in Dataparsed)
            {
                if (targetDays.Contains(i.TargetDate.Date))
                {
                    preSelected.Add(i);
                    targetDays = targetDays.Except(new List<DateTime> { i.TargetDate.Date }).ToList();
                }

            }

            foreach (var d in preSelected)
            {
                Console.WriteLine($"Date:{d.TargetDate.Date.ToString("dd.MM.yyyy")} Steps:{d.Value}");
            }

            var step = preSelected.Select(i => i.Value > 1500 ? i.Value : 1500);
            Console.WriteLine($"MIN:{step.Min()}");

            Console.WriteLine($"AVG:{Math.Round(step.Average(), 0)}");

            Console.WriteLine($"MAX:{step.Max()}");

            string outString = $"DAY:{DateTime.Now.ToString("dd.MM.yyyy")} MIN:{step.Min()} AVG:{Math.Round(step.Average(), 0)} MAX:{step.Max()}";

            string filePath = @"C:\outbox\jason3lite\" + Guid.NewGuid().ToString().Split('-')[0] + ".txt";
            File.WriteAllText(filePath, outString);
            Console.ReadLine();
        }
    }

    public class LazyItem
    {
        private int? hiddedValue;

        private readonly string stringValue;

        public int Value
        {
            get
            {
                if (hiddedValue.HasValue)
                {
                    return hiddedValue.Value;
                }
                else
                {
                    hiddedValue = Int32.Parse(stringValue);
                    return hiddedValue.Value;
                }
            }
        }

        public LazyItem(string value)
        {
            stringValue = value;
        }

        public LazyItem(int value)
        {
            hiddedValue = value;
        }
    }
}
