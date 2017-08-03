using System;
using System.IO;
using System.Linq;

namespace CPIOLibSharp.ArchiveEntry.WriterToDisk
{
    /// <summary>
    /// Writer entry for simple file 
    /// /// </summary>
    internal class FileEntryWriter
        : AbstractArchiveEntryWriter
    {
        public FileEntryWriter(InternalWriteArchiveEntry internalEntry, IReadableCPIOArchiveEntry readableArchiveEntry) 
            : base(internalEntry, readableArchiveEntry)
        {
        }

        public bool ExtractEntryToDisk(string destFolder)
        {
            throw new NotImplementedException();
        }

        public override bool IsPostExtractEntry()
        {
            return false;
        }

        public override bool Write(string destFolder)
        {
            string fileName = InternalWriteArchiveEntry.GetFileName(_internalEntry.FileName);
            string fullPathToFile = Path.Combine(destFolder, fileName);
            string root = Path.GetDirectoryName(fullPathToFile);
            if (Directory.CreateDirectory(root) != null)
            {
                using (FileStream fs = new FileStream(fullPathToFile, FileMode.Create))
                {
                    if (_internalEntry.Data != null)
                    {
                        var data = _internalEntry.Data.Where(g => g != '\0').ToArray();
                        fs.Write(data, 0, data.Length);
                    }
                }

                if ((_internalEntry.ExtractFlags & (uint)CpioExtractFlags.ARCHIVE_EXTRACT_TIME) > 0)
                {
                    File.SetLastWriteTimeUtc(fullPathToFile, _internalEntry.mTime);
                }

                return true;
            }
            return false;
        }
    }
}