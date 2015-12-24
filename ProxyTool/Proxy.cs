using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxyTool
{
    public class Proxy
    {
        public string Host { get; set; }
        public int Port { get; set; }

        public Proxy(string host, int port)
        {
            Host = host;
            Port = port;
        }
        public Proxy(string hostport)
        {
            var parts = hostport.Split(new char[]{':'});
            Host = parts[0];
            int prt;
            int.TryParse(parts[1], out prt);
            Port = prt;
        }

        public override string ToString()
        {
            return Host + ":" + Port;
        }
    }
}
