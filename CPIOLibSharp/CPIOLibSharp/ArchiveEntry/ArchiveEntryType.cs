using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPIOLibSharp.ArchiveEntry
{
    /// <summary>
    /// Тип раздела архива
    /// </summary>
    enum ArchiveEntryType
    {
         FILE_TYPE_BITS = 0170,
         SOCKET = 0140,
         SYMBOLIC_LINK = 0120,
         FILE = 0100,
         BLOCK_SPEC_DEVICE = 0060,
         DIRECTORY = 0040,
         CHARACTER_SPEC_DEVICE = 0020,
         FIFO = 0010,
         SUID = 0004,
         GUID = 0002,
         STICKY_BIT = 0001
    }    
}
