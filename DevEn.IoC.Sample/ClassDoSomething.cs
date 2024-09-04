using System;

namespace DevEn.IoC.Sample
{
    public class ClassDoSomething : IInterface
    {
        private readonly string _fakeParam;

        public ClassDoSomething(string prm)
        {
            _fakeParam = prm;
        }

        public void DoSomething()
        {
            Console.WriteLine(_fakeParam);
        }
    }
}
