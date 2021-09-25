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
        public static List<string> GhostRaceCalendar(List<StepAtDay> Dataparsed)
        {
            List<string> result = new List<string>();

            var currentDate = DateTime.Now;
            var years = Dataparsed.Select(i => i.TargetDate.Year).Distinct().Where(i => i != 2015 && i != DateTime.Now.Year);

            for (int w = 0; w < 90; w++)
            {
                var targetDays = DayAnalizer.DayAnalizer.GetDaysLike(years, currentDate, true).ToList();
                List<StepAtDay> preSelected = new List<StepAtDay>();
                foreach (var i in Dataparsed)
                {
                    if (targetDays.Contains(i.TargetDate.Date))
                    {
                        preSelected.Add(i);
                    }
                }

                var step = preSelected.Select(i => i.Value > 1500 ? i.Value : 1500);

                var min = step.Min();
                var avg = Convert.ToInt32(Math.Round(step.Average(), 0));
                var max = step.Max();

                //result.Add($"{currentDate.ToString("dd.MM.yyyy")};{step.Min()};{Math.Round(step.Average(), 0)};{step.Max()}");

                result.Add(new String('_', 47));
                result.Add(currentDate.ToString("dd.MM.yyyy"));
                result.Add(GetArrow(min, 17500, 35, "-", "min"));
                result.Add(GetArrow(avg, 17500, 35,"-","avg"));
                result.Add(GetArrow(max, 17500, 35, "-", "max"));
                result.Add(GetArrow(17500, 17500, 35, "=", "fact",false));

                currentDate = currentDate.AddDays(1);
            }
            return result;
        }

        private static string GetArrow(int value,int maxValue,int maxLength, string filler,string label,bool printValue=true)
        {
            string result = "";

             
            var charValue = maxValue/ maxLength;
            int chars = value / charValue;
            result = chars <= maxLength ? filler+">" : "~>";
           
            chars = chars <= maxLength ? chars : maxLength;

            for(int i = 0; i < chars-1; i++)
            {
                result = filler+result;
            }

            var factLabel = string.IsNullOrEmpty(label) ? "" : label;
            var factValue = printValue ? $": {value}" : "";
            result = $"{result}{factLabel}{factValue}";
            return result;
        }

        public static List<string> GetSeriesRating(List<StepAtDay> Dataparsed)
        {
            List<string> result = new List<string>();

            List<StepSeries> stepSeries = new List<StepSeries>();
            int targetValue = 8000;

            bool activeSeriesFlag = false;
            foreach (var sad in Dataparsed)
            {
                if (sad.Value >= targetValue)
                {
                    if (activeSeriesFlag)
                    {
                        stepSeries.Last().AddDay();
                    }
                    else
                    {
                        stepSeries.Add(new StepSeries(sad.TargetDate));
                        activeSeriesFlag = true;
                    }
                }
                else
                {
                    activeSeriesFlag = false;
                }
            }

            stepSeries.Sort(delegate (StepSeries a, StepSeries b)
            {
                return b.days.CompareTo(a.days);
            });


            result = stepSeries.Where(s => s.days > 1).Select(s => { return s.ToString(); }).ToList();

            return result;
        }

        internal static List<string> StepsInMonth(List<StepAtDay> Dataparsed)
        {
            List<StepsInMonth> stepsInMonths = new List<StepsInMonth>();
            foreach (var sad in Dataparsed)
            {
                var candidates = stepsInMonths.Where(i => i.Month == sad.TargetDate.Month && sad.TargetDate.Year == i.Year);

                if (candidates != null&& candidates.Count()>0)
                {
                    var target = stepsInMonths.First(i => i.Month == sad.TargetDate.Month && sad.TargetDate.Year == i.Year);
                    target.AddSteps(sad.Value);
                }
                else
                {
                    stepsInMonths.Add(new StepsInMonth(sad.TargetDate.Year, sad.TargetDate.Month, sad.Value));
                }
            }

            List<string> result = stepsInMonths.Select(i => { return i.ToString(); }).ToList();
            return result;
        }
    }

    public class StepsInMonth{

        public int Month;

        public int Year;

        public int Steps;

        public int days;

        public int Avg
        {
            get
            {
                return Steps / days;
            }
        }

        public StepsInMonth(int Year, int Month, int Steps)
        {
            this.Year = Year;
            this.Month = Month;
            this.Steps = Steps;
            days = 1;
        }

        public void AddSteps(int Steps)
        {
            this.Steps += Steps;
            days++;
        }

        public override string ToString()
        {
            string result = $"{this.Month}.{this.Year};{this.Avg}";
            return result;
        }

    }

    public class StepSeries
    {
        public DateTime start;

        public DateTime end
        {
            get
            {
                return start.AddDays(days);
            }
        }

        public int days;

        public StepSeries(DateTime start)
        {
            this.start = start;
            days = 1;
        }

        public void AddDay()
        {
            days++;
        }

        public override string ToString()
        {
            string result = $"Days: {this.days} start:{this.start.ToString("dd.MM.yyyy")} end:{this.end.ToString("dd.MM.yyyy")}";
            return result;
        }

    }


}
