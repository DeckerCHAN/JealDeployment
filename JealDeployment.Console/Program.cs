using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using JealDeployment.Entites;
using JealDeployment.Entites.Config;
using Newtonsoft.Json;

namespace JealDeployment.Console
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                System.Console.WriteLine("Running mode nees to be specified.");
                return;
            }

            switch (args[0].ToLower())
            {
                case "--server":
                {
                    var configPath = args[1];
                    var config = JsonConvert.DeserializeObject<ServerConfig>(File.ReadAllText(configPath));
                    var server = new Server(config);
                    server.Start();
                    System.Console.ReadKey();
                    break;
                }
                case "--client":
                {
                    var configPath = args[1];
                    var config = JsonConvert.DeserializeObject<ClientConfig>(File.ReadAllText(configPath));
                    var client = new Client(config);
                    string deployResult;

                    if (args.Length == 4 && args[3] == "--dry")
                    {
                        deployResult = client.DryDeployToRemote(args[2]);
                    }
                    else
                    {
                        deployResult = client.DeployToRemote(args[2]);
                    }

                    System.Console.WriteLine(deployResult);
                    break;
                }
                case "--config":
                {
                    var serverConfig = new ServerConfig
                    {
                        IpAddress = "127.0.0.1",
                        Port = "12345"
                    };
                    File.WriteAllText("DefaultServerConfig.json", JsonConvert.SerializeObject(serverConfig));

                    var clientConfig = new ClientConfig
                    {
                        IpAddress = "127.0.0.1",
                        Port = "12345",
                        ProjectName = "TestDeploymentProject",
                        DateFormat = "ddMMyyyy",
                        LocalBackups = new List<Backup>(new[]
                        {
                            new Backup()
                            {
                                DesinationFolder = "F:\\LocalBackup",
                                DuplicateNameingRule = DuplicateNameingRule.UppercaseLetterAtEnd
                            },
                        }),
                        DeployBackups = new List<Backup>(new[]
                        {
                            new Backup()
                            {
                                DesinationFolder = "F:\\DeployBackup",
                                DuplicateNameingRule = DuplicateNameingRule.UppercaseLetterAtEnd
                            },
                        }),
                        ShapshotBackups = new List<Backup>(new[]
                        {
                            new Backup()
                            {
                                DesinationFolder = "F:\\Shapshots",
                                DuplicateNameingRule = DuplicateNameingRule.UppercaseLetterAtEnd
                            },
                        }),
                        Deploys = new List<Deploy>(new Deploy[]
                        {
                            new Deploy
                            {
                                From = "Views",
                                To = "Views"
                            },
                        }),
                        Desination = "F:\\DEF",
                    };
                    File.WriteAllText("DefaultClientConfig.json", JsonConvert.SerializeObject(clientConfig,Formatting.Indented));


                    break;
                }
                default:
                {
                    System.Console.WriteLine($"{args[0]} does not support.");
                    return;
                }
            }
        }
    }
}