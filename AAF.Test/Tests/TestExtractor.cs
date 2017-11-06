using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AAF.Library.ExtractRule;
using AAF.Library.Extractor.Android;

namespace AAF.Library.Extractor
{
    class TestExtractor
    {
        public static void TestMain(string[] args)
        {
            //var res=AdbHelper.CopyFromDevice(AdbHelper.GetSerialNo(), "/data/done.txt", @"C:\Users\zjf\Desktop\ww\1\11.txt");
            //Console.WriteLine(res);
            var res = AdbHelper.SearchFiles(AdbHelper.GetSerialNo(), "*", "data/data/");

            Console.WriteLine(res);

            Console.ReadKey();
        }

        public static void T1()
        {
            var x = new RuleManager();
            x.LoadRulePackages();

            string root = @"F:\杂物\开发破解工具\backup\apps\";

            var res = new List<PackageExtractResult>();
            foreach (var rp in x.Packages)
            {
                foreach (var rule in rp.Items)
                {
                    rule.RelativePath = "db";
                }
                var pe = new PackageExtractor()
                {
                    RulePackage = rp,
                    PackageDirectoryPath = root + rp.Name,
                };
                pe.Init();
                pe.DoWork();
                res.Add(pe.Result);
            }
        }
    }
}
