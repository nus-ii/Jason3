using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuMasterLib;

namespace Lobalug
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            StepRepository.StepRepository stepRepository = new StepRepository.StepRepository(@"C:\stepsDataClean\cleanDataString.csv");
            CommonProperty CommonProperty = new CommonProperty(stepRepository,new LobalugSettings());

            MenuMasterAction<CommonProperty> mainMenu = new MenuMasterAction<CommonProperty>();
            mainMenu.AddItem("Print Summary", SummaryPrinter.Print);

            mainMenu.AddItem("Empty Data", SummaryPrinter.EmptyData);

            mainMenu.AddItem("Print for day", SummaryPrinter.PrintForDay);

            mainMenu.AddItem("Input step`s data", StepsInserter.StepInsertMain);

            mainMenu.AddItem("Analize data", delegate { Console.WriteLine("Sorry, we are under construction"); });
            
            mainMenu.PrintAndWait(CommonProperty);
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


        public static void Print(CommonProperty property)
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
                if (cart == null)
                {
                    Console.WriteLine(cart.ToString("dd.MM.yyy"));
                }
                cart = cart.AddDays(1);
            }
        }
    }
}
