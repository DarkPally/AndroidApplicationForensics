using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAF.Library.ExtractRule
{
    public class RulePackageInfo
    {
        //包名，com.xx.xx
        public string Name { get; set; }

        //包名描述，中文名称
        public string Desc { get; set; }

        //弱规则时舍弃,规则下统一的根目录，统一按/标记
        //缺省为data/data/ + Name
        //没有正则！
        public string RootPath { get; set; }

        public List<RuleItemInfo> Items { get; set; }
    }
}
