using System;

namespace DevEn.IoC.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class JobAttribute : Attribute
    {
        public JobAttribute() { }
    }
}
