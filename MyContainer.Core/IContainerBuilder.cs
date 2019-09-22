using System;
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
        IContainerBuilder Register<TClass, TInterface>() where TClass : class, TInterface;
        //将TClass类注册到当前容器实例中
        IContainerBuilder Register<TClass>() where TClass : class;
        //直接将type注册到当前容器实例中
        IContainerBuilder Register(Type type);
        //将TClass类以TInterface接口形式注入到容器
        //T Resolve<T>(params object[] parameters);

        //按每次生成新实例的方式注册
        void AsInstancePerDependency<T>();
        //按单例方式注册
        void AsSingleInstance<T>();
        //按生命周期方式注册
        void AsPerLifetimeScope<T>();
    }
}
