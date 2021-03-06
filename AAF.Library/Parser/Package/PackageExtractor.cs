﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAF.Library.ExtractRule;
using System.IO;

namespace AAF.Library.Parser
{
    public class PackageExtracter
    {
        public string PackageDirectoryPath { get; set; } //搜索的根目录,是包名

        public RulePackageInfo RulePackage;
        public PackageParseResult Result;

        List<string> allFilePaths;

        public void Init()
        {
            DirectoryInfo theFolder = new DirectoryInfo(PackageDirectoryPath);
            FileInfo[] thefileInfo = theFolder.GetFiles("*.*", SearchOption.AllDirectories);
            allFilePaths = thefileInfo.Select(c => c.FullName).ToList();
        }

        
        string getSingleFile(string fileName,string relativePath)
        {
            string path;
            if (relativePath==null || relativePath == "")
            {
                path= PackageDirectoryPath + "\\" + fileName;
            }
            else
            {
                path = PackageDirectoryPath + "\\" + relativePath + "\\" + fileName;
            }
            
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
                    handleRuleFile(rule);
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
        void handleRuleFile(RuleItemInfo rule)
        {
            var path = getSingleFile(rule.FileName, rule.RelativePath);
            if (path == null) return;
            try
            {
                var dt = PackageParseHelper.GetFileDataTable(path, rule.DataPath);
                var item = new TableParseResultItem()
                {
                    Rule = rule,
                    Table = dt,
                };
                Result.KeyItems.Add(rule.Name, item);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " ---- " + path);
            }
        }

        void handleRuleDatabase(RuleItemInfo rule)
        {
            var path=getSingleFile( rule.FileName, rule.RelativePath);
            if (path == null) return;
            try
            {
                var dt = PackageParseHelper.GetDbDataTable(path, rule.DataPath);
                var item = new TableParseResultItem()
                {
                    Rule = rule,
                    Table = dt,
                };
                Result.KeyItems.Add(rule.Name, item);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message + " ---- " + path);
            }

        }

        public void DoWork()
        {
            if( RulePackage == null) return;
            Result = new PackageParseResult()
            {
                Desc = RulePackage.Desc,
                Name = RulePackage.Name,
                KeyItems = new Dictionary<string, DataParseResultItem>()
            };

            foreach(var rule in RulePackage.Items)
            {
                handleRule(rule);
            }
        }


    }
}
