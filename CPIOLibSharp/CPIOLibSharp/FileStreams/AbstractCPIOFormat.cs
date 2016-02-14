using CPIOLibSharp.ArchiveEntry;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CPIOLibSharp.FileStreams
{
    abstract class AbstractCPIOFormat
        : ICPIOFormat
    {
        protected FileStream _fileStream;

        public AbstractCPIOFormat(FileStream stream)
        {
            _fileStream = stream;
        }

        public bool Save(string destFolder)
        {
            _fileStream.Seek(0, SeekOrigin.Begin);


            while (_fileStream.CanRead)
            {
                ICPIOArchiveEntry archiveEntry = GetArchiveEntry();
                int sizeBuffer = archiveEntry.EntrySize;
                byte[] buffer = new byte[sizeBuffer];
                _fileStream.Read(buffer, 0, sizeBuffer);
                archiveEntry.FillEntry(buffer);
                int fileNameSize = (int)archiveEntry.FileNameSize;
                if (fileNameSize > 0)
                {
                    byte[] fileName = new byte[fileNameSize];
                    _fileStream.Read(fileName, 0, fileNameSize);
                    archiveEntry.FillFileNameData(fileName);
                }
                long dataSize = archiveEntry.DataSize;
                if (dataSize > 0)
                {
                    byte[] data = new byte[dataSize];
                    _fileStream.Read(data, 0, (int)dataSize);
                    archiveEntry.FillDataEntry(data);
                }
                if(!archiveEntry.ExtractEntryToDisk(destFolder))
                {
                    return false;
                }
            }


            return true;
        }

        /// <summary>
        /// Возвращает конкретный экземпляр ArchiveEnty
        /// Фабричный метож
        /// </summary>
        /// <returns></returns>
        public abstract ICPIOArchiveEntry GetArchiveEntry();

        /// <summary>
        /// Определение формата CPIO 
        /// </summary>
        /// <returns></returns>
        public abstract bool DetectFormat();

        /// <summary>
        /// Проверка на равенство двух массивов байт
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        static public bool ByteArrayCompare(byte[] a1, byte[] a2)
        {
            return StructuralComparisons.StructuralEqualityComparer.Equals(a1, a2);
        }
    }
}
