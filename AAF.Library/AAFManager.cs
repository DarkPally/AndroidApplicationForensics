using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AAF.Library.Extractor;
using AAF.Library.ExtractRule;
using AAF.Library.Searcher;

namespace AAF.Library
{
    public class AAFManager
    {
        private static readonly AAFManager instance = new AAFManager();
        public static AAFManager Instance { get { return instance; } }

        public RuleManager RuleManager;
        public ExtractManager ExtractManager;
        public DataSearcher DataSearcher;
        private AAFManager()
        {
            
        }

        public void Init()
        {
            if(RuleManager==null)
            {
                RuleManager = new RuleManager();
                RuleManager.LoadRulePackages();

                ExtractManager = new ExtractManager();
                ExtractManager.RuleManager = RuleManager;
            }
        }

        public List<PackageExtractResult> ExtractData(string rootPath)
        {
            Init();
            return ExtractManager.ExtractDataFromDirectory(rootPath);
        }

        public List<PackageExtractResult> ExtractDataFromADB()
        {
            Init();
            return ExtractManager.ExtractDataFromADB();
        }

    }
}
