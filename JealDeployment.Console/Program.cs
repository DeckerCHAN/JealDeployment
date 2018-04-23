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
            var server = new Server();
            server.Start();
            System.Console.ReadKey();
        }
    }
}
