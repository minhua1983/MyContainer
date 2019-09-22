using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyContainer.Core
{
    public class LifetimeScope : ILifetimeScope, IDisposable
    {
        IContainer _container;
        bool _isDisposed = false;
        List<IDisposable> _disposalList = new List<IDisposable>();
        /// <summary>
        /// LifetimeScope级别实例字典
        /// </summary>
        ConcurrentDictionary<Type, object> _lifetimeScopeDictionary = new ConcurrentDictionary<Type, object>();

        public LifetimeScope(IContainer container)
        {
            _container = container;
        }

        public T Resolve<T>(ILifetimeScope lifetimeScope = null)
        {
            return _container.Resolve<T>(lifetimeScope);
        }

        ~LifetimeScope()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool manual)
        {
            //如果还没释放
            if (!_isDisposed)
            {
                //无论是手动或被动模式，都需要释放非托管资源
                _disposalList.ForEach(m => m.Dispose());

                //如果是手动模式释放，即调用Dispose方法或用using关键字
                if (manual)
                {
                    //释放托管对象（由于取消了终结，所以必须释放托管对象）
                    _lifetimeScopeDictionary = null;
                    _disposalList = null;
                }

                //设置为已经释放
                _isDisposed = true;
            }
        }

        public void AddDisposal<T>(T instance)
        {
            _disposalList.Add((IDisposable)instance);
        }

        public void AddInstanceByType<T>(T instance)
        {
            _lifetimeScopeDictionary.TryAdd(typeof(T), instance);
        }

        public bool IsInstancedByType<T>()
        {
            if (_lifetimeScopeDictionary.ContainsKey(typeof(T)))
            {
                return _lifetimeScopeDictionary[typeof(T)] == null ? false : true;
            }
            return false;
        }

        public T GetInstanceByType<T>()
        {
            return (T)_lifetimeScopeDictionary[typeof(T)];
        }
    }
}
