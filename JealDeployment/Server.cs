﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using JealDeployment.Services;

namespace JealDeployment
{
    public sealed class Server
    {
        public Server()
        {
            this.Host = new ServiceHost(typeof(DeploymentService));
        }

        private ServiceHost Host { get; set; }

        public void Start()
        {
            #region

            this.Host.AddServiceEndpoint(typeof(IDeploy), new WSHttpBinding(),
                "http://127.0.0.1:5858/DeploymentService");
            if (this.Host.Description.Behaviors.Find<ServiceMetadataBehavior>() == null)
            {
                var behavior = new ServiceMetadataBehavior
                {
                    HttpGetEnabled = true,
                    HttpGetUrl = new Uri("http://127.0.0.1:5858/DeploymentService/metadata")
                };
                this.Host.Description.Behaviors.Add(behavior);
            }

            #endregion

            this.Host.Opened += (sender, args) => Console.WriteLine("The service already opened.");

            this.Host.Open();
        }
    }
}