using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxyTool
{
    class ProxyTesterTask : ITaskable
    {
        string _task = String.Empty;

        public ITaskable Run(IExecutingContext ctx)
        {
            IWebPageDownloader wc = ctx.CreateDownloader();

            var result = wc.Download("http://dab.biz/verify.php?proxy=" + _task, 12);
            this.Result = (result == "true");
            
            return this;
        }

        public ITaskable SetTask(string task)
        {
            _task = task;
            return this;
        }

        public bool Result { get; private set; }
    }
}
