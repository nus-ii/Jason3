using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepRepository
{
    public interface IStepRepository
    {
        List<StepAtDay> GetAll();

        StepAtDay Get(DateTime day);

        void Insert(DateTime day, int Steps);
    }
}
