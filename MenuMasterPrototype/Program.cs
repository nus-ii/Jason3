using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuMasterLib;

namespace MenuMasterPrototype
{
    class Program
    {
        static void Main(string[] args)
        {
   
            var menu=new MenuMasterAction<Settings>();

            menu.AddItem("Test", TestFunc.Print);

            menu.AddItem("Test2", delegate { Console.WriteLine("eeeeee"); });

            menu.AddItem("Test3", menu.PrintAndWait);

            Settings set = new Settings
            {
                Foo = "Hello World"
            };

            menu.PrintAndWait(set);

            Console.ReadLine();
        }
    }

    public class Settings
    {
        public string Foo;
    }

    public static class TestFunc
    {
        public static void Print(Settings obj)
        {
            Console.WriteLine(obj.Foo);

        }
    }

 
}
