using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPIOLibSharp.ArchiveEntry.WriterToDisk
{
    /// <summary>
    /// Simple file writer of entry
    /// </summary>
    class FileWriterEntry
        : IWriterEntry
    {
        public bool Write(InternalWriteArchiveEntry _entry, string destFolder)
        {
            string fileName = InternalWriteArchiveEntry.GetFileName(_entry.FileName);
            string fullPathToFile = Path.Combine(destFolder, fileName);
            string root = Path.GetDirectoryName(fullPathToFile );
            if (Directory.CreateDirectory(root) != null)
            {
                using (FileStream fs = new FileStream(fullPathToFile, FileMode.Create))
                {
                    fs.Write(_entry.Data, 0, _entry.Data.Length);
                }

                if ((_entry.ExtractFlags & (uint)ArchiveTypes.ExtractArchiveFlags.ARCHIVE_EXTRACT_TIME) > 0)
                {
                    File.SetLastWriteTimeUtc(fullPathToFile, _entry.mTime);
                }

                return true;
            }
            return false;
        }
    }
}
