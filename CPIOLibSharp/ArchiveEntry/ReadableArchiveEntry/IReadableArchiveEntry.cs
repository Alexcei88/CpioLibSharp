using CPIOLibSharp.ArchiveEntry.WriterToDisk;
using System.Collections.Generic;

namespace CPIOLibSharp.ArchiveEntry
{
    /// <summary>
    /// Interface reader from cpio archive entry
    /// </summary>
    internal interface IReadableCPIOArchiveEntry
    {
        /// <summary>
        /// Size of entry
        /// </summary>
        int EntrySize { get; }

        /// <summary>
        /// Is exist data of entry
        /// </summary>
        bool HasData { get; }

        /// <summary>
        /// Size of data
        /// </summary>
        ulong DataSize { get; }

        /// <summary>
        /// Size of filename data
        /// </summary>
        ulong FileNameSize { get; }

        /// <summary>
        /// File name
        /// </summary>
        byte[] FileName { get; set; }

        /// <summary>
        /// Set data entry
        /// </summary>
        byte[] Data { get; set; }

        /// <summary>
        /// Fill metadata of entry
        /// </summary>
        /// <param name="data"></param>
        bool ReadMetadataEntry(byte[] data);

        /// <summary>
        /// 
        /// </summary>
        string INode { get; }

        /*
        /// <summary>
        /// Save entry to disk after read all entry
        /// </summary>
        /// <param name="destFolder"></param>
        /// <param name="archiveEntries"></param>
        /// <returns></returns>
        bool PostExtractEntryToDisk(string destFolder, List<IReadableCPIOArchiveEntry> archiveEntries);
        */
        /// <summary>
        /// Is last entry in the file(trailer entry)
        /// </summary>
        /// <returns></returns>
        bool IsLastArchiveEntry();

        /// <summary>
        /// writer of readable entry to disk
        /// </summary>
        IArchiveEntryWriter Writer { get; }
    }
}