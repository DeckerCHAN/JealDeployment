using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JealDeployment.Services
{
    class HelloWorldService : IHelloWorld
    {
        public string GetHelloWorld()
        {
            return "Hello World";
        }
    }
}
