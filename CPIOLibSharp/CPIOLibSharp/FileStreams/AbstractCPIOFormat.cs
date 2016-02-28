using CPIOLibSharp.ArchiveEntry;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

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

        public bool Save(string destFolder, ArchiveTypes.ExtractArchiveFlags flags = 0)
        {
            if (Directory.Exists(destFolder))
            {

                _fileStream.Seek(0, SeekOrigin.Begin);

                // list all the archive entry
                List<IReaderCPIOArchiveEntry> archiveEntries = new List<IReaderCPIOArchiveEntry>();

                IReaderCPIOArchiveEntry archiveEntry = GetArchiveEntry(flags);
                int sizeBuffer = archiveEntry.EntrySize;
                byte[] buffer = new byte[sizeBuffer];
                while (_fileStream.Read(buffer, 0, sizeBuffer) == sizeBuffer)
                {
                    archiveEntry = GetArchiveEntry(flags);
                    archiveEntries.Add(archiveEntry);

                    archiveEntry.FillEntry(buffer);
                    int fileNameSize = (int)archiveEntry.FileNameSize;
                    if (fileNameSize > 0)
                    {
                        byte[] fileName = new byte[fileNameSize];
                        _fileStream.Read(fileName, 0, fileNameSize);
                        archiveEntry.FillFileNameData(fileName);
                    }
                    if (archiveEntry.IsLastArchiveEntry())
                    {
                        return true;
                    }
                    long dataSize = archiveEntry.DataSize;
                    if (dataSize > 0)
                    {
                        byte[] data = new byte[dataSize];
                        _fileStream.Read(data, 0, (int)dataSize);
                        archiveEntry.FillDataEntry(data);
                    }
                    if (!archiveEntry.ExtractEntryToDisk(destFolder))
                    {
                        Console.WriteLine("Fail to extract the archive entry: {0}", archiveEntry.ToString());
                        Directory.Delete(destFolder);
                        return false;
                    }

                }
                Console.WriteLine("Not find the end entry in file. Fail is invalid");
                Directory.Delete(destFolder);
                return false;
            }
            else
            {
                throw new Exception(string.Format("Directory {0} not exist"));
            }
        }

        /// <summary>
        /// Возвращает конкретный экземпляр ArchiveEnty
        /// Фабричный метож
        /// </summary>
        /// <returns></returns>
        public abstract IReaderCPIOArchiveEntry GetArchiveEntry(ArchiveTypes.ExtractArchiveFlags flags);

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
