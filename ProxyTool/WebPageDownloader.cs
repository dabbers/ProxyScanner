using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace ProxyTool
{
    public interface IWebPageDownloader
    {
        string Download(string url);
        string Download(string url, int timeout);
    }

    public class WebPageDownloader : IWebPageDownloader
    {

        private WebDownload _wc = new WebDownload();
        
        public string Download(string url)
        {
            return Download(url, 20);
        }
        public string Download(string url, int timeout)
        {
            _wc.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            _wc.Timeout = timeout * 1000;
            string result = String.Empty;
            try
            {
                result = _wc.DownloadString(url);
            }
            catch
            {
                
            }

            return result;
        }
    }

    public class WebDownload : WebClient
    {
        /// <summary>
        /// Time in milliseconds
        /// </summary>
        public int Timeout { get; set; }

        public WebDownload() : this(60000) { }

        public WebDownload(int timeout)
        {
            this.Timeout = timeout;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);
            if (request != null)
            {
                request.Timeout = this.Timeout;
            }
            return request;
        }
    }
}
