using System.Configuration;

namespace DevEn.IoC.Elements
{
    public sealed class JobConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name => (string)this["name"];

        [ConfigurationProperty("order", IsRequired = true)]
        public int Order => (int)this["order"];

        [ConfigurationProperty("shouldrun", IsRequired = true)]
        public bool ShouldRun => (bool)this["shouldrun"];

    }
}
