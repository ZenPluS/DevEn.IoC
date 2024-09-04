using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEn.IoC.Sample
{
    public class AnotherClassWithMoreParams : ISecondInterface
    {
        private readonly IInterface _myParam;
        private readonly string _foo;

        public AnotherClassWithMoreParams(string foo, IInterface doSome)
        {
            _myParam = doSome;
            _foo = foo;
        }

        public void DoSomething()
        {
            _myParam.DoSomething();
        }
    }
}
