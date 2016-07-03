using CPIOLibSharp.Helper;
using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace CPIOLibSharp.ArchiveEntry.WriterToDisk
{
    /// <summary>
    /// Writer of entry with symbolick link
    /// </summary>
    internal class SymbolicLinkFileWriterEntry
        : IWriterEntry
    {
        public bool IsPostExtractEntry(InternalWriteArchiveEntry _entry)
        {
            return true;
        }

        public bool Write(InternalWriteArchiveEntry entry, string destFolder)
        {
            string fileName = InternalWriteArchiveEntry.GetFileName(entry.FileName);
            string fullPathToFile = Path.Combine(destFolder, fileName);
            string root = Path.GetDirectoryName(fullPathToFile);
            if (Directory.CreateDirectory(root) != null)
            {
                string targetFile = InternalWriteArchiveEntry.GetTargetFileToLink(entry.Data);
                string fullPathToTargetFile = Path.Combine(destFolder, targetFile);
                if (WindowsNativeLibrary.CreateSymbolicLink(fullPathToFile, fullPathToTargetFile, 0))
                {
                    if ((entry.ExtractFlags & (uint)ExtractFlags.ARCHIVE_EXTRACT_TIME) > 0)
                    {
                        SetSymLinkLastWriteTime(fullPathToFile, entry.mTime);
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

        private bool SetSymLinkLastWriteTime(string fileName, DateTime lastWriteTime)
        {
            SafeFileHandle handle = WindowsNativeLibrary.CreateFile(fileName, WindowsNativeLibrary.FileAccess.FILE_WRITE_ATTRIBUTES, FileShare.None,
                IntPtr.Zero, (FileMode)3, FileAttributes.ReparsePoint, IntPtr.Zero);

            if (handle.IsInvalid)
            {
                return false;
            }

            //long lpCreationTime = File.GetCreationTimeUtc(fileName).ToFileTimeUtc();
            //long lpLastAccessTime = File.GetLastAccessTimeUtc(fileName).ToFileTimeUtc();
            long lpLastWriteTime = lastWriteTime.ToFileTimeUtc();

            if (!WindowsNativeLibrary.SetFileTime(handle, ref lpLastWriteTime, ref lpLastWriteTime, ref lpLastWriteTime))
            {
                Console.WriteLine(Marshal.GetLastWin32Error());
                return false;
            }
            return true;
        }
    }
}