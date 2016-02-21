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
        public bool Write(InternalArchiveEntry _entry, string destFolder)
        {
            string dir = InternalArchiveEntry.GetFileName(_entry.FileName);
            string path = Path.Combine(destFolder, dir);
            return Directory.CreateDirectory(path) != null;
        }
    }
}
