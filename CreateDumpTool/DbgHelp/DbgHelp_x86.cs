using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CreateDumpTool
{
    class DbgHelp_x86
    {
        [DllImport(".\\DbgHelpDll\\dbghelp_x86.dll", ExactSpelling = false, SetLastError = true)]
        static extern bool MiniDumpWriteDump(
            IntPtr hProcess,
            Int32 ProcessId,
            IntPtr hFile,
            DumpConstants.MINIDUMP_TYPE DumpType,
            IntPtr ExceptionParam,
            IntPtr UserStreamParam,
            IntPtr CallackParam
        );

        public static bool WriteDump(IntPtr hProcess,
            Int32 ProcessId,
            IntPtr hFile,
            DumpConstants.MINIDUMP_TYPE DumpType,
            IntPtr ExceptionParam,
            IntPtr UserStreamParam,
            IntPtr CallackParam
        )
        {
            return MiniDumpWriteDump(hProcess, ProcessId, hFile, DumpType, ExceptionParam, UserStreamParam, CallackParam);
        }
    }
}
