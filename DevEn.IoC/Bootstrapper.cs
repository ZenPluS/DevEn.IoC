using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DevEn.IoC.Attributes;
using DevEn.IoC.Core;
using DevEn.IoC.Elements;

namespace DevEn.IoC
{
    public sealed class Bootstrapper : IBootstrapper
    {
        private readonly IDependencyContainer _container;
        private readonly Assembly[] _assembly;

        public Bootstrapper()
            : this(AppDomain.CurrentDomain.GetAssemblies())
        { }
        private Bootstrapper(Assembly[] assembly)
            : this(InternalContainer(assembly), assembly)
        { }
        public Bootstrapper(IDependencyContainer container)
            : this(container, AppDomain.CurrentDomain.GetAssemblies())
        { }
        public Bootstrapper(Func<IDependencyContainer> container)
            : this(container(), AppDomain.CurrentDomain.GetAssemblies())
        { }
        public Bootstrapper(Func<IDependencyContainer> container, Func<Assembly[]> assembly)
            : this(container(), assembly())
        { }
        public Bootstrapper(Func<IDependencyContainer> container, Assembly[] assembly)
            : this(container(), assembly)
        { }
        public Bootstrapper(IDependencyContainer container, Func<Assembly[]> assembly)
            : this(container, assembly())
        { }
        private Bootstrapper(IDependencyContainer container, Assembly[] assembly)
        {
            _container = container;
            _assembly = assembly;
        }

        private static IDependencyContainer InternalContainer(IEnumerable<Assembly> assembly)
        {
            var internalCont = new DependencyContainer();
            var dependencies = assembly.ToList().SelectMany(s => s.GetTypes())
                .Where(t => (t.GetCustomAttributes(typeof(IoCRegisterAttribute), true).Any()) && !t.IsInterface && !t.IsAbstract)
                .Select(
                    t => new
                    {
                        Interfaces = t.GetInterfaces().ToList(),
                        Concrete = t
                    })
                .ToList();

            dependencies.ForEach(item =>
            {
                if(item.Interfaces.Count > 0)
                    item.Interfaces.ForEach( i => internalCont.Register(i, item.Concrete));
                internalCont.Register(item.Concrete, item.Concrete);
            });

            return internalCont;
        }

        public void Run()
        {
            var jobSection = (JobConfigSection)ConfigurationManager.GetSection("jobConfiguration");
            if (jobSection == null)
            {
                Debug.WriteLine("jobConfiguration not found on app.config");
                return;
            }

            var jobsToRun = jobSection.Jobs.Cast<JobConfigElement>()
                .Where(job => job.ShouldRun)
                .OrderBy(job => job.Order)
                .ToList();

            if (jobsToRun.Count <=0)
            {
                Debug.WriteLine("no JobConfigElement found on AppConfig");
                return;
            }

            var jobTypes = _assembly.ToList().SelectMany(assembly => assembly.GetTypes())
                .Where(t => (t.GetCustomAttributes(typeof(JobAttribute), true).Any()) && typeof(IJob).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .ToDictionary(t => t.Name, t => t);

            if (jobTypes.Count <=0)
            {
                Debug.WriteLine("no Jobs found in current assembly");
                return;
            }

            var jobsToBeExecuted = jobsToRun
                .Select(
                    jobToBeRun =>
                    {
                        var isPresent = jobTypes.TryGetValue(jobToBeRun.Name, out var job);
                        return !isPresent
                            ? (null, -1)
                            : (job, jobToBeRun.Order);
                    })
                .OrderBy(x => x.Order)
                .ToList();

            if (jobsToBeExecuted.Count <=0)
            {
                Debug.WriteLine("no Jobs To Be Executed found in AppConfig");
                return;
            }

            jobsToBeExecuted.ForEach(x => _container.Register(x.job));
            jobsToBeExecuted.ForEach(
                x =>
                {
                    var job = _container.Resolve<IJob>(x.job);
                    if (job == null)
                    {
                        Debug.WriteLine($"Not Found: Job #{x.Order} of #{jobsToBeExecuted.Count} - {x.job.Name}");
                        return;
                    }

                    Debug.WriteLine($" Starting Job #{x.Order} of #{jobsToBeExecuted.Count} - {x.job.Name}");
                    job.Execute();
                });

        }
    }
}
