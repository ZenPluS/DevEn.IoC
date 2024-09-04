﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEn.IoC.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class IoCRegisterAttribute : Attribute
    {
        public IoCRegisterAttribute() { }
    }
}
