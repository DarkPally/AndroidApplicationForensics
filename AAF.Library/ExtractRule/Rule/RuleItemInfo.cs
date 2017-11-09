using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAF.Library.ExtractRule
{
    public class RuleItemInfo
    {
        public enum DataSourceType
        {
            None,
            File,//普通文件，默认为正则匹配
            FileName,//文件名
            Xml,//仅是标准xml配置词典，其他xml走File处理
            DataBase,
        }
        //数据名称，一个规则只有一个名称
        //不在本数据库表内，而在完整规则内定义
        public string Name { get; set; }
        //数据描述，中文名称
        //不在本数据库表内，而在完整规则内定义
        public string Desc { get; set; }        
        //规则附加信息，为Json
        //不在本数据库表内，而在完整规则内定义
        public RuleItemExtraInfo ExtraInfo { get; set; }        
        //弱规则时舍弃
        public string RelativePath { get; set; }
        //文件名的正则表达式（默认） 或 组合表达式
        //正则全部由C#处理，并有RuleItemExtraInfo作标记，没有标记一律视为普通名称
        public string FileName { get; set; }
        //数据来源类型
        public DataSourceType SourceType { get; set; }
        //数据路径的正则表达式
        //FileName下同文件名正则
        //Xml为Key名称
        //数据库为Table正则名称
        //File为数据匹配的正则
        public string DataPath { get; set; }
    }
}
