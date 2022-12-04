using System;

namespace Lobalug
{
    /// <summary>
    /// Класс помощи в получении данных от пользователя
    /// </summary>
    public static class ConsoleHelper
    {
        /// <summary>
        /// Получение параметра от пользователя
        /// </summary>
        /// <param name="name">Название параметра</param>
        /// <param name="def">Значение по умолчанию</param>
        /// <returns></returns>
        public static int GetIntParameterFromUser(string name, int def)
        {
            int target = 0;
            bool succeedParse = false;
            while (!succeedParse)
            {
                Console.Clear();
                Console.Write($"Input {name} value (by default {def}):");
                var targetCandidate = Console.ReadLine();

                if (string.IsNullOrEmpty(targetCandidate))
                    return def;

                succeedParse = int.TryParse(targetCandidate, out target);
            }

            return target;
        }

        /// <summary>
        /// Получение параметра от пользователя
        /// </summary>
        /// <param name="name">Название параметра</param>
        /// <param name="def">Значение по умолчанию</param>
        /// <returns></returns>
        public static DateTime GetDateParameterFromUser(string name, DateTime def)
        {

            DateTime target = def;
            bool succeedParse = false;
            while (!succeedParse)
            {
                Console.Clear();
                Console.Write($"Input {name} value (by default {def}):");
                var targetCandidate = Console.ReadLine();

                if (string.IsNullOrEmpty(targetCandidate))
                    return def;

                succeedParse = DateTime.TryParse(targetCandidate, out target);
            }

            return target;
        }
    }
}