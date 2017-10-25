using System;
using System.IO;

namespace CPIOLibSharp.ArchiveEntry.WriterToDisk
{
    /// <summary>
    /// Writer entry for directory
    /// </summary>
    internal class DirectoryEntryWriter
        : AbstractArchiveEntryWriter
    {
        public DirectoryEntryWriter(InternalWriteArchiveEntry internalEntry, IReadableCPIOArchiveEntry readableArchiveEntry) 
            : base(internalEntry, readableArchiveEntry)
        {
        }

        public override bool IsPostExtractEntry()
        {
            return false;
        }

        public override bool Write(string destFolder)
        {
            string dir = InternalWriteArchiveEntry.GetFileName(_internalEntry.FileName);
            var d = Directory.GetParent(dir);
            string fullPathToDir = Path.Combine(destFolder, dir);
            if (Directory.CreateDirectory(fullPathToDir) != null)
            {
                if ((_internalEntry.ExtractFlags & (uint)CpioExtractFlags.ARCHIVE_EXTRACT_TIME) > 0)
                {
                    string currentDir = fullPathToDir;
                    string _dir = dir;
                    do
                    {
                        Directory.SetLastWriteTimeUtc(currentDir, _internalEntry.mTime);
                        _dir = Path.GetDirectoryName(_dir);
                        currentDir = Path.GetDirectoryName(currentDir);
                    }
                    while (!string.IsNullOrEmpty(_dir) && _dir != ".");
                }
                return true;
            }
            return false;
        }
    }
}