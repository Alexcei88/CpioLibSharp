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

        string IArchiveEntryWriter.ExtractEntryToDisk(string destFolder)
        {
            if (_internalEntry.nLink > 0 && !_internalEntry.IsExtractToDisk)
            {
                if (this.IsPostExtractEntry())
                {
                    return InternalWriteArchiveEntry.GetFileName(_internalEntry.FileName);
                }
                _internalEntry.IsExtractToDisk = true;
                try
                {
                    return Write(destFolder) ? InternalWriteArchiveEntry.GetFileName(_internalEntry.FileName) : null;
                }
                catch
                {
                    _internalEntry.IsExtractToDisk = false;
                }
            }
            return InternalWriteArchiveEntry.GetFileName(_internalEntry.FileName);
        }

        string IArchiveEntryWriter.PostExtractEntryToDisk(string destFolder, List<IReadableCPIOArchiveEntry> entries)
        {
            if (_internalEntry.nLink > 0 && !_internalEntry.IsExtractToDisk)
            {
                if (IsPostExtractEntry())
                {
                    // check is hardlinkfile
                    if (_internalEntry.ArchiveType == ArchiveEntryType.FILE && _internalEntry.nLink > 1)
                    {
                        // check is it entry with data or is it just link
                        if(_readableArchiveEntry.DataSize <= 0)
                        {
                            var hardLinkFiles = entries.Where(a => a.INode == _readableArchiveEntry.INode);
                            return ExtractHardlinkFiles(destFolder, hardLinkFiles.ToList()) ? InternalWriteArchiveEntry.GetFileName(_internalEntry.FileName) : null;
                        }
                        else
                        {
                            _internalEntry.IsExtractToDisk = true;
                            try
                            {
                                return Write(destFolder) ? InternalWriteArchiveEntry.GetFileName(_internalEntry.FileName) : null;
                            }
                            catch
                            {
                                _internalEntry.IsExtractToDisk = false;
                            }
                        }
                    }
                }
            }
            return InternalWriteArchiveEntry.GetFileName(_internalEntry.FileName);
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
            var originalFile = archiveEntries.FirstOrDefault(a => a.DataSize > 0);
            if (originalFile == null)
            {
                originalFile = archiveEntries.First();
            }
            string originalFileName = originalFile.Writer.PostExtractEntryToDisk(destFolder, archiveEntries);
            if (originalFileName != null)
            {
                HardLinkEntryWriter hardWriter = new HardLinkEntryWriter(_internalEntry, originalFile, originalFileName);
                _internalEntry.IsExtractToDisk = hardWriter.Write(destFolder);
                return _internalEntry.IsExtractToDisk;
            }
            return false;
        }

    }
}
