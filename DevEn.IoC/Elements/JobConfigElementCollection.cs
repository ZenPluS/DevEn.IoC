using System.Configuration;

namespace DevEn.IoC.Elements
{
    [ConfigurationCollection(typeof(JobConfigElement))]
    public sealed class JobConfigElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new JobConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((JobConfigElement)element).Name;
        }
    }
}
