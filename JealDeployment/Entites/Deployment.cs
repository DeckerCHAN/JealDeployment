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
        public MemoryStream ZipFile { get; set; }
        public string Destination { get; set; }
        public ICollection<Deploy> Deploys { get; set; }
        public ICollection<Backup> DeployBackups { get; set; }
        public ICollection<Backup> ShapshotBackups { get; set; }
    }
}