using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPIOLibSharp.ArchiveEntry.WriterToDisk
{
    /// <summary>
    /// Interface for writer a entry of archive to a disk 
    /// </summary>
    interface IWriterEntry
    {
        bool Write(InternalArchiveEntry _entry, string destFolder);
    }
}
