using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAF.ViewModel
{
    public class VMMain
    {
        private static readonly VMMain instance = new VMMain();
        public static VMMain Instance { get { return instance; } }

        private VMMain()
        {
        }

        VMExtractor vmExtractor = new VMExtractor();
        public VMExtractor VMExtractor { get { return vmExtractor; } }

        VMSearcher vmSearcher = new VMSearcher();
        public VMSearcher VMSearcher { get { return vmSearcher; } }
    }
}
