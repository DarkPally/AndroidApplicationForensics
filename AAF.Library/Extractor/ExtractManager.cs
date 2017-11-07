using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AAF.Library.ExtractRule;
using System.IO;

namespace AAF.Library.Extractor
{
    public class ExtractManager
    {
        //public string RootPath { get; set; }
        public RuleManager RuleManager { get; set; }

        const string adbDataPath = "data/data/";
        public List<PackageExtractResult> ExtractDataFromDirectory(string rootPath)
        {
            var res = new List<PackageExtractResult>();
            if (rootPath.Last() != '\\') rootPath += '\\';
            foreach (var rp in RuleManager.Packages)
            {                
                try
                {
                    var pe = new PackageExtractor()
                    {
                        RulePackage = rp,
                        PackageDirectoryPath = rootPath + rp.Name,
                    };

                    pe.Init();
                    pe.DoWork();
                    res.Add(pe.Result);
                }
                catch
                {

                }
            }
            return res;
        }

        public void ExtractFileFromDirectory(string org_rootPath, string des_rootPath)
        {

        }

        public void ExtractFileFromADB(string org_rootPath, string des_rootPath)
        {
            FileExtracter feh = new ShellScriptFileExtracter();
            feh.InitConnection();
            var device = feh.Devices.First();
            var res = feh.ListDirecotry(device, org_rootPath);
            if (res.success)
            {
                foreach (var rp in RuleManager.Packages)
                {
                    if (res.filesName.Contains(rp.Name))
                    {
                        foreach (var rule in rp.Items)
                        {
                            string des_path = des_rootPath + rp.Name + "\\" + rule.RelativePath;
                            string org_path = org_rootPath + rp.Name + "/" + rule.RelativePath + "/" + rule.FileName;
                            feh.CopyFileFromDevice(device, org_path, des_path);
                        }
                    }                       
                }
            }
            else
            {
                throw new Exception(res.errorMessage);
            }
            
        }

        public List<PackageExtractResult> ExtractDataFromADB()
        {
            string path = System.Environment.CurrentDirectory + "\\temp\\";
            ExtractFileFromADB(adbDataPath, path);
            return ExtractDataFromDirectory(path);
        }
    }
}
