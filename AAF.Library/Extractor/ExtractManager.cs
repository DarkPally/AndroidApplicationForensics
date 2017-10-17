using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AAF.Library.ExtractRule;

namespace AAF.Library.Extractor
{
    public class ExtractManager
    {
        //public string RootPath { get; set; }
        public RuleManager RuleManager { get; set; }

        public List<PackageExtractResult> Extract(string rootPath)
        {
            var res = new List<PackageExtractResult>();
            if (rootPath.Last() != '\\') rootPath += '\\';
            foreach (var rp in RuleManager.Packages)
            {
                foreach (var rule in rp.Items)
                {
                    rule.RelativePath = "db";
                }
                var pe = new PackageExtractor()
                {
                    RulePackage = rp,
                    PackageDictionaryPath = rootPath + rp.Name,
                };
                pe.Init();
                pe.DoWork();
                res.Add(pe.Result);
            }
            return res;
        }
    }
}
