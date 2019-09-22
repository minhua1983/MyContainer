using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyContainer.Core
{
    public class LifetimeScopeItem
    {
        public Type ItemType { get; set; }
        public ILifetimeScope Scope { get; set; }
        public object Data { get; set; }
    }
}
