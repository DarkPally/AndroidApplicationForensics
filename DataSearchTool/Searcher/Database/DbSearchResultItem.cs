using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DataSearchTool
{
    public class DbSearchResultItem : DataSearchResultItem
    {
        public bool ResultInTableName{ get; set; }
        public List<string> RelatingFields { get; set; }
        public DataTable Table { get; set; }

        public DbSearchResultItem()
        {
            sourceType = DataSourceType.DataBase;
        }
    }
}
