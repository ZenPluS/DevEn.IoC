using DevEn.IoC.Core;
using DevEn.IoC.Attributes;
using DevEn.IoC.Sample;

namespace ConsoleAppDemo.Jobs
{
    [Job]
    public class JobOne : IJob
    {
        private readonly IInterface _classOne;
        private readonly ISecondInterface _classTwo;

        public JobOne(IInterface classOne, ISecondInterface classTwo)
        {
            _classOne = classOne;
            _classTwo = classTwo;
        }

        public void Execute()
        {
            
        }
    }
}
