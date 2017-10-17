using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AAF.Library.ExtractRule
{
    class TestExtractRule
    {
        static void TestMain(string[] args)
        {
            var x = new RuleManager();
            x.LoadRulePackages();
            Console.ReadKey();
        }

        static void TestJsonShow(string[] args)
        {
            var tempEntity = new RuleItemInfo()
            {
                RelativePath = "databases",
                FileName = "database.db",
                DataPath = "t_history_record",
                SourceType = RuleItemInfo.DataSourceType.DataBase,
                Name = "Histroy",
                Desc = "观看历史记录(直播）",
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
                new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore });
            Console.WriteLine(json);
            Console.ReadKey();
        }
    }
}
