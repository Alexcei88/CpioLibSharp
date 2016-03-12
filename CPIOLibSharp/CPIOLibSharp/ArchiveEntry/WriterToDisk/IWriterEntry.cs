﻿namespace CPIOLibSharp.ArchiveEntry.WriterToDisk
{
    /// <summary>
    /// Interface for writer a entry of archive to a disk
    /// </summary>
    internal interface IWriterEntry
    {
        /// <summary>
        /// save entry to disk
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="destFolder"></param>
        /// <returns></returns>
        bool Write(InternalWriteArchiveEntry entry, string destFolder);

        /// <summary>
        /// Is post-extract required
        /// </summary>
        /// <param name="_entry"></param>
        /// <returns></returns>
        bool IsPostExtractEntry(InternalWriteArchiveEntry _entry);
    }
}