using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JealDeployment.Entites;

namespace JealDeployment.Utils
{
    public static class FileUtils
    {
        public static string CreateNewTempFolder()
        {
            var temp = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(temp);
            return temp;
        }

        public static void BackupTo(string file, Backup backup)
        {
            var destionationFileName = Path.Combine(backup.DesinationFolder,
                Path.GetFileName(file) ?? throw new InvalidOperationException("File name can not be null or empty."));
            destionationFileName = DuplicateNaming(destionationFileName, backup.DuplicateNameingRule);
            File.Copy(file, destionationFileName);
        }

        private static string DuplicateNaming(string originName, DuplicateNameingRule rule)
        {
            if (!File.Exists(originName))
            {
                return originName;
            }

            switch (rule)
            {
                case DuplicateNameingRule.GuidAtEnd:
                    throw new NotImplementedException();
                    break;
                case DuplicateNameingRule.NumberAtEnd:
                    throw new NotImplementedException();
                    break;
                case DuplicateNameingRule.UppercaseLetterAtEnd:
                    return UpperCaseEndNaming(originName);
                case DuplicateNameingRule.LowercaseLetterAtEnd:
                    throw new NotImplementedException();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rule), rule, null);
            }
        }

        private static string UpperCaseEndNaming(string originFile)
        {
            if (!File.Exists(originFile))
            {
                return originFile;
            }

            var folder = Path.GetDirectoryName(originFile);
            var filename = Path.GetFileNameWithoutExtension(originFile);
            var extensionName = Path.GetExtension(filename);
            //TODO: Support better renaming
            for (var i = 65; i <= 90; i++)
            {
                var newName = Path.Combine(folder ?? "", string.Format($"{filename}{(char) i}.{extensionName}"));
                if (!File.Exists(newName))
                {
                    return newName;
                }
            }

            throw new Exception("Naming excede attempt range.");
        }

        public static HashSet<Tuple<string, string>> CompreTwoFolders(string folderPathA, string folderPathB)
        {
            var differs = new HashSet<Tuple<string, string>>();

            //Process files only
            var fileNamesA = new HashSet<string>(Directory.GetFiles(folderPathA).Select(Path.GetFileName));
            var fileNamesB = new HashSet<string>(Directory.GetFiles(folderPathB).Select(Path.GetFileName));

            //For files they all have
            foreach (var fileName in fileNamesA.Intersect(fileNamesB))
            {
                var fileFullPathA = Path.Combine(folderPathA, fileName);
                var fileFullPathB = Path.Combine(folderPathB, fileName);

                if (!CommonUtils.CalculateMd5(fileFullPathA).Equals(CommonUtils.CalculateMd5(fileFullPathB)))
                {
                    differs.Add(new Tuple<string, string>(fileFullPathA, fileFullPathB));
                }
            }

            //For new files
            foreach (var fileName in fileNamesA.Except(fileNamesB))
            {
                var fileFullPathA = Path.Combine(folderPathA, fileName);
                var fileFullPathB = Path.Combine(folderPathB, fileName);

                differs.Add(new Tuple<string, string>(fileFullPathA, fileFullPathB));
            }

            //For old files
            foreach (var fileName in fileNamesB.Except(fileNamesA))
            {
                var fileFullPathB = Path.Combine(folderPathB, fileName);

                differs.Add(new Tuple<string, string>(fileFullPathB, ""));
            }


            //For folders
            var folderNamesA = new HashSet<string>(Directory.GetDirectories(folderPathA).Select(Path.GetFileName));
            var folderNamesB = new HashSet<string>(Directory.GetDirectories(folderPathB).Select(Path.GetFileName));

            //For folder they all have
            foreach (var folderName in folderNamesA.Intersect(folderNamesB))
            {
                var folderFullPathA = Path.Combine(folderPathA, folderName);
                var folderFullPathB = Path.Combine(folderPathB, folderName);

                var result = CompreTwoFolders(folderFullPathA, folderFullPathB);
                differs.UnionWith(result);
            }

            //Create new folders
            foreach (var folderName in new HashSet<string>(folderNamesA.Except(folderNamesB)))
            {
                var folderFullPathA = Path.Combine(folderPathA, folderName);
                var folderFullPathB = Path.Combine(folderPathB, folderName);

                differs.Add(new Tuple<string, string>(folderFullPathA, folderFullPathB));
            }

            //Delete old folders
            foreach (var folderName in new HashSet<string>(folderNamesB.Except(folderNamesA)))
            {
                var folderFullPathA = Path.Combine(folderPathA, folderName);
                var folderFullPathB = Path.Combine(folderPathB, folderName);

                differs.Add(new Tuple<string, string>(folderFullPathB, ""));
            }

            return differs;
        }
    }
}