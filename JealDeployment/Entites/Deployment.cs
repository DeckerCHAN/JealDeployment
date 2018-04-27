using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JealDeployment.Entites
{
    public class Deployment
    {
        public string Hash { get; set; }
        public MemoryStream Payload { get; set; }
        public string DestinationFolderPath { get; set; }
        public string FileName { get; set; }
        public ICollection<Deploy> Deploys { get; set; }
        public ICollection<Backup> DeployBackups { get; set; }
        public ICollection<Backup> ShapshotBackups { get; set; }
    }
}