using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyContainer.Core
{
    public interface ILifetimeScope : IDisposable
    {
        T Resolve<T>(ILifetimeScope lifetimeScope = null);
        void AddDisposal<T>(T instance);
        void AddInstanceByType<T>(T instance);
        bool IsInstancedByType<T>();
        T GetInstanceByType<T>();
    }
}
