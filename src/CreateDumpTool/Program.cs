using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace CreateDumpTool
{
    class Program
    {
        private static string DumpFolder = string.Empty;
        static void Main(string[] args)
        {
            ProcessTheParameters(5188, @"C:\Justin\dump", true);

        }

        private static void ProcessTheParameters(int processId, string dumpFolder, bool isFull)
        {
            Process p = Process.GetProcessById(processId);
            DumpFolder = dumpFolder;
            string dumpFile = Path.Combine(dumpFolder, DateTime.Now.ToString(DumpConstants.DATETYPEDefault) + ".dump");
            FileStream fs = new FileStream(dumpFile, FileMode.Create, FileAccess.Write);
            DumpConstants.MINIDUMP_TYPE dumpType = isFull ? DumpConstants.MINIDUMP_TYPE.MiniDumpWithFullMemory : DumpConstants.MINIDUMP_TYPE.MiniDumpNormal;
            DumpConstants.SYSTEM_INFO systemInfo = new DumpConstants.SYSTEM_INFO();
            kernel32.GetSystemInfo(ref systemInfo);
            CreateMiniDump(p, fs, systemInfo.wProcessorArchitecture == 9, dumpType);
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
                GetSystemInfo();
                return;
            }
            else
            {
               
            }

        }

        private static void GetSystemInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("Computer Name: {0}", Environment.MachineName));
            sb.AppendLine(string.Format("OS Name: {0}", GetOSVersion()));
            sb.AppendLine(string.Format("OS Version: {0}", Environment.OSVersion.VersionString));
            File.WriteAllText(Path.Combine(Program.DumpFolder, "SystemInfo.txt"), sb.ToString(), Encoding.UTF8);

        }

        private static string GetOSVersion()
        {
            string productName = string.Empty;
            RegistryManager.GetRegistryValue<string>(RegistryHive.LocalMachine, RegistryManager.ProductNameRegKeyName, RegistryManager.ProductNameRegValueName, RegistryValueKind.String, out productName);
            return productName;
        }
    }
}
