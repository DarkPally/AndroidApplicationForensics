using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AAF.Library.ExtractRule;
using System.Data;
namespace AAF.Library.Parser
{
    public class DataParseResultItem
    {
        public RuleItemInfo Rule;
    }

    //事实上目前所有的返回值都是Table

    public class TableParseResultItem : DataParseResultItem
    {
        public DataTable Table { get; set; }
    }

    /*
    public class ValueParseResultItem : DataParseResultItem
    {
        public string Value { get; set; }
    }
    */
}
