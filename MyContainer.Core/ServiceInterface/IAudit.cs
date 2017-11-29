using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyContainer.Core.ServiceInterface
{
    public interface IAudit
    {
        void Load();
        void Save();
    }
}
