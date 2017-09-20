using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DataSearchTool
{
    public class DbSearchResult
    {        
        public string FullPath { get; set; }

        public string RelativePath { get; set; }
        public string Name { get; set; }

        public bool SearchByTableName{ get; set; }
        public List<DataTable> Tables { get; set; }
    }
}
