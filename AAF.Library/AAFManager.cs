using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AAF.Library.Extracter;
using AAF.Library.ExtractRule;
using AAF.Library.Parser;

namespace AAF.Library
{
    public class AAFManager
    {
        private static readonly AAFManager instance = new AAFManager();
        public static AAFManager Instance { get { return instance; } }

        RuleManager RuleManager;
        ExtractManager ExtractManager;
        ParseManager ParseManager;

        private AAFManager()
        {
            Init();
        }

        public void Init()
        {
            if(RuleManager==null)
            {
                RuleManager = new RuleManager();
                RuleManager.LoadRulePackages();

                ExtractManager = new ExtractManager();
                ExtractManager.RuleManager = RuleManager;

                ParseManager = new ParseManager();
                ParseManager.RuleManager = RuleManager;
            }
        }

        public List<PackageParseResult> ParseData(string rootPath)
        {
            return ParseManager.ParseData(rootPath);
        }

        public void ExtractDataFromADB(string pcPath)
        {
            ExtractManager.ExtractFromADB(pcPath);
        }

    }
}
