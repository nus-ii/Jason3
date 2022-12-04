using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepRepository
{
    public class StepRepositoryFile : IStepRepository
    {
        private List<StepAtDay> _values;

        private string Path { set; get; }

        private string CultureName { set; get; }

        private CultureInfo CultureInfo { set; get; }

        private char SeparatorValue { set; get; }

        public StepRepositoryFile(string connectionString, string culture = "ru-Ru", char separator = ';')
        {
            Path = connectionString;
            CultureName = culture;
            SeparatorValue = separator;
            CultureInfo = CultureInfo.GetCultureInfo(culture);
            ReadFromFile();
        }

        public StepAtDay Get(DateTime day)
        {
            return _values.FirstOrDefault(i => i.TargetDate.Date == day.Date);
        }

        public List<StepAtDay> GetAll()
        {
            return _values;
        }

        public void Insert(List<StepAtDay> newValues)
        {
            _values=_values.Union(newValues, new StepAtDayComparere()).OrderBy(i => i.TargetDate).ToList(); 
            List<string> lines = _values.Select(i => $"{i.TargetDate.Date.ToString(CultureInfo.DateTimeFormat.ShortDatePattern)}{SeparatorValue}{i.Value}").ToList();
            File.WriteAllLines(Path, lines);
            ReadFromFile();
        }

        private void ReadFromFile()
        {   
            string[] inData = File.ReadAllLines(Path);
            _values = inData.Where(l => !string.IsNullOrEmpty(l) &&l.Contains(';')).Select(l => l.Split(SeparatorValue)).Select(a => new StepAtDay(DateTime.Parse(a[0], CultureInfo), a[1])).ToList();
        }
    }

    class StepAtDayComparere : IEqualityComparer<StepAtDay>
    {
        public bool Equals(StepAtDay x, StepAtDay y)
        {
            return x.TargetDate.Date.Equals(y.TargetDate.Date) && (x.Value.Equals(y.Value));
        }

        public int GetHashCode(StepAtDay obj)
        {
            return obj.TargetDate.GetHashCode() + obj.Value.GetHashCode();
        }
    }
}
