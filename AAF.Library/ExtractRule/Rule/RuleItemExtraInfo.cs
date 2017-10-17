using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAF.Library.ExtractRule
{
    public class RuleItemExtraInfo
    {
        public List<EI_ShowField> ShowFields { get; set; }
        //数据描述，中文名称
        //不在本数据库表内，而在完整规则内定义
        public EI_ReferResult ReferResult { get; set; }
        public EI_FileNameMatch FileNameMatch { get; set; }
    }
}
