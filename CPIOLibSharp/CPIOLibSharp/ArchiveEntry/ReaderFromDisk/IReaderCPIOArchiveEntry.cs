using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPIOLibSharp.ArchiveEntry
{
    /// <summary>
    /// Reader from cpio archive entry
    /// </summary>
    public interface IReaderCPIOArchiveEntry
    {
        /// <summary>
        /// Размер структура с метаданными
        /// </summary>
        int EntrySize { get; }

        /// <summary>
        /// Size of data
        /// </summary>
        long DataSize { get; }

        /// <summary>
        /// Size of filename data
        /// </summary>
        long FileNameSize { get; }

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
        /// Save entry to disk
        /// </summary>
        /// <param name="destFolder"></param>
        /// <returns></returns>
        bool ExtractEntryToDisk(string destFolder);

        /// <summary>
        /// Последний ли раздел в файле
        /// </summary>
        /// <returns></returns>
        bool IsLastArchiveEntry();


    }
}
