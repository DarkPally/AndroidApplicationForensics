using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DataBaseSearchTool
{
    public class DbSearchResult
    {
        public string DbPath { get; set; }
        public string DbName { get; set; }
        public List<DataTable> Tables { get; set; }
    }
}
