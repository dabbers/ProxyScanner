using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ProxyTool.ProxyFindingParser
{
    class rosinstrument : NaiveParsingClass
    {
        public rosinstrument()
        {

        }

        Regex textreg = new Regex(@"hideTxt\(\s*'(.*)'\);");
        Regex mathreg = new Regex(@"var x=Math.round\(Math.sqrt\((\d+)\)");
        Regex lastpagereg = new Regex(@"href='\?(\d+)' title='to last");


        public override Proxy[] Parse(string source, IWebPageDownloader wc)
        {
            string decoded = decodePage(source);
            var match = lastpagereg.Match(decoded);

            int maxPage = int.Parse(match.Groups[1].Value);

            List<Proxy> results = new List<Proxy>();

            results.AddRange(base.Parse(decoded, wc));

            // Iterate all pages for the proxies
            for (int i = 1; i <= maxPage; i++)
            {
                var src = decodePage(wc.Download("http://tools.rosinstrument.com/raw_free_db.htm?" + i));
                var resuls = base.Parse(src, wc);
                results.AddRange(resuls);
            }

            return results.ToArray();
        }


        private string decodePage(string source)
        {
            var code = textreg.Match(source);
            var math = mathreg.Match(source);

            //match.Captures
            var s = Uri.UnescapeDataString(code.Groups[1].Value);

            var x = (int)Math.Round(Math.Sqrt(int.Parse(math.Groups[1].Value)));
            var t = new StringBuilder();

            for (var i = 0; i < s.Length; i++) t.Append(Convert.ToChar((int)((int)s[i] ^ (int)(i % 2 != 0 ? x : 0))));

            return System.Net.WebUtility.HtmlDecode(t.ToString());
        }

    }
}
