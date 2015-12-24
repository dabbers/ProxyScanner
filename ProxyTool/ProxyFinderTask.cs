using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace ProxyTool
{
    class ProxyFinderTask : ITaskable
    {
        string _task = String.Empty;

        Proxy[] _results;

        public ITaskable SetTask(string task)
        {
            _task = task;
            return this;
        }

        public ITaskable Run(IExecutingContext ctx)
        {
            IWebPageDownloader wc = ctx.CreateDownloader();

            var result = wc.Download(_task);
            var uri = new Uri(_task);
            _results = ctx.CreateParsingClass(uri.Host).Parse(result, wc);

            return this;
        }

        public Proxy[] Results
        {
            get
            {
                return _results;
            }
        }
    }
}
