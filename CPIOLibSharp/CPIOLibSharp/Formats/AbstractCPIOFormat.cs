using CPIOLibSharp.ArchiveEntry;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static CPIOLibSharp.ArchiveFormat;

namespace CPIOLibSharp.Formats
{
    internal abstract class AbstractCPIOFormat
        : ICPIOFormat
    {
        protected FileStream _fileStream;

        /// <summary>
        /// CPIO format
        /// </summary>
        protected CpioFormats _format;

        public CpioFormats Format
        {
            get
            {
                return _format;
            }
        }

        public AbstractCPIOFormat(FileStream stream)
        {
            _fileStream = stream;
        }

        public bool Save(string destFolder, ExtractFlags[] flags = null)
        {
            if (Directory.Exists(destFolder))
            {
                bool findTrailer = false;
                _fileStream.Seek(0, SeekOrigin.Begin);

                // list all the archive entry
                List<IReaderCPIOArchiveEntry> archiveEntries = new List<IReaderCPIOArchiveEntry>();

                IReaderCPIOArchiveEntry archiveEntry = GetArchiveEntry(flags);
                int sizeBuffer = archiveEntry.EntrySize;
                byte[] buffer = new byte[sizeBuffer];

                while (_fileStream.Read(buffer, 0, sizeBuffer) == sizeBuffer)
                {
                    archiveEntry = GetArchiveEntry(flags);
                    archiveEntry.FillEntry(buffer);

                    ulong fileNameSize = archiveEntry.FileNameSize;
                    if (fileNameSize != 0)
                    {
                        byte[] fileName = new byte[fileNameSize];
                        _fileStream.Read(fileName, 0, (int)fileNameSize);
                        archiveEntry.FillFileNameData(fileName);
                    }
                    if (archiveEntry.IsLastArchiveEntry())
                    {
                        findTrailer = true;
                        break;
                    }
                    if (archiveEntry.HasData)
                    {
                        ulong dataSize = archiveEntry.DataSize;
                        byte[] data = new byte[dataSize];
                        _fileStream.Read(data, 0, (int)dataSize);
                        archiveEntry.FillDataEntry(data);
                    }

                    if (!archiveEntry.ExtractEntryToDisk(destFolder))
                    {
                        Console.WriteLine("Fail to extract the archive entry: {0}", archiveEntry.ToString());
                        Directory.Delete(destFolder, true);
                        return false;
                    }
                    archiveEntries.Add(archiveEntry);
                }
                if (!findTrailer)
                {
                    Console.WriteLine("Not find the end entry in file. File is invalid format");
                    Directory.Delete(destFolder, true);
                    return false;
                }
                else
                {
                    return PostProcessingSaveArchive(destFolder, archiveEntries);
                }
            }
            else
            {
                throw new Exception(string.Format("The destinition directory {0} not exists", destFolder));
            }
        }

        /// <summary>
        /// Возвращает конкретный экземпляр ArchiveEntry
        /// Fabric method
        /// </summary>
        /// <returns></returns>
        public abstract IReaderCPIOArchiveEntry GetArchiveEntry(ExtractFlags[] flags);

        /// <summary>
        /// Detect CPIO format
        /// </summary>
        /// <returns></returns>
        public abstract bool DetectFormat();

        /// <summary>
        /// Post extract entry to disk(after read all entry from file)
        /// </summary>
        /// <param name="destFolder"></param>
        /// <param name="archiveEntries"></param>
        /// <returns></returns>
        protected virtual bool PostProcessingSaveArchive(string destFolder, List<IReaderCPIOArchiveEntry> archiveEntries)
        {
            foreach (var entry in archiveEntries.Where(a => !a.InternalEntry.IsExtractToDisk))
            {
                if (!entry.PostExtractEntryToDisk(destFolder, archiveEntries))
                {
                    return false;
                }
            }
            return true;
        }

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

        static protected uint GetUintFromExtractArchiveFlags(ExtractFlags[] flags)
        {
            uint exFlags = 0;
            if (flags == null)
                return exFlags;

            foreach (ExtractFlags flag in flags)
            {
                exFlags = exFlags | (uint)flag;
            }
            return exFlags;
        }
    }
}