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
        List<string> allFilePaths;
        public void Init(string rootDirectoryPath)
        {
            RootDirectoryPath = rootDirectoryPath;

            DirectoryInfo theFolder = new DirectoryInfo(RootDirectoryPath);
            FileInfo[] thefileInfo = theFolder.GetFiles("*.*",SearchOption.AllDirectories);
            allFilePaths = thefileInfo.Select(c => c.FullName).ToList();

            dbFilePaths = FileHelper.GetAllSqlite3Paths(allFilePaths);
        }
        public FileSearchResultDB SearchStrInDB(string keyStr)
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

            return new FileSearchResultDB()
            {
                Results = t,
                RootDirectoryName = Path.GetDirectoryName(RootDirectoryPath),
                RootPath = RootDirectoryPath,
            };
        }
        public FileSearchResultDB SearchStrInDBTableName(string keyStr)
        {
            var t = new List<DbSearchResult>();
            Parallel.ForEach(dbFilePaths, it =>
            {
                using (var dbs = new DbSearcher())
                {
                    dbs.Init(it);
                    var res = dbs.SearchStrInTableName(keyStr);
                    if (res.Tables.Count != 0)
                    {
                        res.RelativePath = res.FullPath.Remove(0, RootDirectoryPath.Length);
                        t.Add(res);
                    }
                }
            });

            return new FileSearchResultDB()
            {
                Results = t,
                RootDirectoryName = Path.GetDirectoryName(RootDirectoryPath),
                RootPath = RootDirectoryPath,
            };
        }
        public List<FileSearchResultSingle> SearchStrInFileName(string keyStr)
        {
            var res = allFilePaths.Where(c => c.Contains(keyStr)).
                Select(c => new FileSearchResultSingle()
                {
                    FileName = c.Remove(0, RootDirectoryPath.Length),
                    Type = FileSearchResultSingle.ResultType.FileName
                }).ToList();
            return res;
        }
        public FileSearchResultAll SearchStrAll(string keyStr)
        {
            var temp = new List<DbSearchResult>();
            Parallel.ForEach(dbFilePaths, it =>
            {
                using (var dbs = new DbSearcher())
                {
                    dbs.Init(it);
                    var res1 = dbs.SearchStr(keyStr);
                    if (res1.Tables.Count != 0)
                    {
                        res1.RelativePath = res1.FullPath.Remove(0, RootDirectoryPath.Length);
                        temp.Add(res1);
                    }

                    var res2 = dbs.SearchStrInTableName(keyStr);
                    if (res2.Tables.Count != 0)
                    {
                        res2.RelativePath = res2.FullPath.Remove(0, RootDirectoryPath.Length);
                        temp.Add(res2);
                    }
                }
            });

            var res = SearchStrInFileName(keyStr);
            
            var rootDirectoryName = Path.GetDirectoryName(RootDirectoryPath);
            
            foreach (var it in temp)
            {
                foreach(var table in it.Tables)
                {

                    res.Add(new FileSearchResultSingle()
                        {
                            FileName = it.Name,
                            TableName = table.TableName,
                            Table = table,
                            FieldName = getFieldNameByKeyStr(table, keyStr),
                            Type = it.SearchByTableName?FileSearchResultSingle.ResultType.TableName: FileSearchResultSingle.ResultType.Field
                        });
                }
            }

            return new FileSearchResultAll()
                {
                    RootDirectoryName = rootDirectoryName,
                    RootPath = RootDirectoryPath,
                    Results = res
                };
        }
        string getFieldNameByKeyStr(DataTable table, string keyStr)
        {

            for (int i = 0; i < table.Columns.Count; ++i)
            {
                if (table.Rows[0][i].ToString().ToLower().Contains(keyStr.ToLower()))
                {
                    return table.Columns[i].ColumnName;
                }
            }
            return null;
        }
    }
}
