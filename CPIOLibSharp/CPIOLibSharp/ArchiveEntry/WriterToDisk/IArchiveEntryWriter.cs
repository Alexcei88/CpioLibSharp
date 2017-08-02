namespace CPIOLibSharp.ArchiveEntry.WriterToDisk
{
    /// <summary>
    /// Interface of writer a archive entry to disk
    /// </summary>
    internal interface IWriterArchiveEntry
    {
        /// <summary>
        /// Save entry to disk if it possible
        /// </summary>
        /// <param name="destFolder"></param>
        /// <returns></returns>
        bool ExtractEntryToDisk(string destFolder);

        /// <summary>
        /// save entry to disk
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="destFolder"></param>
        /// <returns></returns>
        bool Write(InternalWriteArchiveEntry entry, string destFolder);

        /// <summary>
        /// Is post-extract entry
        /// </summary>
        /// <param name="_entry"></param>
        /// <returns></returns>
        bool IsPostExtractEntry(InternalWriteArchiveEntry _entry);
    }
}