using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DataSearchTool
{
    public class FileSearchResult
    {
        public string RootPath { get; set; }
        public string RootDirectoryName { get; set; }

    }

    public class FileSearchResultSingle
    {
        public enum ResultType
        {
            None,
            Field,
            TableName,
            FileName
        }
        public ResultType Type { get; set; }

        public string FileName { get; set; }
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public DataTable Table { get; set; }

    }
    public class FileSearchResultAll : FileSearchResult
    {
        public List<FileSearchResultSingle> Results { get; set; }
    }

    public class FileSearchResultDB : FileSearchResult
    {

        public List<DbSearchResult> Results { get; set; }
    }
    
}
