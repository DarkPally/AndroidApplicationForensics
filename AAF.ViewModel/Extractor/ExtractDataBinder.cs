using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using AAF.Library.Parser;

namespace AAF.ViewModel
{
    public class ExtractDataBinder
    {
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public List<ExtractDataBinder> Children { get; set; }
        public DataTable DataSource { get; set; }

        /*
        public static List<ExtractDataBinder> GetChildren(PackageParseResult r)
        {
            var t = r.TableItems.Select(cc =>
                                       new ExtractDataBinder()
                                       {
                                           Name = cc.Rule.Name,
                                           DisplayName = cc.Rule.Desc,
                                           DataSource = cc.Table
                                       }).ToList();
            if(r.ValueItems.Count>0)
            {

                var tTable = new DataTable("Params");
                tTable.Columns.AddRange(
                    new DataColumn[] 
                    {
                        new DataColumn("名称", Type.GetType("System.String")),
                        new DataColumn("值", Type.GetType("System.String")),
                    });
                foreach(var it in r.ValueItems)
                {
                    DataRow dr = tTable.NewRow();
                    dr["名称"] = it.Rule.Desc;
                    dr["point"] = 10;
                    dr["number"] = 1;
                    dr["totalpoint"] = 10;
                    dr["prizeid"] = "001";
                    dt.Rows.Add(dr);
                }

                var tchild = new ExtractDataBinder()
                {
                    Name = "Params",
                    DisplayName = "参数信息",
                    DataSource =
                };

            }
            return t;
        }
        */
    }
}
