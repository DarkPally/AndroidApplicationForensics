using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAF.Library.ExtractRule;

namespace AAF.Library.Parser
{
    public class PackageParseResult
    {
        public string Name { get; set; }

        public string Desc { get; set; }

        public Dictionary<string, DataParseResultItem> KeyItems;

        public List<TableParseResultItem> TableItems
        {
            get
            {
                if(KeyItems!=null)
                {
                    return KeyItems.AsParallel().Where(c => c.Value is TableParseResultItem).Select(c => c.Value as TableParseResultItem).ToList();
                }
                return null;
            }
        }
        /*
        public List<ValueParseResultItem> ValueItems
        {
            get
            {
                if (KeyItems != null)
                {
                    return KeyItems.AsParallel().Where(c => c.Value is ValueParseResultItem).Select(c => c.Value as ValueParseResultItem).ToList();
                }
                return null;
            }
        }
        */
    }
}
