using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DataBaseSearchTool
{
    public class FileSearchResult
    {
        public string RootPath { get; set; }
        public string RootDirectoryName { get; set; }
        public List<DbSearchResult> Results { get; set; }
    }
}
