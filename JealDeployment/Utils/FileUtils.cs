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
        public static void BackupTo(string file, Backup backup)
        {
        }

        private static string DuplicateNaming(string originName, DuplicateNameingRule rule)
        {
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
                var newName = Path.Combine(folder??"", string.Format($"{filename}{(char) i}.{extensionName}"));
                if (!File.Exists(newName))
                {
                    return newName;
                }
            }

            throw new Exception("Naming excede attempt range.");
        }
    }
}