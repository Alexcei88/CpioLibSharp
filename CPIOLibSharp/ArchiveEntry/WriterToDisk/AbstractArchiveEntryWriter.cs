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
            if (_internalEntry.nLink > 0 && !_internalEntry.IsExtractToDisk)
            {
                if (this.IsPostExtractEntry())
                {
                    return true;
                }
                _internalEntry.IsExtractToDisk = true;
                return Write(destFolder);
            }
            return true;
        }

        bool IArchiveEntryWriter.PostExtractEntryToDisk(string destFolder, List<IReadableCPIOArchiveEntry> entries)
        {
            if (_internalEntry.nLink > 0 && !_internalEntry.IsExtractToDisk)
            {
                if (IsPostExtractEntry())
                {
                    // check is hardlinkfile
                    if (_internalEntry.ArchiveType == ArchiveEntryType.FILE && _internalEntry.nLink > 1)
                    {
                        var hardLinkFiles = entries.Where(a => a.INode == _readableArchiveEntry.INode);
                        return ExtractHardlinkFiles(destFolder, hardLinkFiles.ToList());
                    }
                    _internalEntry.IsExtractToDisk = true;
                    return Write(destFolder);
                }
            }
            return true;
        }
        
        public abstract bool IsPostExtractEntry();

        public abstract bool Write(string destFolder);
        
        /// <summary>
        /// extract hardlink files
        /// </summary>
        /// <param name="destFolder"></param>
        /// <param name="archiveEntries"></param>
        /// <returns></returns>
        private bool ExtractHardlinkFiles(string destFolder, List<IReadableCPIOArchiveEntry> archiveEntries)
        {
            var presentFile = archiveEntries.Where(a => a.DataSize > 0).FirstOrDefault();
            if (presentFile == null)
            {
                presentFile = archiveEntries.First();
            }
            FileEntryWriter writer = new FileEntryWriter(_internalEntry, _readableArchiveEntry);
            if (writer.Write(presentFile.InternalEntry, destFolder))
            {
                presentFile.InternalEntry.IsExtractToDisk = true;
                HardLinkEntryWriter hardWriter = new HardLinkEntryWriter();

                foreach (var entry in archiveEntries.Where(a => a.InternalEntry != presentFile.InternalEntry).Select(a => a.InternalEntry))
                {
                    entry.LinkEntry = presentFile.InternalEntry;
                    if (!hardWriter.Write(entry, destFolder))
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
