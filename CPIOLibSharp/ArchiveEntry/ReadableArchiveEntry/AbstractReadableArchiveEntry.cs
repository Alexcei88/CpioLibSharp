using CPIOLibSharp.ArchiveEntry.WriterToDisk;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CPIOLibSharp.ArchiveEntry
{
    /// <summary>
    /// Base class of reader for archive entry
    /// </summary>
    internal abstract class AbstractReadableArchiveEntry
        : IReadableCPIOArchiveEntry
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

        public abstract bool HasData { get; }
        
        /// <summary>
        /// Writer
        /// </summary>
        public IArchiveEntryWriter _writer;

        public byte[] FileName
        {
            get
            {
                return _archiveEntry.FileName;
            }
            set
            {
                _archiveEntry.FileName = value;
            }
        }

        public byte[] Data
        {
            get
            {
                return _archiveEntry.Data;
            }
            set
            {
                _archiveEntry.Data = value;
            }
        }

        public InternalWriteArchiveEntry InternalEntry
        {
            get
            {
                return _archiveEntry;
            }
        }

        public IArchiveEntryWriter Writer
        {
            get
            {
                if (_writer == null)
                {
                    _writer = InternalWriteArchiveEntry.GetWriter(_archiveEntry, this);
                }
                return _writer;
            }
        }

        public string INode
        {
            get
            {
                return _archiveEntry.INode;
            }
        }

        public AbstractReadableArchiveEntry(uint extractFlags)
        {
            _extractFlags = extractFlags;
        }

        /// <summary>
        /// Fill metadata of entry
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public abstract bool FillEntry(byte[] data);

        /// <summary>
        /// Read netadata from entry
        /// </summary>
        public abstract void ReadMetadataEntry();

        /// <summary>
        /// Сoхранение данных об имени фа   ла
        /// </summary>
        /// <param name="data"></param>
        public void FillFileNameData(byte[] data)
        {
            _archiveEntry.FileName = data;
        }

        /*
        /// <summary>
        /// Post extract entry
        /// </summary>
        /// <param name="destFolder"></param>
        /// <param name="archiveEntries"></param>
        /// <returns></returns>
        public bool PostExtractEntryToDisk(string destFolder, List<IReaderCPIOArchiveEntry> archiveEntries)
        {
            if (_archiveEntry.nLink > 0 && !_archiveEntry.IsExtractToDisk)
            {
                IArchiveEntryWriter writer = InternalWriteArchiveEntry.GetWriter(_archiveEntry);
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
        */

        /// <summary>
        /// Is a entry has name is TRAILER?
        /// </summary>
        /// <returns></returns>
        public bool IsLastArchiveEntry()
        {
            return InternalWriteArchiveEntry.GetFileName(_archiveEntry.FileName).Equals(CpioStructDefinition.LAST_ARCHIVEENTRY_FILENAME);
        }

        /// <summary>
        /// Get a array of bytes from input pointer on array
        /// </summary>
        /// <param name="source"></param>
        /// <param name="length"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get a array of bytes from input pointer on array
        /// </summary>
        /// <param name="source"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        protected static unsafe byte[] GetByteArrayFromFixedArray(ushort* source, int length)
        {
            int len = length * sizeof(ushort);
            byte[] buffer = new byte[len];
            unsafe
            {
                int i = 0;
                for (ushort* d = source; i < length; ++d, ++i)
                {
                    var buf = BitConverter.GetBytes(*d);
                    buf.CopyTo(buffer, (length - 1 - i) * sizeof(ushort));
                }
            }
            return buffer;
        }       
    }
}