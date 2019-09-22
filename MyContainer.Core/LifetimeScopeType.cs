using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyContainer.Core
{
    public enum LifetimeScopeType
    {
        PerDependency,
        Single,
        PerLifetimeScope
    }
}
