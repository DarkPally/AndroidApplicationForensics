using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AAF.Library.ExtractRule;
using System.IO;

namespace AAF.Library.Parser
{
    public class ParseManager
    {
        //public string RootPath { get; set; }
        public RuleManager RuleManager { get; set; }

        const string adbDataPath_PC = "data\\data\\";
        public List<PackageParseResult> ParseData(string rootPath)
        {
            var res = new List<PackageParseResult>();
            if (rootPath.Last() != '\\') rootPath += '\\';
            foreach (var rp in RuleManager.Packages)
            {                
                try
                {
                    var pe = new PackageExtracter()
                    {
                        RulePackage = rp,
                        PackageDirectoryPath = rp.RootPath != null ? rootPath + rp.RootPath.Replace('/','\\') : rootPath + adbDataPath_PC + rp.Name
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

 
    }
}
