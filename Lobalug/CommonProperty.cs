using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StepRepository;

namespace Lobalug
{
    public class CommonProperty
    {
        public IStepRepository _repository;

        public LobalugSettings _settings;

        public CommonProperty(IStepRepository repository,LobalugSettings settings)
        {
            _repository = repository;
            _settings = settings;
        }
    }

    public class LobalugSettings
    {

    }
}
