using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxyTool.ProxyFindingParser
{
    public interface IParsingClass
    {
        Proxy[] Parse(string source, IWebPageDownloader downloader);
    }

    public interface IParsingFactory
    {
        IParsingClass CreateInstance(string host);
    }

    public class ParsingFactory : IParsingFactory
    {
        public IParsingClass CreateInstance(string host)
        {
            switch(host)
            {
                case "tools.rosinstrument.com":
                    return new rosinstrument();
            }

            return new NaiveParsingClass();
        }
    }
}
