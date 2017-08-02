using System.IO;

namespace CPIOLibSharp.ArchiveEntry.WriterToDisk
{
    /// <summary>
    /// Writer entry for directory
    /// </summary>
    internal class DirectoryWriterEntry
        : IArchiveEntryWriter
    {
        public bool IsPostExtractEntry(InternalWriteArchiveEntry _entry)
        {
            return false;
        }

        public bool Write(InternalWriteArchiveEntry _entry, string destFolder)
        {
            string dir = InternalWriteArchiveEntry.GetFileName(_entry.FileName);
            var d = Directory.GetParent(dir);
            string fullPathToDir = Path.Combine(destFolder, dir);
            if (Directory.CreateDirectory(fullPathToDir) != null)
            {
                if ((_entry.ExtractFlags & (uint)CpioExtractFlags.ARCHIVE_EXTRACT_TIME) > 0)
                {
                    string currentDir = fullPathToDir;
                    string _dir = dir;
                    do
                    {
                        Directory.SetLastWriteTimeUtc(currentDir, _entry.mTime);
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