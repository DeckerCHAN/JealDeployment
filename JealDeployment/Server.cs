using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using JealDeployment.Entites.Config;
using JealDeployment.Services;

namespace JealDeployment
{
    public sealed class Server
    {
        public Server(ServerConfig serverConfig)
        {
            this.Config = serverConfig;
            this.Host = new ServiceHost(typeof(DeploymentService));
        }

        private ServerConfig Config { get; set; }

        private ServiceHost Host { get; set; }

        public void Start()
        {
            #region

            this.Host.AddServiceEndpoint(typeof(IDeploy), new WSHttpBinding(),
                $"http://{this.Config.IpAddress}:{this.Config.Port}/DeploymentService");
            if (this.Host.Description.Behaviors.Find<ServiceMetadataBehavior>() == null)
            {
                var behavior = new ServiceMetadataBehavior
                {
                    HttpGetEnabled = true,
                    HttpGetUrl =
                        new Uri($"http://{this.Config.IpAddress}:{this.Config.Port}/DeploymentService/metadata")
                };
                this.Host.Description.Behaviors.Add(behavior);
            }

            #endregion

            this.Host.Opened += (sender, args) => Console.WriteLine("The service already opened.");

            this.Host.Open();
        }
    }
}