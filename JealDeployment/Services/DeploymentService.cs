using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JealDeployment.Entites;
using JealDeployment.Utils;

namespace JealDeployment.Services
{
    public class DeploymentService : IDeploy
    {
        public string Calibrate()
        {
            var file = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var md5 = CommonUtils.CalculateMd5(file);
            return md5;
        }

        public Consiquence Deploy(Deployment deployment)
        {
            return new Consiquence()
            {
                DeployLog = new StreamReader(deployment.ZipFile).ReadToEnd()
            };
        }
    }
}
