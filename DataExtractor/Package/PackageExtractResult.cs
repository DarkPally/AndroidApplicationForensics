using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RuleManager;

namespace DataExtractor
{
    public class PackageExtractResult
    {
        public string Name { get; set; }

        public string Desc { get; set; }

        public Dictionary<string, DataExtractResultItem> KeyItems;

        public List<TableExtractResultItem> TableItems
        {
            get
            {
                if(KeyItems!=null)
                {
                    return KeyItems.AsParallel().Where(c => c.Value is TableExtractResultItem).Select(c => c.Value as TableExtractResultItem).ToList();
                }
                return null;
            }
        }

        public List<ValueExtractResultItem> ValueTable
        {
            get
            {
                if (KeyItems != null)
                {
                    return KeyItems.AsParallel().Where(c => c.Value is ValueExtractResultItem).Select(c => c.Value as ValueExtractResultItem).ToList();
                }
                return null;
            }
        }
    }
}
