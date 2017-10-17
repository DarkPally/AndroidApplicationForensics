using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
namespace AAF.Library.Searcher
{
    public class DbHelper
    {
        static public List<string> getTableNames(SQLiteConnection conn)
        {
            var res = new List<string>();
            var schemaTable = conn.GetSchema("TABLES");
            for(int i = 0 ; i < schemaTable.Rows.Count ; i++)     
            {
                res.Add(schemaTable.Rows[i]["TABLE_NAME"].ToString());
            }
            return res;

        }

        static public List<string> getFieldNames(SQLiteConnection conn,string tableName)
        {
            var res = new List<string>();
            var schemaTable = GetReaderSchema(conn, tableName);
            for (int i = 0; i < schemaTable.Rows.Count; i++)
            {
                res.Add(schemaTable.Rows[i][0].ToString());
            }
            return res;

        }

        static DataTable GetReaderSchema(SQLiteConnection connection,string tableName)
        {
            DataTable schemaTable = null;
            IDbCommand cmd = new SQLiteCommand();
            cmd.CommandText = string.Format("select * from [{0}]", tableName);
            cmd.Connection = connection;

            using (IDataReader reader = cmd.ExecuteReader(CommandBehavior.KeyInfo | CommandBehavior.SchemaOnly))
            {
                schemaTable = reader.GetSchemaTable();
            }
            return schemaTable;
        }


    }
}
