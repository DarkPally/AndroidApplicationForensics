using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AAF.Library.ExtractRule;

namespace AAF.Library.Extractor
{
    class TestExtractor
    {
        static void TestMain(string[] args)
        {
            var x = new RuleManager();
            x.LoadRulePackages();

            string root = @"F:\杂物\开发破解工具\backup\apps\";

            var res = new List<PackageExtractResult>();
            foreach (var rp in x.Packages)
            {
                foreach(var rule in rp.Items)
                {
                    rule.RelativePath = "db";
                }
                var pe = new PackageExtractor()
                {
                    RulePackage = rp,
                    PackageDictionaryPath = root + rp.Name,
                };
                pe.Init();
                pe.DoWork();
                res.Add(pe.Result);
            }
            Console.ReadKey();
        }
    }
}
