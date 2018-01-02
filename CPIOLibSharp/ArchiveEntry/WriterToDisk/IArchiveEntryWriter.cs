using System.Collections.Generic;

namespace CPIOLibSharp.ArchiveEntry.WriterToDisk
{
    /// <summary>
    /// Interface a writer of archive entry to the disk
    /// </summary>
    internal interface IArchiveEntryWriter
    {
        /// <summary>
        /// Save an entry to disk if it possible now
        /// </summary>
        /// <param name="destFolder"></param>
        /// <returns>path to file</returns>
        string ExtractEntryToDisk(string destFolder);

        /// <summary>
        /// Postprocessing for the entry(for example for files is type of link)
        /// </summary>
        /// <param name="destFolder"></param>
        /// <param name="entries"></param>
        /// <returns>path to file</returns>
        string PostExtractEntryToDisk(string destFolder, List<IReadableCPIOArchiveEntry> entries);

        /// <summary>
        /// Write an entry to the disk
        /// </summary>
        /// <param name="destFolder"></param>
        /// <param name="entries"></param>
        /// <returns>path to file</returns>
        string WriteEntryToDisk(string destFolder);

    }
}