using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuMasterLib;
using StepRepository;
using System.IO;

namespace Lobalug
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            StepRepositoryFile stepRepository = new StepRepositoryFile(@"C:\stepsDataClean\cleanDataString.csv");
            CommonProperty commonProperty = new CommonProperty(stepRepository,new LobalugSettings());

            MenuMasterAction<CommonProperty> mainMenu = new MenuMasterAction<CommonProperty>();
            mainMenu.AddItem("Print Summary", SummaryPrinter.PrintSummary);

            mainMenu.AddItem("Empty Data", SummaryPrinter.EmptyData);

            mainMenu.AddItem("Print for day", SummaryPrinter.PrintForDay);

            mainMenu.AddItem("Input step`s data", StepsInserter.StepInsertMain);

            mainMenu.AddItem("Analysis data", AnalysisDataPrinter.AnalysisData);
            
            mainMenu.PrintAndWait(commonProperty);
        }
    }

    public static class SummaryPrinter
    {  
        public static void PrintForDay(CommonProperty property)
        {
            while(DateTime.TryParse(Console.ReadLine(), out DateTime date))
            {
                var value = property._repository.Get(date);
                if (value != null)
                {
                    Console.WriteLine($" {value.Value}");
                    Console.WriteLine("_________________________________________________________");
                }
            }  
        }


        public static void PrintSummary(CommonProperty property)
        {
            var data = property._repository.GetAll();

            var from = data.Select(i => i.TargetDate).Min();

            var to = data.Select(i => i.TargetDate).Max();

            Console.WriteLine($"From: {from.ToString("dd.MM.yyy")} To: {to.ToString("dd.MM.yyy")}");

            int allStep = data.Select(i => i.Value).Sum();

            Console.WriteLine($"All steps: {allStep}");

            int max = data.Select(i => i.Value).Max();

            Console.WriteLine($"Max steps: {max}");

            double avg = Math.Round(data.Select(i => i.Value).Average());

            Console.WriteLine($"Average steps: {avg}");
        }

        public static void EmptyData(CommonProperty property)
        {
            var data = property._repository.GetAll();

            var from = data.Select(i => i.TargetDate).Min();

            var to = data.Select(i => i.TargetDate).Max();

            var cart = from;
            while (cart <= to)
            {
                var val = data.FirstOrDefault(v=>v.TargetDate.Date==cart);
                if (val == null)
                {
                    Console.WriteLine(cart.ToString("dd.MM.yyy"));
                }
                cart = cart.AddDays(1);
            }
        }
    }

    public static class AnalysisDataPrinter
    {
        /// <summary>
        /// Пункт меню анализа данных
        /// </summary>
        /// <param name="property"></param>
        public static void AnalysisData(CommonProperty property)
        {
            var analysisMenu = new MenuMasterFunc<List<StepAtDay>, List<string>>();
            analysisMenu.AddItem("Ghost Race Calendar", DataAnalyzer.GhostRaceCalendar);
            analysisMenu.AddItem("Series", SeriesRatingCover);
            analysisMenu.AddItem("Steps in Month", DataAnalyzer.StepsInMonth);
            analysisMenu.AddItem("New target", TargetRecalculateCover);
            analysisMenu.AddItem("Distribution", DistributionCover);

            var data = property._repository.GetAll();
            var result = analysisMenu.PrintAndWait(data);

            foreach (var s in result)
            {
                Console.WriteLine(s);
            }

            Console.WriteLine("Input file name:");
            var fileName = Console.ReadLine();

            if (!string.IsNullOrEmpty(fileName))
            {
                File.WriteAllLines(fileName, result);
            }

        }

        private static List<string> DistributionCover(List<StepAtDay> data)
        {
            var pitch = ConsoleHelper.GetIntParameterFromUser("grid pitch", 500);
            var firstDate = ConsoleHelper.GetDateParameterFromUser("first day", DataHelper.FirstDate(data));
            var lastDate = ConsoleHelper.GetDateParameterFromUser("last day", DataHelper.LastDate(data));
            List<string> result = DataAnalyzer.Distribution(data, pitch, firstDate,lastDate);

            return result;
        }

        private static List<string> SeriesRatingCover(List<StepAtDay> data)
        {
            var target = ConsoleHelper.GetIntParameterFromUser("new target", 8000);
            var minDurations = ConsoleHelper.GetIntParameterFromUser("min durations", 2);
            var result = DataAnalyzer.SeriesRating(data,target, minDurations);
            return result;
        }

        private static List<string> TargetRecalculateCover(List<StepAtDay> data)
        {
            var target = ConsoleHelper.GetIntParameterFromUser("new target", 8000);
            var result = DataAnalyzer.TargetRecalculate(data, target, DateTime.Now);
            return result;
        }

    }
}
