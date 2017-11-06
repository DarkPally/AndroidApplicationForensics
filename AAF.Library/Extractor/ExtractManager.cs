using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AAF.Library.ExtractRule;
using AAF.Library.Extractor.Android;
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
                var pe = new PackageExtractor()
                {
                    RulePackage = rp,
                    PackageDirectoryPath = rootPath + rp.Name,
                };
                pe.Init();
                pe.DoWork();
                res.Add(pe.Result);
            }
            return res;
        }

        public void ExtractFileFromDirectory(string org_rootPath, string des_rootPath)
        {

        }

        public void ExtractFileFromADB(string org_rootPath, string des_rootPath)
        {
            foreach (var rp in RuleManager.Packages)
            {
                foreach(var rule in rp.Items)
                {
                    string des_dir = des_rootPath + rp.Name + "\\" + rule.RelativePath ;
                    string des_path = des_rootPath+rp.Name+"\\"+ rule.RelativePath + "\\" + rule.FileName;
                    string org_path = org_rootPath+rp.Name + "/"+rule.RelativePath + "/" + rule.FileName;
                    if(!Directory.Exists(des_dir))
                    {
                        Directory.CreateDirectory(des_dir);
                    }
                    AdbHelper.CopyFromDevice(AdbHelper.GetSerialNo(), org_path, des_path);
                }
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
