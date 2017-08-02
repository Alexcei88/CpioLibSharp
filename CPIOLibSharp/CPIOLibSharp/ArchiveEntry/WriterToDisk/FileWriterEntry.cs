﻿using System.IO;
using System.Linq;

namespace CPIOLibSharp.ArchiveEntry.WriterToDisk
{
    /// <summary>
    /// Writer entry for simple file 
    /// /// </summary>
    internal class FileWriterEntry
        : IWriterArchiveEntry
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
                    if (_entry.Data != null)
                    {
                        var data = _entry.Data.Where(g => g != '\0').ToArray();
                        fs.Write(data, 0, data.Length);
                    }
                }

                if ((_entry.ExtractFlags & (uint)CpioExtractFlags.ARCHIVE_EXTRACT_TIME) > 0)
                {
                    File.SetLastWriteTimeUtc(fullPathToFile, _entry.mTime);
                }

                return true;
            }
            return false;
        }
    }
}