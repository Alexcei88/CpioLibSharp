using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPIOLibSharp.ArchiveEntry.WriterToDisk
{
    class DirectoryWriterEntry
        : IWriterEntry
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
            if(Directory.CreateDirectory(fullPathToDir) != null)
            {
                if ((_entry.ExtractFlags & (uint)ArchiveTypes.ExtractArchiveFlags.ARCHIVE_EXTRACT_TIME) > 0)
                {
                    string currentDir = fullPathToDir;
                    string _dir = dir;
                    do
                    {
                        Directory.SetLastWriteTimeUtc(currentDir, _entry.mTime);
                        _dir = Path.GetDirectoryName(_dir);
                        currentDir = Path.GetDirectoryName(currentDir);
                    }                        
                    while(!string.IsNullOrEmpty(_dir) && _dir != ".");                
                }
            }
            return false;
        }
    }
}
