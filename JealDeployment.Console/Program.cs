using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JealDeployment.Console
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                System.Console.WriteLine("Running mode nees to be specified.");
                return;
            }

            switch (args[0].ToLower())
            {
                case "-server":
                {
                    var server = new Server();
                    server.Start();
                    System.Console.ReadKey();
                    break;
                }
                case "-client":
                {
                    var config = args[1];
                    var client = new Client(IPAddress.Parse("127.0.0.1"), 5858);
                    var deployResult = client.DeployToRemote(folder);
                    System.Console.WriteLine(deployResult);
                    break;
                }
                default:
                {
                    System.Console.WriteLine($"{args[0]} does not support.");
                    return;
                }
            }


            foreach (var arg in args)
            {
                System.Console.Write(arg);
            }
        }
    }
}