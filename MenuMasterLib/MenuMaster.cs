﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuMasterLib
{
    public class MenuMasterAction<T> where T : class
    {
        Dictionary<int, MenuItem<T>> Items;

        public MenuMasterAction()
        {
            Items = new Dictionary<int, MenuItem<T>>();
        }

        public void AddItem(string Name, Action<T> Item)
        {
            int lastIndex = Items.Count > 0 ? Items.Select(i => i.Key).Max() : 0;
            Items.Add(++lastIndex, new MenuItem<T> { Name = Name, ContainedAction = Item });
        }

        public void PrintAndWait(T obj)
        {
            for (; ; )
            {
                Console.Clear();             

                if(Items.Values.All(i=>i.Selected==false))
                {
                    var first = Items[1];
                    first.Selected = true;
                }  

                foreach (var i in Items)
                {
                    string fmark = i.Value.Selected ? ">" : " ";
                    string lmark = i.Value.Selected ? "<" : " ";
                    if (i.Value.Selected)
                    {
                        SwithColors();
                    }
                    Console.WriteLine($"{fmark} {i.Value.Name} {lmark}");
                    if (i.Value.Selected)
                    {
                        SwithColors();
                    }
                }

                ConsoleKeyInfo k = Console.ReadKey();

                if(k.Key== ConsoleKey.Enter)
                {
                    Console.Clear();
                    Items.First(i => i.Value.Selected).Value.ContainedAction(obj);
                    //Console.WriteLine($"{Items[targetIndex.Value].Name} finished");
                    Console.ReadLine();
                }
                else
                {
                    int selectedIndex = Items.First(i => i.Value.Selected).Key;
                    ResetSelected(ref Items);
                    
                    if (k.Key == ConsoleKey.UpArrow)
                    {
                        selectedIndex--;
                        if (Items.Keys.Contains(selectedIndex))
                        {                           
                            Items[selectedIndex].Selected = true;
                        }
                        else
                        {
                            var last = Items.Last();
                            last.Value.Selected = true;
                        }
                       
                    }

                    if (k.Key == ConsoleKey.DownArrow)
                    {
                        selectedIndex++;
                        if (Items.Keys.Contains(selectedIndex))
                        {
                            Items[selectedIndex].Selected = true;
                        }
                        else
                        {
                            var First = Items.First();
                            First.Value.Selected = true;
                        }
                
                    }
                }

        
                
            }
        }

        private void ResetSelected(ref Dictionary<int, MenuItem<T>> items)
        {
            foreach(var i in items)
            {
                i.Value.Selected = false;
            }
        }

        private void SwithColors()
        {
            var ActiveForeground = Console.ForegroundColor;          
            Console.ForegroundColor = Console.BackgroundColor;
            Console.BackgroundColor = ActiveForeground;
        }
    }

    public class MenuItem<T> where T : class
    {
        public string Name;
        public Action<T> ContainedAction;
        public bool Selected;
    }

    static class Helper
    {
        public static int? NiceIntParse(this string value)
        {
            int? result = null;
            if (!string.IsNullOrEmpty(value))
            {
                if (Int32.TryParse(value, out int targetIndex))
                {
                    result = targetIndex;
                }
            }
            return result;

        }
    }
}
