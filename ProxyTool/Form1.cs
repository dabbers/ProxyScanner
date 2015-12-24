using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading;
using ProxyTool.ProxyFindingParser;
using System.Net;

namespace ProxyTool
{
    public delegate void PerformCallback(ConcurrentBag<Proxy> list);

    public partial class Form1 : Form
    {
        Thread action;
                
        public Form1()
        {
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;

            var lines = richTextBox1.Lines;
            if (checkBox1.Checked)
            {
                action = new Thread(() => performVerify(donecb, performLinks(lines)));
            }
            else
            {
                action = new Thread(() => performLinks(donecb, lines));
            }

            action.Name = "Action";
            action.IsBackground = true;
            action.Start();
        }

        private void performVerify(PerformCallback cb, ConcurrentBag<Proxy> list)
        {
            IExecutingContext ctx = new ExecutingContext(new ParsingFactory());
            ConcurrentBag<Proxy> results = new ConcurrentBag<Proxy>();
            richTextBox2.Invoke(new Action(() =>
            {
                status_action.Text = "Proxy Verification";
                richTextBox2.Text = String.Empty;
                progressBar.Maximum = list.Count();
                progressBar.Value = 0;
            }));

            Parallel.ForEach(list, (item) =>
            {
                var task = new ProxyTesterTask();
                task.SetTask(item.ToString());
                task.Run(ctx);

                if (task.Result)
                {
                    richTextBox2.Invoke(new Action(() =>
                    {
                        richTextBox2.AppendText(item.ToString() + Environment.NewLine);
                    }));
                }
                richTextBox2.Invoke(new Action(() =>
                {
                    progressBar.Value++;
                }));

            });

            richTextBox2.Invoke(new Action(() =>
            {
                status_action.Text = "Idle.";
                button1.Enabled = true;
            }));

            //cb(results);
        }

        private void donecb(ConcurrentBag<Proxy> list)
        {
            richTextBox2.Invoke(new Action(() =>
            {
                richTextBox2.Text = String.Empty;

                foreach(var l in list)
                {
                    richTextBox2.AppendText(l.ToString() + Environment.NewLine);
                }
                status_action.Text = "Idle.";
                button1.Enabled = true;
            }));
        }

        public ConcurrentBag<Proxy> performLinks(string[] lines)
        {
            ConcurrentBag<Proxy> proxyList = new ConcurrentBag<Proxy>();

            richTextBox2.Invoke(new Action(() =>
            {
                status_action.Text = "Proxy fetching";
                richTextBox2.Text = String.Empty;
                progressBar.Maximum = lines.Count();
                progressBar.Value = 0;
            }));

            if (lines[0].StartsWith("http"))
            {


                IExecutingContext ctx = new ExecutingContext(new ParsingFactory());

                Parallel.ForEach(lines, (line) =>
                {
                    if (!String.IsNullOrEmpty(line))
                    {
                        ProxyFinderTask task = new ProxyFinderTask();

                        task.SetTask(line);
                        task.Run(ctx);
                        var results = task.Results;
                        foreach (var result in results)
                        {

                            if (proxyList.Where(p => p.Host == result.Host).Count() == 0)
                            {
                                IPAddress ip = Dns.GetHostAddresses(result.Host).FirstOrDefault();
                                result.Host = ip.ToString();

                                if (proxyList.Where(p => p.Host == result.Host).Count() == 0)
                                {
                                    proxyList.Add(result);
                                }
                            }
                        }

                        richTextBox2.Invoke(new Action(() =>
                        {
                            progressBar.Value++;
                        }));
                    }
                });

            }
            else
            {
                foreach(var line in lines)
                {
                    try
                    {
                        richTextBox2.Invoke(new Action(() =>
                        {
                            progressBar.Value++;
                        }));
                        var result = new Proxy(line);
                        if (proxyList.Where(p => p.Host == result.Host).Count() == 0)
                        {
                            proxyList.Add(result);
                        }
                    }
                    catch { }
                }
            }
            return proxyList;
        }

        public void performLinks(PerformCallback cb, string[] lines)
        {
            cb(performLinks(lines));
        }

    }


}
