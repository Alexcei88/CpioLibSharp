using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPIOLibSharp.ArchiveEntry.WriterToDisk
{
    internal abstract class AbstractArchiveEntryWriter
        : IArchiveEntryWriter
    {
        /// <summary>
        /// the internal entry for write to disk
        /// </summary>
        protected InternalWriteArchiveEntry _internalEntry;

        /// <summary>
        /// the readable archive entry
        /// </summary>
        protected IReadableCPIOArchiveEntry _readableArchiveEntry;
        
        public AbstractArchiveEntryWriter(InternalWriteArchiveEntry internalEntry, IReadableCPIOArchiveEntry readableArchiveEntry)
        {
            _internalEntry = internalEntry;
            _readableArchiveEntry = readableArchiveEntry;
        }

        bool IArchiveEntryWriter.ExtractEntryToDisk(string destFolder)
        {
            return true;
        }

        public abstract bool IsPostExtractEntry();

        public abstract bool Write(string destFolder);
    }
}
