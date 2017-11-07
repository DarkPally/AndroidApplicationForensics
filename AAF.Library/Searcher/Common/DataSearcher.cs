using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;

namespace AAF.Library.Searcher
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
        void prepareXml()
        {
            if (!hasPreparedXml)
            {
                foreach (var it in standardXmlFilePaths)
                {
                    var dbs = new XmlSearcher();
                    dbs.Init(it);
                    if (dbs.KeyValueItems != null)
                        KeyValueItems.AddRange(dbs.KeyValueItems);
                }
                hasPreparedXml = true;
            }
        }
        DataSearchResult processResult(DataSearchResult res)
        {
            res.RootPath = RootDirectoryPath;
            Parallel.ForEach(res.Items, it =>
            {
                if(it!=null && it.FullPath!=null)
                {
                    it.RelativePath = it.FullPath.Remove(0, RootDirectoryPath.Length);
                }
                
            });
            return res;
        }

        #region 按关键词搜索文件/数据
        public DataSearchResult SearchStrInDB(string keyStr)
        {
            DataSearchResult temp = new DataSearchResult();
            Parallel.ForEach(dbFilePaths, it =>
            {
                using (var dbs = new DbSearcher())
                {
                    dbs.Init(it);
                    var t = dbs.SearchStr(keyStr).Items;
                    lock(temp)
                    {
                        temp.Items.AddRange(t);
                    }
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
                    var t = dbs.SearchStr(keyStr).Items;
                    lock (temp)
                    {
                        temp.Items.AddRange(t);
                    }
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
                    var t1=dbs.SearchStr(keyStr).Items;
                    var t2 = dbs.SearchStrInTableName(keyStr).Items;
                    lock (temp)
                    {
                        temp.Items.AddRange(t1);
                        temp.Items.AddRange(t2);
                    }
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
            prepareXml();
            var t=new DataSearchResult()
            {
                Items = KeyValueItems.AsParallel().Where(c =>c.ElementValue!=null&& c.ElementValue.Contains(keyStr)).Select(c => c as DataSearchResultItem).ToList()
            };
            return processResult(t);
        }
        public DataSearchResult SearchStr(string keyStr)
        {
            var res = SearchStrInDB(keyStr);
            res.Items.AddRange(SearchStrInXml(keyStr).Items);
            return res;
        }
        public DataSearchResult SearchStrInPath(string keyStr)
        {
            var res = SearchStrInDBTableName(keyStr);
            res.Items.AddRange(SearchStrInFileName(keyStr).Items);
            return res;
        }
        #endregion

        #region 搜索包含中文的数据
        public DataSearchResult SearchChineseStrInDB()
        {
            return SearchStrInDB("[吖-做]");
        }
        #endregion

        #region 搜索全部数据
        public DataSearchResult SearchAll()
        {
            var res = new DataSearchResult();
            
            Parallel.ForEach(dbFilePaths, it =>
            {
                using (var dbs = new DbSearcher())
                {
                    dbs.Init(it);

                    var tt = dbs.SearchAll().Items.ToList();
                    lock(res)
                    {
                        res.Items.AddRange(tt);
                    }
                }
            });
            prepareXml();
            res.Items.AddRange(KeyValueItems.AsParallel().Select(c => c as DataSearchResultItem).ToList());
            

            return processResult(res);
        }
        #endregion

    }



}
