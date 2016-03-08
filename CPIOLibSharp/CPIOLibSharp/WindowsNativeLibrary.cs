using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CPIOLibSharp.ArchiveEntry
{
    static class WindowsNativeLibrary
    {
        public enum SYMBOLIC_LINK_FLAG
        {
            File = 0,
            Directory = 1
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CreateSymbolicLink(string lpSymlinkFileName, string lpTargetFileName, SYMBOLIC_LINK_FLAG dwFlags);

        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();

    }
}
