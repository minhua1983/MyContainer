using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Collections.Concurrent;

namespace MyContainer.Core
{
    public class Container : IContainer
    {
        /// <summary>
        /// 单例字典
        /// </summary>
        ConcurrentDictionary<Type, object> _singleDictionary = new ConcurrentDictionary<Type, object>();

        /// <summary>
        /// 类型字典
        /// </summary>
        ConcurrentDictionary<Type, LifetimeScopeType> _typeDictionary = new ConcurrentDictionary<Type, LifetimeScopeType>();

        /// <summary>
        /// 线程安全的同步字典
        /// </summary>
        ConcurrentDictionary<Type, Type> _dictionary = new ConcurrentDictionary<Type, Type>();

        /// <summary>
        /// 锁的帮助实例
        /// </summary>
        static object _lockHelper = new object();

        /// <summary>
        /// 实例（用于双重检验锁，加volatile为了不被本地线程缓存，从而确认多个线程可以正确处理该变量）
        /// </summary>
        static volatile Container _myContainer;

        /// <summary>
        /// 私有构造，以避免调用者主动构造对象
        /// </summary>
        private Container()
        {

        }

        /// <summary>
        /// 获取实例
        /// </summary>
        /// <returns>实例</returns>
        public static Container GetInstance(ConcurrentDictionary<Type, Type> dictionary, ConcurrentDictionary<Type, LifetimeScopeType> typeDictionary)
        {
            if (_myContainer == null)
            {
                lock (_lockHelper)
                {
                    if (_myContainer == null)
                    {
                        _myContainer = new Container();
                        _myContainer._dictionary = dictionary;
                        _myContainer._typeDictionary = typeDictionary;
                    }
                }
            }
            return _myContainer;
        }

        /*
        /// <summary>
        /// 以有参数构造获取T接口类型的实例（构造函数不准使用默认参数和命名参数）
        /// </summary>
        /// <typeparam name="T">T接口类型</typeparam>
        /// <returns>实例</returns>
        public T Resolve<T>(params object[] parameters)
        {
            try
            {

                Type classType;
                //尝试读取T接口类型对应的实现类型
                _dictionary.TryGetValue(typeof(T), out classType);
                //获取第一个构造函数
                ConstructorInfo constructor = classType.GetConstructors().FirstOrDefault();

                //获取构造函数参数
                //ParameterInfo[] parameters = constructor.GetParameters();
                object o = constructor.Invoke(parameters);
                return (T)o;
            }
            catch
            {
                return default(T);
            }
        }
        //*/

        /// <summary>
        /// 以无参数构造获取T接口类型的实例
        /// </summary>
        /// <typeparam name="T">T接口类型</typeparam>
        /// <returns>实例</returns>
        public T Resolve<T>(ILifetimeScope lifetimeScope = null)
        {
            try
            {
                T temp;
                LifetimeScopeType lifetimeScopeType = GetLifetimeScopeType(typeof(T));
                bool isDisposal = typeof(IDisposable).IsAssignableFrom(typeof(T));

                if (lifetimeScopeType == LifetimeScopeType.Single)
                {
                    //单例
                    if (_singleDictionary.ContainsKey(typeof(T)))
                    {
                        //已存在
                        temp = (T)_singleDictionary[typeof(T)];
                    }
                    else
                    {
                        //不存在
                        temp = InnerResolve<T>();
                        _singleDictionary.TryAdd(typeof(T), temp);
                    }
                }
                else if (lifetimeScopeType == LifetimeScopeType.PerLifetimeScope)
                {
                    //生命周期，即生命周期中只能实例化一次

                    bool isInstanced = lifetimeScope.IsInstancedByType<T>();

                    if (isInstanced)
                    {
                        //如果已经实例化，则从lifetimeScope实例中获取T实例
                        temp = lifetimeScope.GetInstanceByType<T>();
                    }
                    else
                    {
                        //如果还没实例化，则由Container实例来实例化T
                        temp = InnerResolve<T>();

                        //添加到LifetimeScope级别实例字典
                        lifetimeScope.AddInstanceByType<T>(temp);

                        //如果实现IDispose接口
                        if (isDisposal)
                        {
                            lifetimeScope.AddDisposal(temp);
                        }
                    }
                }
                else
                {
                    //每次都实例化
                    temp = InnerResolve<T>();
                    //如果实现IDispose接口
                    if (isDisposal)
                    {
                        lifetimeScope.AddDisposal(temp);
                    }
                }

                return temp;
            }
            catch(Exception ex)
            {
                return default(T);
            }
        }

        T InnerResolve<T>()
        {
            try
            {
                Type classType;
                //尝试读取T接口类型对应的实现类型
                _dictionary.TryGetValue(typeof(T), out classType);
                //获取第一个构造函数
                ConstructorInfo constructor = classType.GetConstructors().FirstOrDefault();

                //获取构造函数参数
                //ParameterInfo[] parameters = constructor.GetParameters();
                /*
                List<ParameterInfo> parameterInfoList = constructor.GetParameters().ToList();
                object[] parameters = new object[parameterInfoList.Count];
                for (int i = 0; i < parameterInfoList.Count; i++)
                {
                    parameters[i] = parameterInfoList[i];
                }
                //*/
                object o = constructor.Invoke(null);
                return (T)o;
            }
            catch
            {
                return default(T);
            }
        }

        //public dynamic Resolve(Type type)
        //{
        //    try
        //    {
        //        Type classType;
        //        //尝试读取T接口类型对应的实现类型
        //        _dictionary.TryGetValue(type, out classType);
        //        //获取第一个构造函数
        //        ConstructorInfo constructor = classType.GetConstructors().FirstOrDefault();

        //        //获取构造函数参数
        //        //ParameterInfo[] parameters = constructor.GetParameters();
        //        /*
        //        List<ParameterInfo> parameterInfoList = constructor.GetParameters().ToList();
        //        object[] parameters = new object[parameterInfoList.Count];
        //        for (int i = 0; i < parameterInfoList.Count; i++)
        //        {
        //            parameters[i] = parameterInfoList[i];
        //        }
        //        //*/
        //        object o = constructor.Invoke(null);
        //        return (dynamic)o;
        //    }
        //    catch
        //    {
        //        return default(dynamic);
        //    }
        //}

        public ILifetimeScope BeginLifetimeScope()
        {
            return new LifetimeScope(this);
        }

        LifetimeScopeType GetLifetimeScopeType(Type type)
        {
            return _typeDictionary[type];
        }
    }
}