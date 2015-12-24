using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProxyTool.ProxyFindingParser;

namespace ProxyTool
{
    public interface IExecutingContext
    {
        IParsingClass CreateParsingClass(string host);
        IWebPageDownloader CreateDownloader();
    }

    class ExecutingContext : IExecutingContext
    {
        IParsingFactory _parsingFactory;

        public ExecutingContext(IParsingFactory parsingFactory)
        {

            _parsingFactory = parsingFactory;
        }
        
        public IParsingClass CreateParsingClass(string host)
        {
            return this._parsingFactory.CreateInstance(host);
        }

        public IWebPageDownloader CreateDownloader()
        {
            return new WebPageDownloader();
        }
    }
}
