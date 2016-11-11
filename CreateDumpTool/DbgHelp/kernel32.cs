using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CreateDumpTool
{
    class kernel32
    {
        [DllImport("kernel32")]
        public static extern void GetSystemInfo(ref DumpConstants.SYSTEM_INFO cpuinfo);
    }
}
