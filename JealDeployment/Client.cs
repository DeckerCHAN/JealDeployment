using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace JealDeployment
{
    public class Client
    {
        public Client()
        {
            this.Instance = new HelloWorldServiceClient(new WSHttpBinding(),
                new EndpointAddress("http://127.0.0.1:5858/HelloWorldService"));
        }

        private HelloWorldServiceClient Instance { get; set; }

        public string Fetch()
        {
            return this.Instance.GetHelloWorld();
        }
    }
}