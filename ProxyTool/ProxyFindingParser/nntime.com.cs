using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ProxyTool.ProxyFindingParser
{
    class nntime : NaiveParsingClass
    {
        public nntime()
        {

        }


        Regex textreg = new Regex(@"<script type=""text/javascript"">\s*((\w)=(\d);)+\s*</script>");
        Regex proxyreg = new Regex(@"onclick=""choice\(\)"" \/></td><td>(.*)<script type=""text/javascript"">document.write\("":""+(.*?)\)<\/script><\/td>");


        public override Proxy[] Parse(string source, IWebPageDownloader wc)
        {
            List<Proxy> results = new List<Proxy>();
            results.AddRange(getProxies(source));

            // Iterate all pages for the proxies
            for (int i = 1; i <= 30; i++)
            {
                results.AddRange(getProxies(wc.Download("http://nntime.com/proxy-list-" + i.ToString("D2") + ".htm")));
            }

            return results.ToArray();
        }

        /// <summary>
        /// Converts the javascript portion of the port into a human readable number
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private Proxy[] getProxies(string source)
        {

            var collection = textreg.Match(source);
            Dictionary<string, string> variables = new Dictionary<string, string>();
            for (var i = 0; i < collection.Groups[2].Captures.Count; i++)
            {
                variables.Add(collection.Groups[2].Captures[i].Value, collection.Groups[3].Captures[i].Value);
            }
            var matches = proxyreg.Matches(source);

            List<Proxy> proxies = new List<Proxy>();

            foreach (Match match in matches)
            {
                var host = match.Groups[1].Value;
                var port = match.Groups[2].Value.Replace("+", "");

                foreach (var key in variables.Keys) port = port.Replace(key, variables[key]);

                proxies.Add(new Proxy(host, int.Parse(port)));
            }

            return proxies.ToArray();
        }

    }
}
