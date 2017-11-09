using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using AAF.Library.Parser;

namespace AAF.ViewModel
{
    public class DeviceSearchBinder
    {
        public string FileName { get; set; }

        public string Type { get; set; }
        public string Size { get; set; }
        public string AccessTime { get; set; }
        public string ModifyTime { get; set; }
    }
}
