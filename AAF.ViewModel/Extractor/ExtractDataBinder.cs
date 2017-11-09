using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using AAF.Library.Parser;

namespace AAF.ViewModel
{
    public class ExtractDataBinder
    {
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public List<ExtractDataBinder> Children { get; set; }
        public DataTable DataSource { get; set; }
    }
}
