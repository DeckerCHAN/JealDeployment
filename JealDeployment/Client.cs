using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using JealDeployment.Entites;
using JealDeployment.Entites.Config;
using JealDeployment.Proxys;
using JealDeployment.Utils;

namespace JealDeployment
{
    public sealed class Client
    {
        public Client(ClientConfig config)
        {
            this.Config = config;
            this.DefaultConfig = DefaultConfigs.GetDefaultClientConfig();

            this.Proxy = new DeployServiceClientProxy(new WSHttpBinding(),
                new EndpointAddress(
                    $"http://{config.IpAddress ?? this.DefaultConfig.IpAddress}:{config.Port ?? this.DefaultConfig.Port}/DeploymentService"));
            var file = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var md5 = CommonUtils.CalculateMd5(file);
            var serverMd5 = this.Proxy.Calibrate();

            if (!md5.Equals(serverMd5))
            {
                throw new Exception("Client side and server side library are not match.");
            }
        }

        private ClientConfig Config { get; set; }

        private ClientConfig DefaultConfig { get; set; }

        private DeployServiceClientProxy Proxy { get; set; }
                  
        public string TryDeploy(string file)
        {
            var ms = new MemoryStream();
            using (var fs = new FileStream(file, FileMode.Open))
            {
                fs.CopyTo(ms);
            }

            var res = this.Proxy.Deploy(new Deployment()
            {
                Destination = "D:\\",
                Hash = CommonUtils.CalculateMd5(file),
                ZipFile = ms
            });
            return res.DeployLog;
        }
    }
}