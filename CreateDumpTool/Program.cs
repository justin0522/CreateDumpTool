using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace CreateDumpTool
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcessTheParameters(3316, @"C:\Justin\dump", true);

        }

        private static void ProcessTheParameters(int processId, string dumpFolder, bool isFull)
        {
            Process p = Process.GetProcessById(processId);
            string dumpFile = Path.Combine(dumpFolder, DateTime.Now.ToString(DumpConstants.DATETYPEDefault) + ".dump");
            FileStream fs = new FileStream(dumpFile, FileMode.Create, FileAccess.Write);
            DumpConstants.MINIDUMP_TYPE dumpType = isFull ? DumpConstants.MINIDUMP_TYPE.MiniDumpWithFullMemory : DumpConstants.MINIDUMP_TYPE.MiniDumpNormal;

            CreateMiniDump(p, fs, true, dumpType);
        }

        private static void CreateMiniDump(Process process, FileStream fs, bool is64bit, DumpConstants.MINIDUMP_TYPE dumpType)
        {
            bool retCode = false;
            if (is64bit)
            {
                retCode = DbgHelp_x64.WriteDump(process.Handle, process.Id,
                               fs.SafeFileHandle.DangerousGetHandle(),
                               dumpType ,
                               IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            }
            else
            {
                retCode = DbgHelp_x86.WriteDump(process.Handle, process.Id,
                               fs.SafeFileHandle.DangerousGetHandle(),
                               dumpType,
                               IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            }

            if(!retCode)
            {
                return;
            }


        }

    }
}
