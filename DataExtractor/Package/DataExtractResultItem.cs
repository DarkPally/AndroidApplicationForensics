using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RuleManager;
using System.Data;
namespace DataExtractor
{
    public class DataExtractResultItem
    {
        public RuleItemInfo Rule;
    }

    public class TableExtractResultItem : DataExtractResultItem
    {
        public DataTable Table { get; set; }
    }

    public class ValueExtractResultItem : DataExtractResultItem
    {
        public string Value { get; set; }
    }
}
