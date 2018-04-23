using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace JealDeployment.Services
{
    [ServiceContract(Name = "HelloWorldService", Namespace = "http://www.jcs.com.au")]
    public  interface IHelloWorld
    {
        
        [OperationContract()]
        string GetHelloWorld();
    }
}
