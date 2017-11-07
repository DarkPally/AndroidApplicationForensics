using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace AAF.Library.Searcher
{
    public class DataSearchResult
    {
        public string RootPath { get; set; }

        public List<DataSearchResultItem> Items = new List<DataSearchResultItem>();

    }

    public class DataSearchResultItem
    {
        public enum DataSourceType
        {
            None,
            File,//普通文件
            FileName,//文件名
            Xml,
            DataBase,
        }

        protected  DataSourceType sourceType=DataSourceType.None;
        public DataSourceType SourceType { get { return sourceType; } set { sourceType = value; } }

        public string FileName { get; set; }

        //完整的文件相对路径
        public string RelativePath { get; set; }
        public string FullPath { get; set; } 
        //数据库:表名（正则）
        //xml: xpath(数据父节点或目标节点)
        //file: 正则表达式
        public string DataPath { get; set; }

        public object Data { get; set; }
    }

    
}
