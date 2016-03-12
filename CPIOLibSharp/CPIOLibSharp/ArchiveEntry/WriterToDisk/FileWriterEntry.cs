using System.IO;
using System.Linq;

namespace CPIOLibSharp.ArchiveEntry.WriterToDisk
{
    /// <summary>
    /// Simple file writer of entry
    /// </summary>
    internal class FileWriterEntry
        : IWriterEntry
    {
        public bool IsPostExtractEntry(InternalWriteArchiveEntry _entry)
        {
            return false;
        }

        public bool Write(InternalWriteArchiveEntry _entry, string destFolder)
        {
            string fileName = InternalWriteArchiveEntry.GetFileName(_entry.FileName);
            string fullPathToFile = Path.Combine(destFolder, fileName);
            string root = Path.GetDirectoryName(fullPathToFile);
            if (Directory.CreateDirectory(root) != null)
            {
                using (FileStream fs = new FileStream(fullPathToFile, FileMode.Create))
                {
                    var data = _entry.Data.Where(g => g != '\0').ToArray();
                    fs.Write(data, 0, data.Length);
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