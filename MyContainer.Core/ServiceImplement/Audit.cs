using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyContainer.Core.ServiceInterface;

namespace MyContainer.Core.ServiceImplement
{
    public class Audit : IAudit
    {
        public void Load()
        {
            Console.WriteLine("load");
        }

        public void Save()
        {
            Console.WriteLine("save");
        }
    }
}
