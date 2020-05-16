using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace mkhdlndir
{
    public static class Utils
    {
        public static bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        public static void CreateHardLink(string linkFull, string targetFull)
        {
            CreateHardLinkW(linkFull, targetFull, IntPtr.Zero);

            [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
            static extern bool CreateHardLinkW(
                string lpFileName,
                string lpExistingFileName,
                IntPtr lpSecurityAttributes
            );
        }
    }
}