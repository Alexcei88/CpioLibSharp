using System;
using System.IO;
using System.Runtime.InteropServices;

namespace CPIOLibSharp.ArchiveEntry.WriterToDisk
{
    /// <summary>
    /// Hard link for file writer of entry
    /// </summary>
    internal class HardLinkFileWriterEntry
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
                string targetFile = InternalWriteArchiveEntry.GetFileName(_entry.LinkEntry.FileName);
                string fullPathToTargetFile = Path.Combine(destFolder, targetFile);
                if (WindowsNativeLibrary.CreateHardLink(fullPathToFile, fullPathToTargetFile, IntPtr.Zero))
                {
                    if ((_entry.ExtractFlags & (uint)ArchiveTypes.ExtractArchiveFlags.ARCHIVE_EXTRACT_TIME) > 0)
                    {
                        File.SetLastWriteTimeUtc(fullPathToFile, _entry.mTime);
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