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
    internal class SymbolicLinkEntryWriter
        : AbstractArchiveEntryWriter
    {
        public SymbolicLinkEntryWriter(InternalWriteArchiveEntry internalEntry, IReadableCPIOArchiveEntry readableArchiveEntry) 
            : base(internalEntry, readableArchiveEntry)
        { }

        public override bool IsPostExtractEntry()
        {
            return true;
        }

        public override bool Write(string destFolder)
        {
            string fileName = InternalWriteArchiveEntry.GetFileName(_internalEntry.FileName);
            string fullPathToFile = Path.Combine(destFolder, fileName);
            string root = Path.GetDirectoryName(fullPathToFile);
            if (Directory.CreateDirectory(root) != null)
            {
                string targetFile = InternalWriteArchiveEntry.GetTargetFileToLink(_internalEntry.Data);
                string fullPathToTargetFile = Path.Combine(destFolder, targetFile);
                if (WindowsNativeLibrary.CreateSymbolicLink(fullPathToFile, fullPathToTargetFile, 0))
                {
                    if ((_internalEntry.ExtractFlags & (uint)CpioExtractFlags.ARCHIVE_EXTRACT_TIME) > 0)
                    {
                        SetSymLinkLastWriteTime(fullPathToFile, _internalEntry.mTime);
                    }
                    return true;
                }
                else
                {
                    throw new Exception(string.Format("Symbolic link for file {0} no created. Win32Error: {1}", fullPathToFile, Marshal.GetLastWin32Error()));
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