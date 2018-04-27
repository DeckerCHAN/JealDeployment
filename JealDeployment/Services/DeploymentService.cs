using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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
            var deployLog = new List<string>();

            try
            {
                //Create temp folder
                var tempFolder = FileUtils.CreateNewTempFolder();
                deployLog.Add($"Temp Folder {tempFolder} created.");

                //Generate zip file path
                var zipFilePath = Path.Combine(tempFolder, deployment.FileName);

                //Read bytes from payload
                using (var fs = new FileStream(zipFilePath, FileMode.CreateNew))
                {
                    deployment.Payload.CopyTo(fs);
                }

                deployLog.Add($"File wrote into temp folder.");


                //Calculate Md5 for file received. Conduct an intergation check.
                var receivedMd5 = CommonUtils.CalculateMd5(zipFilePath);
                if (!receivedMd5.Equals(deployment.Hash))
                {
                    throw new Exception("File intergation check failed.");
                }

                deployLog.Add($"Both file sent and received has md5 {receivedMd5}. Intergation check passed.");

                //Do deploy backup
                foreach (var backup in deployment.DeployBackups)
                {
                    FileUtils.BackupTo(zipFilePath, backup);
                }

                //Create folder for unzip

                var unzipFolderPath = Path.Combine(tempFolder, Path.GetFileNameWithoutExtension(deployment.FileName));

                deployLog.Add($"Folder for unzip {unzipFolderPath} created.");

                //Unzip file
                ZipFile.ExtractToDirectory(zipFilePath, unzipFolderPath);
                deployLog.Add($"File unziped to {unzipFolderPath}");

                //Do a snapshot backup if we have
                if (deployment.ShapshotBackups != null && deployment.ShapshotBackups.Count > 0)
                {
                    var snapZipFilePath = Path.Combine(FileUtils.CreateNewTempFolder(), deployment.FileName);
                    ZipFile.CreateFromDirectory(deployment.DestinationFolderPath, snapZipFilePath);
                    foreach (var backup in deployment.ShapshotBackups)
                    {
                        FileUtils.BackupTo(snapZipFilePath, backup);
                    }
                }

                //Do actuall deployment
                deployLog.Add($"Deploies:");
                foreach (var deploy in deployment.Deploys)
                {
                    var fromFolder = Path.Combine(unzipFolderPath, deploy.From);
                    var toFolder = Path.Combine(deployment.DestinationFolderPath, deploy.To);
                    var differences = FileUtils.CompreTwoFolders(fromFolder, toFolder);
                    foreach (var difference in differences)
                    {
                        deployLog.Add($"{difference.Item1} --> {difference.Item2}");
                        FileAttributes attr;
                        //Delete only
                        if (string.IsNullOrEmpty(difference.Item2))
                        {
                            attr = File.GetAttributes(difference.Item1);
                            if (attr.HasFlag(FileAttributes.Directory))
                            {
                                Directory.Delete(difference.Item1, true);
                            }
                            else
                            {
                                File.Delete(difference.Item1);
                            }
                            continue;
                        }

                        //Copy or create new
                        attr = File.GetAttributes(difference.Item1);
                        if (attr.HasFlag(FileAttributes.Directory))
                        {
                            //Now Create all of the directories
                            foreach (string dirPath in Directory.GetDirectories(difference.Item1, "*",
                                SearchOption.AllDirectories))
                                Directory.CreateDirectory(dirPath.Replace(difference.Item1, difference.Item2));

                            //Copy all the files & Replaces any files with the same name
                            foreach (string newPath in Directory.GetFiles(difference.Item1, "*.*",
                                SearchOption.AllDirectories))
                                File.Copy(newPath, newPath.Replace(difference.Item1, difference.Item2), true);
                        }
                        else
                        {
                            File.Copy(difference.Item1,difference.Item2);
                        }



                    }
                }
            }
            catch (Exception e)
            {
                deployLog.Add($"Due to an exception, deployment stopped. Details{e.Message}");
            }

            return new Consiquence()
            {
                DeployLog = deployLog
            };
        }

        public Consiquence DryDeploy(Deployment deployment)
        {
            var deployLog = new List<string>();

            try
            {
                //Create temp folder
                var tempFolder = FileUtils.CreateNewTempFolder();
                deployLog.Add($"Temp Folder {tempFolder} created.");

                //Generate zip file path
                var zipFilePath = Path.Combine(tempFolder, deployment.FileName);

                //Read bytes from payload
                using (var fs = new FileStream(zipFilePath, FileMode.CreateNew))
                {
                    deployment.Payload.CopyTo(fs);
                }

                deployLog.Add($"File wrote into temp folder.");


                //Calculate Md5 for file received. Conduct an intergation check.
                var receivedMd5 = CommonUtils.CalculateMd5(zipFilePath);
                if (!receivedMd5.Equals(deployment.Hash))
                {
                    throw new Exception("File intergation check failed.");
                }

                deployLog.Add($"Both file sent and received has md5 {receivedMd5}. Intergation check passed.");

                //Do deploy backup
                foreach (var backup in deployment.DeployBackups)
                {
                    FileUtils.BackupTo(zipFilePath, backup);
                    deployLog.Add($"Will backup {zipFilePath} to {backup.DesinationFolder} with {backup.DuplicateNameingRule.ToString()} mode.");
                }

                //Create folder for unzip

                var unzipFolderPath = Path.Combine(tempFolder, Path.GetFileNameWithoutExtension(deployment.FileName));

                deployLog.Add($"Folder for unzip {unzipFolderPath} created.");

                //Unzip file
                ZipFile.ExtractToDirectory(zipFilePath, unzipFolderPath);
                deployLog.Add($"File unziped to {unzipFolderPath}");

                //Do a snapshot backup if we have
                if (deployment.ShapshotBackups != null && deployment.ShapshotBackups.Count > 0)
                {
                    var snapZipFilePath = Path.Combine(FileUtils.CreateNewTempFolder(), deployment.FileName);
                    ZipFile.CreateFromDirectory(deployment.DestinationFolderPath, snapZipFilePath);
                    foreach (var backup in deployment.ShapshotBackups)
                    {
                        FileUtils.BackupTo(snapZipFilePath, backup);
                        deployLog.Add($"Will have snapshot backup {snapZipFilePath} to {backup.DesinationFolder} with {backup.DuplicateNameingRule.ToString()} mode.");

                    }
                }

                //Do actuall deployment
                deployLog.Add($"Deploys:");
                foreach (var deploy in deployment.Deploys)
                {
                    var fromFolder = Path.Combine(unzipFolderPath, deploy.From);
                    var toFolder = Path.Combine(deployment.DestinationFolderPath, deploy.To);
                    var differences = FileUtils.CompreTwoFolders(fromFolder, toFolder);
                    foreach (var difference in differences)
                    {
                        deployLog.Add($"{difference.Item1} --> {difference.Item2}");
                    }
                }
            }
            catch (Exception e)
            {
                deployLog.Add($"Due to an exception, dry deployment stopped. Details: {e.Message}");
            }

            return new Consiquence()
            {
                DeployLog = deployLog
            };
        }
    }
}