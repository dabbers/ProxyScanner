using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ProxyTool.ProxyFindingParser
{
    public class NaiveParsingClass : IParsingClass
    {
        public NaiveParsingClass()
        {

        }
        Regex proxyrgx = new Regex(@"([\w\.\-]{5,}:\d+)");

        public virtual Proxy[] Parse(string source, IWebPageDownloader wc)
        {
            var results = proxyrgx.Matches(source);
            List<Proxy> proxies = new List<Proxy>();

            foreach(var result in results)
            {
                string tmp = result.ToString();
                Proxy prox = new Proxy(tmp);
                
                if (proxies.Where(p => p.Host == prox.Host).Count() == 0)
                {
                    proxies.Add(prox);
                }
            }

            return proxies.ToArray();
        }
    }
}
