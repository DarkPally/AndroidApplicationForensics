using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AAF.Library.ExtractRule;
using System.IO;

namespace AAF.Library.Extracter
{
    public class ExtractManager
    {
        public RuleManager RuleManager { get; set; }

        const string adbDataPath = "data/data/";
        const string adbDataPath_PC = "data\\data\\";

        public void ExtractFromDirectory(string org_rootPath, string des_rootPath)
        {

        }

        public void ExtractFromADB(string des_rootPath)
        {
            FileExtracter feh = new ShellScriptFileExtracter();
            feh.InitConnection();
            var device = feh.Devices.First();
            var res = feh.ListDirecotry(device, adbDataPath);
            if (res.success)
            {
                foreach (var rp in RuleManager.Packages)
                {
                    if(rp.RootPath==null)
                    {
                        if (res.filesName.Contains(rp.Name))
                        {
                            foreach (var rule in rp.Items)
                            {
                                string des_path = des_rootPath+ adbDataPath_PC + rp.Name + "\\" + rule.RelativePath;
                                string org_path = adbDataPath + rp.Name + "/" + rule.RelativePath + "/" + rule.FileName;
                                feh.CopyFileFromDevice(device, org_path, des_path);
                            }
                        }
                    }      
                    else
                    {
                        string pcRootPath = rp.RootPath.Replace('/', '\\');
                        foreach (var rule in rp.Items)
                        {
                            if(rule.RelativePath==null|| rule.RelativePath=="")
                            {
                                string des_path = des_rootPath + pcRootPath;
                                string org_path = rp.RootPath + "/"  + rule.FileName;
                                feh.CopyFileFromDevice(device, org_path, des_path);
                            }
                            else
                            {
                                string des_path = des_rootPath + pcRootPath + "\\" + rule.RelativePath;
                                string org_path = rp.RootPath + "/" + rule.RelativePath + "/" + rule.FileName;
                                feh.CopyFileFromDevice(device, org_path, des_path);
                            }
                            
                        }
                    }
                }
            }
            else
            {
                throw new Exception(res.errorMessage);
            }
            
        }
    }
}
