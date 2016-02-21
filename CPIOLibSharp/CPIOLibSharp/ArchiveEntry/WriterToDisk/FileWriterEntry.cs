using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPIOLibSharp.ArchiveEntry.WriterToDisk
{
    class FileWriterEntry
        : IWriterEntry
    {
        public bool Write(InternalArchiveEntry _entry, string destFolder)
        {
            string fileName = InternalArchiveEntry.GetFileName(_entry.FileName);
            string path = Path.Combine(destFolder, fileName);
            string root = Path.GetDirectoryName(path );
            if (Directory.CreateDirectory(root) != null)
            {
                using (FileStream fs = new FileStream(path, FileMode.CreateNew))
                {
                    fs.Write(_entry.Data, 0, _entry.Data.Length);
                }
                return true;
            }
            return false;
        }
    }
}
