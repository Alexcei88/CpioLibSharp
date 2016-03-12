using System.Collections.Generic;

namespace CPIOLibSharp.ArchiveEntry
{
    /// <summary>
    /// Reader from cpio archive entry
    /// </summary>
    internal interface IReaderCPIOArchiveEntry
    {
        /// <summary>
        /// Размер структура с метаданными
        /// </summary>
        int EntrySize { get; }

        /// <summary>
        /// Size of data
        /// </summary>
        ulong DataSize { get; }

        /// <summary>
        /// Size of filename data
        /// </summary>
        ulong FileNameSize { get; }

        /// <summary>
        /// Имя файла
        /// </summary>
        string FileName { get; }

        /// <summary>
        /// Fill struct entry
        /// </summary>
        /// <param name="data"></param>
        bool FillEntry(byte[] data);

        /// <summary>
        /// Fill filename data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        void FillFileNameData(byte[] data);

        /// <summary>
        /// Fill data of entry
        /// </summary>
        /// <param name="data"></param>
        void FillDataEntry(byte[] data);

        /// <summary>
        /// Save entry to disk if it possible
        /// </summary>
        /// <param name="destFolder"></param>
        /// <returns></returns>
        bool ExtractEntryToDisk(string destFolder);

        /// <summary>
        /// Save entry to disk after read all entry
        /// </summary>
        /// <param name="destFolder"></param>
        /// <param name="archiveEntries"></param>
        /// <returns></returns>
        bool PostExtractEntryToDisk(string destFolder, List<IReaderCPIOArchiveEntry> archiveEntries);

        /// <summary>
        /// Is last entry in the file(trailer entry)
        /// </summary>
        /// <returns></returns>
        bool IsLastArchiveEntry();

        /// <summary>
        /// Internal archive entry
        /// </summary>
        InternalWriteArchiveEntry InternalEntry { get; }
    }
}