using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SQLite;
using System.Data;


namespace AAF.Library.Extractor
{
    public class PackageExtractHelper
    {

        public static DataTable GetDbDataTable(SQLiteConnection connection, string tableName)
        {
            DataTable dataTable = null;

            DataSet ds = new DataSet();
            using (SQLiteDataAdapter da = new SQLiteDataAdapter(string.Format("select * from [{0}]", tableName)
                , connection))
            {
                da.Fill(ds);
            }
            dataTable = ds.Tables[0];
            dataTable.TableName = tableName;
            return dataTable;
        }

        
        public static DataTable GetDbDataTable(string dbFilePath, string tableName)
        {
            const string connectFormat = @"Data Source={0};Version=3;";

            using (var conn = new SQLiteConnection(String.Format(connectFormat, dbFilePath)))
            {
                conn.Open();
                return GetDbDataTable(conn, tableName);
            }
        }
    }
}
