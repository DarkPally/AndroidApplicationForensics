using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.IO;
namespace DataBaseSearchTool
{
    public class FileSearcher
    {
        public string RootDirectoryPath { get; set; }

        List<string> dbFilePaths;

        public void Init(string rootDirectoryPath)
        {
            RootDirectoryPath = rootDirectoryPath;
            dbFilePaths = FileHelper.GetAllSqlite3Paths(rootDirectoryPath, true);
        }
        public FileSearchResult SearchStr(string keyStr)
        {
            var t=new List<DbSearchResult>();
            Parallel.ForEach( dbFilePaths,it=>
                {
                    using(var dbs=new DbSearcher())
                    {
                        dbs.Init(it);
                        var res=dbs.SearchStr(keyStr);
                        if(res.Tables.Count!=0)
                        {
                            res.RelativePath = res.FullPath.Remove(0, RootDirectoryPath.Length);
                            t.Add(res);
                        }
                    }
                });

            return new FileSearchResult()
            {
                Results = t,
                RootDirectoryName = Path.GetDirectoryName(RootDirectoryPath),
                RootPath = RootDirectoryPath,
            };
        }
    }
}
