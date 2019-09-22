using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections.Concurrent;

namespace MyContainer.Core
{
    public class ContainerBuilder : IContainerBuilder
    {
        /// <summary>
        /// 类型字典
        /// </summary>
        ConcurrentDictionary<Type, LifetimeScopeType> _typeDictionary = new ConcurrentDictionary<Type, LifetimeScopeType>();

        /// <summary>
        /// 线程安全的同步字典
        /// </summary>
        static ConcurrentDictionary<Type, Type> _dictionary = new ConcurrentDictionary<Type, Type>();
        static IContainer _container;

        public ContainerBuilder()
        {

        }

        public IContainer Build<T>() where T : IContainer
        {
            //所有继承自Container的类型必须都有这个静态方法
            //_container = Container.GetInstance();
            //必须加上3个条件BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static，不然得不到静态方法GetInstance
            _container = (IContainer)typeof(T).GetMethod("GetInstance", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[] { _dictionary, _typeDictionary });
            return _container;
        }


        /// <summary>
        /// 将TClass类以TInterface接口形式注入到容器
        /// </summary>
        /// <typeparam name="TClass">实现类</typeparam>
        /// <typeparam name="TInterface">实现接口</typeparam>
        public IContainerBuilder Register<TClass, TInterface>() where TClass : class, TInterface
        {
            _dictionary.TryAdd(typeof(TInterface), typeof(TClass));
            return this;
        }

        /// <summary>
        /// 将TClass类注入到容器
        /// </summary>
        /// <typeparam name="TClass">实现类</typeparam>
        public IContainerBuilder Register<TClass>() where TClass : class
        {
            _dictionary.TryAdd(typeof(TClass), typeof(TClass));
            return this;
        }

        public IContainerBuilder Register(Type type)
        {
            _dictionary.TryAdd(type, type);
            return this;
        }

        public void AsInstancePerDependency<TClass>()
        {
            _typeDictionary.TryAdd(typeof(TClass), LifetimeScopeType.PerDependency);
        }

        public void AsSingleInstance<TClass>()
        {
            _typeDictionary.TryAdd(typeof(TClass), LifetimeScopeType.Single);
        }

        public void AsPerLifetimeScope<TClass>()
        {
            _typeDictionary.TryAdd(typeof(TClass), LifetimeScopeType.PerLifetimeScope);
        }
    }
}
