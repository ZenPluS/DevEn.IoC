using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEn.IoC.Sample
{
    public class NoneParamClassWithInterface : IThirdInterface
    {
        public void DoSomething()
        {
            Console.WriteLine("i'm alone");
        }
    }
}
