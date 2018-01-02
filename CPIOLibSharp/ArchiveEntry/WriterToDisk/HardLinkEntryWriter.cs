using CPIOLibSharp.Helper;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace CPIOLibSharp.ArchiveEntry.WriterToDisk
{
    /// <summary>
    /// A Writer of entry with hard link
    /// </summary>
    internal class HardLinkEntryWriter
        : AbstractArchiveEntryWriter
    {
        /// <summary>
        /// original file name
        /// </summary>
        public string OriginalFilePath { get; set; }

        public HardLinkEntryWriter(InternalWriteArchiveEntry internalEntry, IReadableCPIOArchiveEntry readableArchiveEntry) 
            : base(internalEntry, readableArchiveEntry)
        {
        }

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
                string fullPathToTargetFile = Path.Combine(destFolder, OriginalFilePath);
                if (WindowsNativeLibrary.CreateHardLink(fullPathToFile, fullPathToTargetFile, IntPtr.Zero))
                {
                    if ((_internalEntry.ExtractFlags & (uint)CpioExtractFlags.ARCHIVE_EXTRACT_TIME) > 0)
                    {
                        File.SetLastWriteTimeUtc(fullPathToFile, _internalEntry.mTime);
                    }

                    return true;
                }
                else
                {
                    throw new Exception(string.Format("Hardlink for file {0} no created. Win32Error: {1}", fullPathToFile, Marshal.GetLastWin32Error()));
                }
            }
            return false;
        }
    }
}