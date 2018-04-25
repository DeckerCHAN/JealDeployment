using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using JealDeployment.Entites;

namespace JealDeployment.Services
{
    [ServiceContract(Name = "DeploymentService", Namespace = "http://www.jcs.com.au")]
    public interface IDeploy
    {
        [OperationContract]
        string Calibrate();

        [OperationContract]
        Consiquence Deploy(Deployment deployment);
    }
}