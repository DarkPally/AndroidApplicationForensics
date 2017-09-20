using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SQLite;
using System.IO;
using System.Data;
namespace DataSearchTool
{
    public class DbSearcher:IDisposable
    {

        SQLiteConnection DbConnection = null;

        const string connectFormat = @"Data Source={0};Version=3;";

        const string selectTableFormat = "select * from {0}; ";
        const string selectWhereTableFormat = "select * from {0} where";
        const string whereFormat = " {0} like '%{1}%' ";
        
        DbEntity entity;

        public void Init(string dbPath)
        {
            DbConnection = new SQLiteConnection(String.Format(connectFormat, dbPath));
            DbConnection.Open();
            var tbs = DbHelper.getTableNames(DbConnection);
            var tables = tbs.AsParallel().Select(t => new DbTable() { Name = t, Fields = DbHelper.getFieldNames(DbConnection, t) });
            var tlists = tables.ToList();
            entity = new DbEntity() { Path = dbPath, Name = Path.GetFileName(dbPath), Tables = tables.ToList() };        

        }

        public DataSearchResult SearchStrInTableName(string keyStr)
        {
            string selectCmd = "";
            List<string> names=new List<string>();
            foreach (var t in entity.Tables)
            {
                if(t.Name.Contains(keyStr))
                {
                    selectCmd += String.Format(selectTableFormat, t.Name);
                    names.Add(t.Name);
                }
            }
            DataSet ds = new DataSet(); ;
            using (SQLiteDataAdapter da = new SQLiteDataAdapter(selectCmd, DbConnection))
            {
                da.Fill(ds);
            }

            var rt = new List<DataSearchResultItem>();
            for (int i = 0; i < ds.Tables.Count; ++i)
            {
                ds.Tables[i].TableName = names[i];

                rt.Add(new DbSearchResultItem()
                {
                    FileName = entity.Name,
                    ResultInTableName = true,
                    FullPath = entity.Path,
                    RelatingFields = null,
                    DataPath = names[i],
                    Table = ds.Tables[i]
                });
            }
            return new DataSearchResult()
                {
                    Items = rt,                    
                };
        }
        public DataSearchResult SearchStr(string keyStr)
        {
            string selectCmd="";
            foreach (var t in entity.Tables)
            {
                selectCmd += String.Format(selectWhereTableFormat, t.Name);
                for (int i = 0; i < t.Fields.Count;++i )
                {
                    selectCmd += String.Format(whereFormat, t.Fields[i], keyStr);
                    if(i==t.Fields.Count-1)
                    {
                        selectCmd += ";";
                    }
                    else
                    {
                        selectCmd += "or";
                    }
                }
            }


            DataSet ds = new DataSet(); ;
            using (SQLiteDataAdapter da = new SQLiteDataAdapter(selectCmd,DbConnection))
            {
                da.Fill(ds);
            }

            var rt = new List<DataSearchResultItem>();

            List<DataTable> ts = new List<DataTable>();
            for(int i=0;i<entity.Tables.Count;++i)
            {                
                if(ds.Tables[i].Rows.Count!=0)
                {
                    ds.Tables[i].TableName = entity.Tables[i].Name;
                    rt.Add(new DbSearchResultItem()
                    {
                        FileName = entity.Name,
                        ResultInTableName = false,
                        FullPath = entity.Path,
                        RelatingFields = getRelatingFields(ds.Tables[i],keyStr),
                        DataPath = ds.Tables[i].TableName,
                        Table = ds.Tables[i],                        
                    });
                }
            }
            return new DataSearchResult()
            {
                Items = rt,
            };
        }

        List<string> getRelatingFields(DataTable table, string keyStr)
        {
            var res=new List<string>();
            for (int i = 0; i < table.Columns.Count; ++i)
            {
                if (table.Rows[0][i].ToString().ToLower().Contains(keyStr.ToLower()))
                {
                    res.Add(table.Columns[i].ColumnName);
                }
            }
            return res;
        }

        public void Close()
        {
            if (DbConnection != null) DbConnection.Close();
        }

        #region Dispose方法
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                if (disposing)
                {
                    // Release managed resources
                    Close();
                }

                // Release unmanaged resources

                m_disposed = true;
            }
        }

        ~DbSearcher()
        {
            Dispose(false);
        }

        private bool m_disposed;
        #endregion
    }
}
