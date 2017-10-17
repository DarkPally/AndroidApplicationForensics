using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAF.Library.ExtractRule
{
    public class EI_ReferResult
    {
        public string SelfKeyField { get; set; } //自己是外键
        public string RefKeyField { get; set; }//对方是主键
        public string RefResultName { get; set; }//引用的结果名
        public List<string> RefFields { get; set; }//引用来的字段
    }
}
