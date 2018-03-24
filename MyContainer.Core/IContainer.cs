using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace MyContainer.Core
{
    public interface IContainer
    {
        //以无参数构造获取T接口类型的实例
        T Resolve<T>();
        //用于动态获取对象实例
        dynamic Resolve(Type type);
    }
}
