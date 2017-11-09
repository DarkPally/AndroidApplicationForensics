using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AAF.Library.ExtractRule
{
    public class TestExtractRule
    {
        public static void TestMain(string[] args)
        {
            TestJsonShow();
        }
        static void TestLoad()
        {
            var x = new RuleManager();
            x.LoadRulePackages();
            Console.ReadKey();
        }
        static void TestJsonShow()
        {
            var tempEntity = new RuleItemInfo()
            {
                RelativePath = "databases",
                FileName = "database.db",
                DataPath = "(?s)network={\n\t(ssid=\"(?<ssid>.*?)\"\n\t)" +
                "(psk=\"(?<password>.*?)\"\n\t)?" +
                "(key_mgmt=(?<key_mgmt>.*?)\n\t)" +
                "(priority=(?<priority>\\d+))" +
                "(.*?)" +
                "\n}",
                SourceType = RuleItemInfo.DataSourceType.DataBase,
                Name = "Histroy",
                Desc = "观看历史记录(直播）",
                ExtraInfo = new RuleItemExtraInfo()
                {
                    IsHidden = true
                }
            };
            RulePackageInfo package = new RulePackageInfo()
            {
                Name = "android.zhibo8",
                Desc = "直播吧",
                Items = new List<RuleItemInfo>() { tempEntity }
            };
            var json = JsonConvert.SerializeObject(
                package,
                Formatting.Indented,
                new Newtonsoft.Json.JsonSerializerSettings {NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore });
            Console.WriteLine(json);
            Console.ReadKey();
        }
    }
}
