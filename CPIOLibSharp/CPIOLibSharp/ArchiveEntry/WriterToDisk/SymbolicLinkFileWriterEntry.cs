using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CPIOLibSharp.ArchiveEntry.WriterToDisk
{
    /// <summary>
    /// Symbolick link for file writer of entry
    /// </summary>
    class SymbolicLinkFileWriterEntry
        : IWriterEntry
    {
        public bool IsPostExtractEntry(InternalWriteArchiveEntry _entry)
        {
            return true;
        }

        public bool Write(InternalWriteArchiveEntry _entry, string destFolder)
        {
            string fileName = InternalWriteArchiveEntry.GetFileName(_entry.FileName);
            string fullPathToFile = Path.Combine(destFolder, fileName);
            string root = Path.GetDirectoryName(fullPathToFile);
            if (Directory.CreateDirectory(root) != null)
            {
                string targetFile = InternalWriteArchiveEntry.GetTargetFileToLink(_entry.Data);
                string fullPathToTargetFile = Path.Combine(destFolder, targetFile);
                if (WindowsNativeLibrary.CreateSymbolicLink(fullPathToFile, fullPathToTargetFile, 0))
                {
                    if ((_entry.ExtractFlags & (uint)ArchiveTypes.ExtractArchiveFlags.ARCHIVE_EXTRACT_TIME) > 0)
                    {
                        throw new Exception("For symbolic link function of extract time not works!!!");
                    }

                    return true;
                }
                else
                {
                    Console.WriteLine("Symbolic link for file {0} no created. Win32Error: {1}", fullPathToFile, Marshal.GetLastWin32Error());
                    return false;
                }
            }
            return false;
        }
    }
}
