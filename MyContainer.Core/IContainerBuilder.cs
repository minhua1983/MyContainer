﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyContainer.Core
{
    public interface IContainerBuilder
    {
        //生成容器
        IContainer Build<T>() where T : IContainer;
        //将TClass类以TInterface接口形式注册到当前容器实例中
        void Register<TClass, TInterface>() where TClass : class, TInterface;
        //将TClass类注册到当前容器实例中
        void Register<TClass>() where TClass : class;
        //直接将type注册到当前容器实例中
        void Register(Type type);
        //将TClass类以TInterface接口形式注入到容器
        //T Resolve<T>(params object[] parameters);
    }
}