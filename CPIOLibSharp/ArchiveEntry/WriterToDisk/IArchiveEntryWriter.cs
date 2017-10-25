using System.Collections.Generic;

namespace CPIOLibSharp.ArchiveEntry.WriterToDisk
{
    /// <summary>
    /// Interface of writer a archive entry to disk
    /// </summary>
    internal interface IArchiveEntryWriter
    {
        /// <summary>
        /// Save entry to disk if it possible now
        /// </summary>
        /// <param name="destFolder"></param>
        /// <returns>path to file</returns>
        string ExtractEntryToDisk(string destFolder);

        /// <summary>
        /// Postprocessing for entry(for example for files is link)
        /// </summary>
        /// <param name="destFolder"></param>
        /// <param name="entries"></param>
        /// <returns>path to file</returns>
        string PostExtractEntryToDisk(string destFolder, List<IReadableCPIOArchiveEntry> entries);
    }
}