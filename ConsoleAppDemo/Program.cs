using DevEn.IoC;
using DevEn.IoC.Sample;
using System.Configuration;
using DevEn.IoC.Core;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;

namespace ConsoleAppDemo
{
    internal class Program
    {
        private static void Main()
        {
            var bootstrapper = new Bootstrapper(Container);
            bootstrapper.Run();
        }

        private static IDependencyContainer Container()
        {
            var container = new DependencyContainer()
                .Register<IInterface, ClassDoSomething>();
            container.Configuration["foo"] = "bar";
            container.RegisterConfigurationParameter("prm", "i'm a param")
                .Register<ISecondInterface, AnotherClassWithMoreParams>()
                .Register<IThirdInterface, NoneParamClassWithInterface>()
                .Register<NoneParamClass>()
                .RegisterConfigurationParameter("crmConnectionString", ConfigurationManager.ConnectionStrings["name-here"]?.ConnectionString)
                .Register<IOrganizationService,CrmServiceClient>();
        }

    }
}
