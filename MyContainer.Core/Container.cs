﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Collections.Concurrent;

namespace MyContainer.Core
{
    public class Container : IContainer
    {
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
        public static Container GetInstance(ConcurrentDictionary<Type, Type> _dictionary)
        {
            if (_myContainer == null)
            {
                lock (_lockHelper)
                {
                    if (_myContainer == null)
                    {
                        _myContainer = new Container();
                        _myContainer._dictionary = _dictionary;
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
        public T Resolve<T>()
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

        public dynamic Resolve(Type type)
        {
            try
            {
                Type classType;
                //尝试读取T接口类型对应的实现类型
                _dictionary.TryGetValue(type, out classType);
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
                return (dynamic)o;
            }
            catch
            {
                return default(dynamic);
            }
        }
    }
}