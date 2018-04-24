using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JealDeployment.Console
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 1)
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
                    var client = new Client();
                    var content = client.Fetch();
                    System.Console.WriteLine(content);
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