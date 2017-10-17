using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using Newtonsoft.Json;

namespace AAF.Library.ExtractRule
{
    public class RuleManager
    {
        public string LibraryPath = "RuleLibrary";
        
        public List<RulePackageInfo> Packages { get; set; }

        public void LoadRulePackages()
        {
            Packages = new List<RulePackageInfo>();
            DirectoryInfo theFolder = new DirectoryInfo(LibraryPath);
            FileInfo[] thefileInfo = theFolder.GetFiles("*.*", SearchOption.TopDirectoryOnly);

            Parallel.ForEach(thefileInfo, f =>
             {
                 try
                 {
                     var t = File.ReadAllText(f.FullName,Encoding.Default);
                     var temp = JsonConvert.DeserializeObject<RulePackageInfo>(t);
                     lock(Packages)
                     {
                         Packages.Add(temp);
                     }
                 }
                 catch{ return;}
             });
        }
    }
}
