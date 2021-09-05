using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DayAnalizer;
using StepRepository;

namespace Lobalug
{
    public class DataAnalizer
    {
        public static List<string> GetJason3Calendare(List<StepAtDay> Dataparsed)
        {
            List<string> result = new List<string>();

            var currentDate = DateTime.Now;
            

            for(int w=0;w<20;w++)
            {

                var years = Dataparsed.Select(i => i.TargetDate.Year).Distinct().Where(i => i != 2015 && i != DateTime.Now.Year);

                var targetDays = DayAnalizer.DayAnalizer.GetDaysLike(years, currentDate, true).ToList();
                List<StepAtDay> preSelected = new List<StepAtDay>();
                foreach (var i in Dataparsed)
                {
                    if (targetDays.Contains(i.TargetDate.Date))
                    {
                        preSelected.Add(i);
                        //targetDays = targetDays.Except(new List<DateTime> { i.TargetDate.Date }).ToList();
                    }

                    

                }

                var step = preSelected.Select(i => i.Value > 1500 ? i.Value : 1500);

                var min = step.Min();
                var avg = Math.Round(step.Average(), 0);
                var max = step.Max();

                result.Add($"{currentDate.ToString("dd.MM.yyyy")};{step.Min()};{Math.Round(step.Average(), 0)};{step.Max()}");

                currentDate =currentDate.AddDays(1);
            }

            return result;
        }
    }
}
