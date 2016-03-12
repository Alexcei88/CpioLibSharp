using CPIOLibSharp.ArchiveEntry.WriterToDisk;
using System.Collections.Generic;
using System.Linq;

namespace CPIOLibSharp.ArchiveEntry
{
    internal abstract class AbstractReaderCPIOArchiveEntry
        : IReaderCPIOArchiveEntry
    {
        protected InternalWriteArchiveEntry _archiveEntry = new InternalWriteArchiveEntry();

        /// <summary>
        /// flags for optional behaviour
        /// </summary>
        protected uint _extractFlags;

        public abstract int EntrySize { get; }

        public abstract ulong DataSize { get; }

        public abstract ulong FileNameSize { get; }

        public string FileName
        {
            get
            {
                return InternalWriteArchiveEntry.GetFileName(_archiveEntry.FileName);
            }
        }

        public InternalWriteArchiveEntry InternalEntry
        {
            get
            {
                return _archiveEntry;
            }
        }

        public AbstractReaderCPIOArchiveEntry(uint extractFlags)
        {
            _extractFlags = extractFlags;
        }

        /// <summary>
        /// Заполнение CPIO структуры
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public abstract bool FillEntry(byte[] data);

        protected abstract bool FillInternalEntry();

        /// <summary>
        /// СОхранение данных об имени фа   ла
        /// </summary>
        /// <param name="data"></param>
        public void FillFileNameData(byte[] data)
        {
            _archiveEntry.FileName = data;
        }

        /// <summary>
        /// Сохранение данных о файле
        /// </summary>
        /// <param name="data"></param>
        public void FillDataEntry(byte[] data)
        {
            _archiveEntry.Data = data;
        }

        public bool ExtractEntryToDisk(string destFolder)
        {
            FillInternalEntry();
            IWriterEntry writer = InternalWriteArchiveEntry.GetWriter(_archiveEntry);
            if (writer.IsPostExtractEntry(_archiveEntry))
            {
                return true;
            }
            return writer.Write(_archiveEntry, destFolder);
        }

        public bool PostExtractEntryToDisk(string destFolder, List<IReaderCPIOArchiveEntry> archiveEntries)
        {
            IWriterEntry writer = InternalWriteArchiveEntry.GetWriter(_archiveEntry);
            if (writer.IsPostExtractEntry(_archiveEntry))
            {
                // check is hardlinkfile
                if (_archiveEntry.ArchiveType == ArchiveEntryType.FILE && _archiveEntry.nLink > 1)
                {
                    // поиск "настоящего файла" с данными
                    _archiveEntry.LinkEntry = archiveEntries.FirstOrDefault(a => a.InternalEntry.INode == _archiveEntry.INode && a.InternalEntry != _archiveEntry).InternalEntry;
                }

                return writer.Write(_archiveEntry, destFolder);
            }
            return true;
        }

        /// <summary>
        /// Последний ли раздел в архиве
        /// </summary>
        /// <returns></returns>
        public bool IsLastArchiveEntry()
        {
            return FileName.Equals(CpioStruct.LAST_ARCHIVEENTRY_FILENAME);
        }

        protected static unsafe byte[] GetByteArrayFromFixedArray(byte* source, int length)
        {
            byte[] buffer = new byte[length];
            unsafe
            {
                int i = 0;
                for (byte* d = source; i < length; ++i, ++d)
                {
                    buffer[i] = *d;
                }
            }
            return buffer;
        }
    }
}