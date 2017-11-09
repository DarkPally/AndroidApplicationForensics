using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AAF.Library.ExtractRule;
using AAF.Library.Parser;
using AAF.Library.Extracter.Android;
using System.IO;
using System.Text.RegularExpressions;
namespace AAF.Library.Extracter
{
    class TestExtractor
    {
        public static void TestMain(string[] args)
        {
            TestWifiParse();

            Console.ReadKey();
        }

        public static void TestWifiParse()
        {
            // 判断文件是否存在
            string filePath = @"C:\Users\zjf\Desktop\ww1\wifi\wpa_supplicant.conf";
            StreamReader sr = new StreamReader(filePath, System.Text.Encoding.Default);
            string temp = sr.ReadToEnd();
            sr.Close();


            string regexStr = "(?s)network={\n\t(ssid=\"(?<ssid>.*?)\"\n\t)" +
                "(psk=\"(?<password>.*?)\"\n\t)?" +
                "(key_mgmt=(?<key_mgmt>.*?)\n\t)" +
                "(priority=(?<priority>\\d+))" +
                "(.*?)" +
                "\n}";

            Regex r = new Regex(regexStr, RegexOptions.None);
            var mcs = r.Matches(temp);
            string dataStr = mcs[0].Groups["password"].Value;
            foreach (Match match in mcs)
            {
                string value = match.Groups["ssid"].Value;
                string value2 = match.Groups["password"].Value;
                string value3 = match.Groups["key_mgmt"].Value;
                string value4 = match.Groups["priority"].Value;
                string value5 = match.Groups["other"].Value;
            }
        }

        public static void T1()
        {
            var x = new RuleManager();
            x.LoadRulePackages();

            string root = @"F:\杂物\开发破解工具\backup\apps\";

            var res = new List<PackageParseResult>();
            foreach (var rp in x.Packages)
            {
                foreach (var rule in rp.Items)
                {
                    rule.RelativePath = "db";
                }
                var pe = new PackageExtracter()
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
