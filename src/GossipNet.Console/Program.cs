﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace GossipNet.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            if(Debugger.IsAttached)
            {
                // running inside VS
                args = new[] { "20000", "30000" };
            }

            if (args.Length == 0)
            {
                args = new[] { "30000" };
            }

            var port = int.Parse(args[0]);
            int? joinPort = null;
            if(args.Length == 2)
            {
                joinPort = int.Parse(args[1]);
            }

            var config = GossipNodeConfiguration.Create(x =>
            {
                x.LocalEndPoint = new IPEndPoint(IPAddress.Loopback, port);
                x.Logger = new LoggerConfiguration()
                    .MinimumLevel.Verbose()
                    .WriteTo.ColoredConsole()
                    .CreateLogger();
            });

            var node = new GossipNode(config);

            if(joinPort != null)
            {
                node.Join(new IPEndPoint(IPAddress.Loopback, joinPort.Value));
            }

            System.Console.ReadKey();
            node.Dispose();
        }
    }
}