using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;

namespace DataBaseSearchTool
{
    class Program
    {
        static void Main(string[] args)
        {
            Test();
            Console.ReadLine();
        }

        static void Test()
        {
            using(DbSearcher DbSearcher = new DbSearcher())
            {
                DbSearcher.Init(@"C:\Users\zjf\Desktop\新建文件夹\2074532171filelist.db");
                var res=DbSearcher.SearchStr("0");

                foreach(var t in res.Tables)
                {
                    Console.WriteLine(t.TableName);
                    foreach (DataColumn dc in t.Columns)
                    {
                        Console.Write(dc.ColumnName+" ");
                    }
                    Console.WriteLine("");
                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        for (int j = 0; j < t.Columns.Count; j++)
                        {
                            Console.Write(t.Rows[i][j].ToString()+" ");
                        }
                        Console.WriteLine("");
                    }
                }
            };
    
            
        }
        static void BaseOperate()
        {
            using(var sqlite=new SQLiteConnection(@"Data Source=C:\Users\zjf\Desktop\新建文件夹\2074532171filelist.db;Version=3;"))
            {
                sqlite.Open();
                string sql = "select * from cachefilelist";
                SQLiteCommand command = new SQLiteCommand(sql, sqlite);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                    Console.WriteLine("ServerPath: " + reader["server_path"] );
            }
        }

        static string selectFormat = "select * from {0} where {1} like '%{2}%'";
        static void BatchSearch()
        {
            using(var sqlite=new SQLiteConnection(@"Data Source=C:\Users\zjf\Desktop\新建文件夹\2074532171filelist.db;Version=3;"))
            {
                sqlite.Open();
                string sql = String.Format(selectFormat, "cachefilelist", "server_path", "自");
                sql += " ;";

                sql += String.Format(selectFormat, "cachefilelist", "server_path", "少");
                SQLiteCommand command = new SQLiteCommand(sql, sqlite);
                using(SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        Console.WriteLine("ServerPath: " + reader["server_path"]); 
                    reader.NextResult();
                    while (reader.Read())
                        Console.WriteLine("ServerPath: " + reader["server_path"]); 
                }
                
            }
        }
    }
}
