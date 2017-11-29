using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyContainer.Core
{
    public interface IContainer
    {
        //将TClass类以TInterface接口形式注册到当前容器实例中
        void Register<TClass, TInterface>() where TClass : class, TInterface;
        //将TClass类以TInterface接口形式注入到容器
        //T Resolve<T>(params object[] parameters);
        //以无参数构造获取T接口类型的实例
        T Resolve<T>();
    }
}
