﻿namespace CPIOLibSharp.ArchiveEntry.WriterToDisk
{
    /// <summary>
    /// Interface of writer a archive entry to disk
    /// </summary>
    internal interface IArchiveEntryWriter
    {
        /// <summary>
        /// Save entry to disk if it possible
        /// </summary>
        /// <param name="destFolder"></param>
        /// <returns></returns>
        bool ExtractEntryToDisk(string destFolder);
    }
}