﻿namespace CPIOLibSharp.ArchiveEntry
{
    /// <summary>
    /// A Type entry of archive
    /// </summary>
    internal enum ArchiveEntryType
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