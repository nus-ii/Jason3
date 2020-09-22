using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepRepository
{
    public class StepRepository : IStepRepository
    {
        private readonly List<StepAtDay> values;

        private readonly string path;

        private readonly string cultureName;

        private readonly char separatorVale;

        public StepRepository(string ConnectionString, string culture="ru-Ru",char separator=';')
        {
            path = ConnectionString;
            cultureName = culture;
            separatorVale = separator;
            CultureInfo cultureInfo = CultureInfo.GetCultureInfo(cultureName);

            string[] inData = File.ReadAllLines(ConnectionString);
            values = inData.Select(l=>l.Split(separator)).Select(a=>new StepAtDay(DateTime.Parse(a[0], cultureInfo),a[1])).ToList();            
        }

        public StepAtDay Get(DateTime day)
        {
            return values.FirstOrDefault(i => i.TargetDate.Date == day.Date);
        }

        public List<StepAtDay> GetAll()
        {
            return values;
        }

        public void Insert(DateTime day, int Steps)
        {
            throw new NotImplementedException();
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
