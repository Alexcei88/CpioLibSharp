using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPIOLibSharp.ArchiveEntry
{
    /// <summary>
    /// Type entry of archive
    /// </summary>
    enum ArchiveEntryType
    {
         FILE_TYPE_BITS = 120,
         SOCKET = 96,
         SYMBOLIC_LINK = 80,
         FILE = 64,
         BLOCK_SPEC_DEVICE = 48,
         DIRECTORY = 32,
         CHARACTER_SPEC_DEVICE = 16,
         FIFO = 8,
         SUID = 4,
         GUID = 2,
         STICKY_BIT = 1
    }    
}
