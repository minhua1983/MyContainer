using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyContainer.Core.ServiceImplement;
using MyContainer.Core.ServiceInterface;

namespace MyContainer.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            //生成容器创建器
            ContainerBuilder containerBuilder = new ContainerBuilder();
            //注册所需类型
            containerBuilder.Register<Audit, IAudit>();
            //创建容器
            IContainer container = containerBuilder.Build<Container>();
            //调用
            IAudit audit = container.Resolve<IAudit>();
            if (audit != null)
            {
                audit.Load();
                audit.Save();
            }

            Console.ReadLine();
        }
    }
}
