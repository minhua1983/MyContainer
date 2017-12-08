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
            Container container = Container.GetInstance();
            container.Register<Audit, IAudit>();
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
