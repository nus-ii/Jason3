using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using DayAnalizer;
using StepRepository;

namespace Lobalug
{
    /// <summary>
    /// Класс - держатель логики анализа данных
    /// </summary>
    public class DataAnalyzer
    {
        /// <summary>
        /// Построение календаря соревнования с призраком (данные за прошлые годы приведённые к календарным дням в этом году)
        /// </summary>
        /// <param name="parsedData">Данные о шагах</param>
        /// <returns></returns>
        public static List<string> GhostRaceCalendar(List<StepAtDay> parsedData)
        {
            List<string> result = new List<string>();

            var currentDate = DateTime.Now;
            var years = parsedData.Select(i => i.TargetDate.Year).Distinct().Where(i => i != 2015 && i != DateTime.Now.Year);

            for (int w = 0; w < 90; w++)
            {
                var targetDays = DayAnalizer.DayAnalizer.GetDaysLike(years, currentDate, true).ToList();
                var step = parsedData.Where(i => targetDays.Contains(i.TargetDate.Date)).ToList().Select(i => i.Value);

                var min = step.Min();
                var avg = Convert.ToInt32(Math.Round(step.Average(), 0));
                var max = step.Max();

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

        /// <summary>
        /// Метод получения стрелочки для календаря соревнования с призраком
        /// </summary>
        /// <param name="value">Значение для отображения</param>
        /// <param name="maxValue">Максимальное значение</param>
        /// <param name="maxLength">Максимальная длинна</param>
        /// <param name="filler">Символ заполнения стрелочки</param>
        /// <param name="label">Название</param>
        /// <param name="printValue">Печатать значение или нет</param>
        /// <returns></returns>
        private static string GetArrow(int value,int maxValue,int maxLength, string filler,string label,bool printValue=true)
        {
            var charValue = maxValue/ maxLength;
            int chars = value / charValue;
            var result = chars <= maxLength ? filler+">" : "~>";
           
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

        public static List<string> GetSeriesRating(List<StepAtDay> parsedData)
        {
            List<StepSeries> stepSeries = new List<StepSeries>();
            int targetValue = 8000;

            bool activeSeriesFlag = false;
            foreach (var sad in parsedData)
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

            var resultCandidate = stepSeries.GroupBy(s => s.Days).ToList().Select(g => new SeriesGroup{Key=g.Key,Values=g.OrderByDescending(i=>i.End) }).OrderByDescending(h=>h.Key);

            List<string> result = new List<string>();

            foreach (var sg in resultCandidate)
            {
                if (sg.Key >= 2)
                {
                    result.Add($"> {sg.Key} days <");
                    foreach (var s in sg.Values)
                    {
                        result.Add(s.ToString());
                    }
                }
            }

            return result;
        }

        internal static List<string> StepsInMonth(List<StepAtDay> parsedData)
        {
            List<StepsInMonth> stepsInMonths = new List<StepsInMonth>();
            foreach (var sad in parsedData)
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

        public static List<string> TargetRecalculate(List<StepAtDay> parsedData,int target,DateTime cDate)
        {
            List<string> result = new List<string>();

            var targetMonthData = parsedData.Where(i => i.TargetDate.Year == cDate.Year && i.TargetDate.Month == cDate.Month);
            var targetMonthLength = DaysInThisMonth(cDate.Month, cDate.Year);
            int newTarget = 0;

            newTarget = ((target * targetMonthLength) - targetMonthData.Select(i => i.Value).Sum()) / (targetMonthLength - targetMonthData.Count());

            result.Add($"New target: {newTarget}");
            return result;
        }

        private static int DaysInThisMonth(int month,int year)
        {
            int result = 0;
            var cartDate = new DateTime(year, month, 1);
            while (cartDate.Month == month)
            {
                result++;
                cartDate=cartDate.AddDays(1);
                
            }
            return result;
        }
    }

    public class SeriesGroup
    {
        public int Key { get; set; }

        public IEnumerable<StepSeries> Values { get; set; }
    }

    public class StepsInMonth{

        public int Month;

        public int Year;

        public int Steps;

        public int Days;

        public int Avg => Steps / Days;

        public StepsInMonth(int year, int month, int steps)
        {
            this.Year = year;
            this.Month = month;
            this.Steps = steps;
            Days = 1;
        }

        public void AddSteps(int steps)
        {
            this.Steps += steps;
            Days++;
        }

        public override string ToString()
        {
            string result = $"{this.Month}.{this.Year};{this.Avg}";
            return result;
        }

    }

    public class StepSeries
    {
        public DateTime Start;

        public DateTime End => Start.AddDays(Days);

        public int Days;

        public StepSeries(DateTime start)
        {
            this.Start = start;
            Days = 1;
        }

        public void AddDay()
        {
            Days++;
        }

        public override string ToString()
        {
            string result = $"start:{this.Start:dd.MM.yyyy} end:{this.End:dd.MM.yyyy}";
            return result;
        }

    }


}
