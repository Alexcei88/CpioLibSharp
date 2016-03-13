﻿using CPIOLibSharp.ArchiveEntry.WriterToDisk;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CPIOLibSharp.ArchiveEntry
{
    internal abstract class AbstractReaderCPIOArchiveEntry
        : IReaderCPIOArchiveEntry
    {

        /// <summary>
        /// internal archive entry
        /// </summary>
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
            if (_archiveEntry.nLink > 0 && !_archiveEntry.IsExtractToDisk)
            {
                IWriterEntry writer = InternalWriteArchiveEntry.GetWriter(_archiveEntry);
                if (writer.IsPostExtractEntry(_archiveEntry))
                {
                    return true;
                }
                _archiveEntry.IsExtractToDisk = true;
                return writer.Write(_archiveEntry, destFolder);
            }
            return true;

        }

        public bool PostExtractEntryToDisk(string destFolder, List<IReaderCPIOArchiveEntry> archiveEntries)
        {
            if (_archiveEntry.nLink > 0 && !_archiveEntry.IsExtractToDisk)
            {
                IWriterEntry writer = InternalWriteArchiveEntry.GetWriter(_archiveEntry);
                if (writer.IsPostExtractEntry(_archiveEntry))
                {
                    // check is hardlinkfile
                    if (_archiveEntry.ArchiveType == ArchiveEntryType.FILE && _archiveEntry.nLink > 1)
                    {
                        var hardLinkFiles = archiveEntries.Where(a => a.InternalEntry.INode == _archiveEntry.INode);
                        return ExtractHardlinkFiles(destFolder, hardLinkFiles.ToList());
                    }
                    _archiveEntry.IsExtractToDisk = true;
                    return writer.Write(_archiveEntry, destFolder);
                }
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

        protected static unsafe byte[] GetByteArrayFromFixedArray(ushort* source, int length)
        {
            byte[] buffer = new byte[length * sizeof(ushort)];
            unsafe
            {
                int i = 0;
                for (ushort* d = source; i < length; ++i, ++d)
                {
                    BitConverter.GetBytes(d[i]).CopyTo(buffer, i * sizeof(ushort));
                }
            }
            return buffer;
        }

        /// <summary>
        /// extract hardlink files
        /// </summary>
        /// <param name="destFolder"></param>
        /// <param name="archiveEntries"></param>
        /// <returns></returns>
        private bool ExtractHardlinkFiles(string destFolder, List<IReaderCPIOArchiveEntry> archiveEntries)
        {
            var presentFile = archiveEntries.Where(a => a.DataSize > 0).FirstOrDefault();
            if(presentFile == null)
            {
                presentFile = archiveEntries.First();
            }
            FileWriterEntry writer = new FileWriterEntry();
            if(writer.Write(presentFile.InternalEntry, destFolder))
            {
                presentFile.InternalEntry.IsExtractToDisk = true;
                HardLinkFileWriterEntry hardWriter = new HardLinkFileWriterEntry();

                foreach(var entry in archiveEntries.Where(a => a.InternalEntry != presentFile.InternalEntry).Select(a => a.InternalEntry))
                {
                    entry.LinkEntry = presentFile.InternalEntry;
                    if(!hardWriter.Write(entry, destFolder))
                    {
                        return false;
                    }
                    else
                    {
                        entry.IsExtractToDisk = true;
                    }
                }
                return true;
            }
            return false;
        }
    }
}