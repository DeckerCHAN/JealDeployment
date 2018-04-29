using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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

            this.Proxy = new DeployServiceClientProxy(new WSHttpBinding(),
                new EndpointAddress(
                    $"http://{config.IpAddress}:{config.Port}/DeploymentService"));
            var file = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var md5 = CommonUtils.CalculateMd5(file);
            var serverMd5 = this.Proxy.Calibrate();

            if (!md5.Equals(serverMd5))
            {
                throw new Exception("Client side and server side library are not match.");
            }
        }

        private ClientConfig Config { get; set; }

        private DeployServiceClientProxy Proxy { get; set; }


        public string DryDeployToRemote(string publishDirectory)
        {
            //Check
            if (!Directory.Exists(publishDirectory))
            {
                throw new ArgumentException();
            }

            //Compose
            var tempFolder = FileUtils.CreateNewTempFolder();

            var zipFilePath = Path.Combine(tempFolder,
                $"{this.Config.ProjectName}{DateTime.Now.ToString(this.Config.DateFormat)}.zip");

            ZipFile.CreateFromDirectory(publishDirectory, zipFilePath);

            //Local backup
            foreach (var backup in this.Config.LocalBackups)
            {
                FileUtils.BackupTo(zipFilePath, backup);
            }

            //Parepare deployment package
            var ms = new MemoryStream();
            using (var fs = new FileStream(zipFilePath, FileMode.Open))
            {
                fs.CopyTo(ms);
            }

            ms.Position = 0L;

            //Do a deploy
            var deployment = new Deployment
            {
                DestinationFolderPath = this.Config.Desination,
                FileName = Path.GetFileName(zipFilePath),
                Hash = CommonUtils.CalculateMd5(zipFilePath),
                Payload = ms,
                DeployBackups = this.Config.DeployBackups,
                ShapshotBackups = this.Config.ShapshotBackups,
                Deploys = this.Config.Deploys
            };
            var res = this.Proxy.DryDeploy(deployment);

            //Collect deploy logs.
            return res.DeployLog.Aggregate((i, j) => i + Environment.NewLine + j);
        }
        public string DeployToRemote(string publishDirectory)
        {
            //Check
            if (!Directory.Exists(publishDirectory))
            {
                throw new ArgumentException();
            }

            //Compose
            var tempFolder = FileUtils.CreateNewTempFolder();

            var zipFilePath = Path.Combine(tempFolder,
                $"{this.Config.ProjectName}{DateTime.Now.ToString(this.Config.DateFormat)}.zip");

            ZipFile.CreateFromDirectory(publishDirectory, zipFilePath);

            //Local backup
            foreach (var backup in this.Config.LocalBackups)
            {
                FileUtils.BackupTo(zipFilePath, backup);
            }

            //Parepare deployment package
            var ms = new MemoryStream();
            using (var fs = new FileStream(zipFilePath, FileMode.Open))
            {
                fs.CopyTo(ms);
            }
            ms.Position = 0L;

            //Do a deploy
            var res = this.Proxy.Deploy(new Deployment()
            {
                DestinationFolderPath = this.Config.Desination,
                FileName = Path.GetFileName(zipFilePath),
                Hash = CommonUtils.CalculateMd5(zipFilePath),
                Payload = ms,
                DeployBackups = this.Config.DeployBackups,
                ShapshotBackups = this.Config.ShapshotBackups,
                Deploys = this.Config.Deploys
            });

            //Collect deploy logs.
            return res.DeployLog.Aggregate((i, j) => i + Environment.NewLine + j);
        }
    }
}