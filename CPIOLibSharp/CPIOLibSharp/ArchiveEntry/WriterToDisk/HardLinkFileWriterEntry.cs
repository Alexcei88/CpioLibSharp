using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPIOLibSharp.ArchiveEntry.WriterToDisk
{
    /// <summary>
    /// Hard link for file writer of entry
    /// </summary>
    class HardLinkFileWriterEntry
        : IWriterEntry
    {
        public bool Write(InternalWriteArchiveEntry _entry, string destFolder)
        {
            return true;
        }
    }
}
