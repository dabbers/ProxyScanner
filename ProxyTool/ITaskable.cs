using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxyTool
{
    interface ITaskable
    {
        ITaskable Run(IExecutingContext ctx);

        ITaskable SetTask(string task);
    }
}
