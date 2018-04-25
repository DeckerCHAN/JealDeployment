using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JealDeployment.Entites.Config
{
    public class ClientConfig
    {
        public string IpAddress { get; set; }
        public string Port { get; set; }
        public string ProjectName { get; set; }
        public string DateFormat { get; set; }
        public ICollection<Backup> LocalBackups { get; set; }
        public ICollection<Backup> DeployBackups { get; set; }
        public ICollection<Backup> ShapshotBackups { get; set; }

    }
}