using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPIOLibSharp.ArchiveEntry
{
    abstract class AbstractCPIOArchiveEntry
        : ICPIOArchiveEntry
    {
        protected InternalArchiveEntry _archiveEntry = new InternalArchiveEntry();

        public abstract int EntrySize { get; }

        public abstract long DataSize { get; }

        public abstract long FileNameSize { get; }

        /// <summary>
        /// Заполнение внутренней структуры 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public abstract bool FillEntry(byte[] data);
                
        protected abstract bool FillInternalEntry();

            
        public void FillFileNameData(byte[] data)
        {
            _archiveEntry.FileName = data;
        }

        public void FillDataEntry(byte[] data)
        {
            _archiveEntry.Data = data;
        }

        public bool ExtractEntryToDisk(string destFolder)
        {
            FillInternalEntry();
                
            string file = InternalArchiveEntry.GetFileName(_archiveEntry.FileName);
            //string file = GetFileName();
            return true;

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

        /// <summary>
        /// Последний ли раздел в архиве
        /// </summary>
        /// <returns></returns>
        public bool IsLastArchiveEntry()
        {
            return InternalArchiveEntry.GetFileName(_archiveEntry.FileName).Equals(CpioStruct.LAST_ARCHIVEENTRY_FILENAME);
        }
    }
}
