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
            containerBuilder.Register<Audit, IAudit>().AsPerLifetimeScope<IAudit>();
            //创建容器
            IContainer container = containerBuilder.Build<Container>();
            //调用
            using (ILifetimeScope lifetimeScope = container.BeginLifetimeScope())
            {
                IAudit audit = lifetimeScope.Resolve<IAudit>(lifetimeScope);
                if (audit != null)
                {
                    audit.Load();
                    audit.Save();
                }

                IAudit audit1 = lifetimeScope.Resolve<IAudit>(lifetimeScope);
                if (audit1 != null)
                {
                    audit1.Load();
                    audit1.Save();
                }
            }

            Console.ReadLine();
        }
    }
}
