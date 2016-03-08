using CPIOLibSharp.ArchiveEntry.WriterToDisk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPIOLibSharp.ArchiveEntry
{
    abstract class AbstractReaderCPIOArchiveEntry
        : IReaderCPIOArchiveEntry
    {
        protected InternalWriteArchiveEntry _archiveEntry = new InternalWriteArchiveEntry();

        /// <summary>
        /// flags for optional behaviour
        /// </summary>
        protected uint _extractFlags;

        public abstract int EntrySize { get; }

        public abstract long DataSize { get; }

        public abstract long FileNameSize { get; }

        
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
            bool? result = writer?.Write(_archiveEntry, destFolder);
            return result != null;
        }

        /// <summary>
        /// Последний ли раздел в архиве
        /// </summary>
        /// <returns></returns>
        public bool IsLastArchiveEntry()
        {
            return InternalWriteArchiveEntry.GetFileName(_archiveEntry.FileName).Equals(CpioStruct.LAST_ARCHIVEENTRY_FILENAME);
        }


        protected static unsafe byte[] GetByteArrayFromFixedArray(byte* source, int length)
        {            
            byte[] buffer = new byte[length];
            unsafe
            {
                int i = 0;
                for (byte* d = source; i< length; ++i, ++d)
                {
                    buffer[i] = *d;
                }
            }
            return buffer;
        }
    }
}
