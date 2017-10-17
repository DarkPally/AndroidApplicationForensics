using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Data;

namespace AAF.Library.Searcher
{
    public class XmlSearchResultItem : DataSearchResultItem
    {
        public string ElementType { get; set; }
        public string ElementName { get; set; }
        public string ElementValue { get; set; }
        public XmlSearchResultItem()
        {
            sourceType = DataSourceType.Xml;
        }
    }
}
