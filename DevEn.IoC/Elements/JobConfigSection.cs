using System.Configuration;

namespace DevEn.IoC.Elements
{
    public sealed class JobConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("jobs")]
        public JobConfigElementCollection Jobs => (JobConfigElementCollection)this["jobs"];
    }
}
