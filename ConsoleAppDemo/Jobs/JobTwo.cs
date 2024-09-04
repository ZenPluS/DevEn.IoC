using DevEn.IoC.Attributes;
using DevEn.IoC.Core;
using DevEn.IoC.Sample;

namespace ConsoleAppDemo.Jobs
{
    [Job]
    public class JobTwo : IJob
    {
        private readonly IInterface _classOne;
        private readonly ISecondInterface _classTwo;

        public JobTwo(IInterface classOne, ISecondInterface classTwo)
        {
            _classOne = classOne;
            _classTwo = classTwo;
        }

        public void Execute()
        {
            
        }
    }
}
