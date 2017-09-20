using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;

namespace DataSearchTool
{
    public class DataSearcher
    {
        public string RootDirectoryPath { get; set; }

        List<string> dbFilePaths;
        List<string> standardXmlFilePaths;
        List<string> allFilePaths;

        bool hasPreparedXml = false;
        public List<XmlSearchResultItem> KeyValueItems=new List<XmlSearchResultItem>();
        public void Init(string rootDirectoryPath)
        {
            RootDirectoryPath = rootDirectoryPath;

            DirectoryInfo theFolder = new DirectoryInfo(RootDirectoryPath);
            FileInfo[] thefileInfo = theFolder.GetFiles("*.*", SearchOption.AllDirectories);
            
            allFilePaths = thefileInfo.Select(c => c.FullName).ToList();
            dbFilePaths = DataSearchHelper.GetSqlite3Paths(allFilePaths);
            standardXmlFilePaths = DataSearchHelper.GetStandardXmlPaths(allFilePaths);
            
        }
        DataSearchResult processResult(DataSearchResult res)
        {
            res.RootPath = RootDirectoryPath;
            Parallel.ForEach(res.Items, it =>
            {
                it.RelativePath = it.FullPath.Remove(0, RootDirectoryPath.Length);
            });
            return res;
        }
        public DataSearchResult SearchStrInDB(string keyStr)
        {
            DataSearchResult temp = new DataSearchResult();
            Parallel.ForEach(dbFilePaths, it =>
            {
                using (var dbs = new DbSearcher())
                {
                    dbs.Init(it);
                    temp.Items.AddRange(dbs.SearchStr(keyStr).Items);
                }
            });
            return processResult(temp);
        }
        public DataSearchResult SearchStrInDBTableName(string keyStr)
        {
            DataSearchResult temp = new DataSearchResult();
            Parallel.ForEach(dbFilePaths, it =>
            {
                using (var dbs = new DbSearcher())
                {
                    dbs.Init(it);
                    temp.Items.AddRange(dbs.SearchStr(keyStr).Items);
                }
            });
            return processResult(temp);
        }
        public DataSearchResult SearchStrInDBAll(string keyStr)
        {
            DataSearchResult temp = new DataSearchResult();
            Parallel.ForEach(dbFilePaths, it =>
            {
                using (var dbs = new DbSearcher())
                {
                    dbs.Init(it);
                    temp.Items.AddRange(dbs.SearchStr(keyStr).Items);
                    temp.Items.AddRange(dbs.SearchStrInTableName(keyStr).Items);
                }
            });
            return processResult(temp);
        }
        public DataSearchResult SearchStrInFileName(string keyStr)
        {
            var res = allFilePaths.Where(c => c.Contains(keyStr)).
                Select(c => new DataSearchResultItem()
                {
                    FileName = c.Remove(0, RootDirectoryPath.Length),
                    FullPath = c,
                    SourceType = DataSearchResultItem.DataSourceType.FileName,
                }).ToList();
            var temp = new DataSearchResult() { Items = res };
            return processResult(temp);;
        }
        public DataSearchResult SearchStrInXml(string keyStr)
        {
            if (!hasPreparedXml)
            {
                foreach(var it in standardXmlFilePaths)
                {
                    var dbs = new XmlSearcher();
                    dbs.Init(it);
                    if (dbs.KeyValueItems!=null)
                    KeyValueItems.AddRange(dbs.KeyValueItems);
                }
                hasPreparedXml = true;
            }
            var t=new DataSearchResult()
            {
                Items = KeyValueItems.AsParallel().Where(c =>c.ElementValue!=null&& c.ElementValue.Contains(keyStr)).Select(c => c as DataSearchResultItem).ToList()
            };
            return processResult(t);
        }
        public DataSearchResult SearchStr(string keyStr)
        {
            var res = SearchStrInDBAll(keyStr);
            res.Items.AddRange(SearchStrInFileName(keyStr).Items);
            return res;
        }

    }


    
}
