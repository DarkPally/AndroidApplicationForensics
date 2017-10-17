using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAF.Library.ExtractRule;
using System.IO;

namespace AAF.Library.Extractor
{
    public class PackageExtractor
    {
        public string PackageDictionaryPath { get; set; } //搜索的根目录,是包名

        public RulePackageInfo RulePackage;
        public PackageExtractResult Result;

        List<string> allFilePaths;

        public void Init()
        {
            DirectoryInfo theFolder = new DirectoryInfo(PackageDictionaryPath);
            FileInfo[] thefileInfo = theFolder.GetFiles("*.*", SearchOption.AllDirectories);
            allFilePaths = thefileInfo.Select(c => c.FullName).ToList();
        }

        
        string getSingleFile(string fileName,string relativePath)
        {
            string path = PackageDictionaryPath+"\\"+relativePath + "\\" + fileName;
            if (allFilePaths.Contains(path)) return path;
            return null;
        }
        
        void handleRule(RuleItemInfo rule)
        {

            switch (rule.SourceType)
            {
                case RuleItemInfo.DataSourceType.None:
                    break;
                case RuleItemInfo.DataSourceType.File:
                    break;
                case RuleItemInfo.DataSourceType.FileName:
                    break;
                case RuleItemInfo.DataSourceType.Xml:
                    break;
                case RuleItemInfo.DataSourceType.DataBase:
                    handleRuleDatabase(rule);
                    break;
                default:
                    break;
            }
        }

        void handleRuleDatabase(RuleItemInfo rule)
        {
            var path=getSingleFile( rule.FileName, rule.RelativePath);
            if (path == null) return;
            try
            {
                var dt = PackageExtractHelper.GetDbDataTable(path, rule.DataPath);
                var item = new TableExtractResultItem()
                {
                    Rule = rule,
                    Table = dt,
                };
                Result.KeyItems.Add(rule.Name, item);
            }
            catch
            {

            }

        }

        public void DoWork()
        {
            if( RulePackage == null) return;
            Result = new PackageExtractResult()
            {
                Desc = RulePackage.Desc,
                Name = RulePackage.Name,
                KeyItems = new Dictionary<string, DataExtractResultItem>()
            };

            foreach(var rule in RulePackage.Items)
            {
                handleRule(rule);
            }
        }


    }
}
