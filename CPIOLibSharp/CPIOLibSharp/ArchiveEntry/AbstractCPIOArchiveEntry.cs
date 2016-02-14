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
        protected byte[] _fileName;

        protected byte[] _data;

        public abstract int EntrySize { get; }

        public abstract long DataSize { get; }

        public abstract long FileNameSize { get; }

        public abstract bool FillEntry(byte[] data);

        public void FillFileNameData(byte[] data)
        {
            _fileName = data;
        }

        /// <summary>
        /// Возвращает имя текущего раздела архива
        /// </summary>
        /// <returns></returns>
        protected string GetFileName()
        {
            StringBuilder fileName = new StringBuilder();
            int i = 0;
            while (i < _fileName.Length && _fileName[i] != '\0')
            {
                fileName.Append((char)_fileName[i++]);
            }
            return fileName.ToString();
        }


        public void FillDataEntry(byte[] data)
        {
            _data = data;
        }

        public bool ExtractEntryToDisk(string destFolder)
        {
            string file = GetFileName();
            return true;
        }
    }
}
