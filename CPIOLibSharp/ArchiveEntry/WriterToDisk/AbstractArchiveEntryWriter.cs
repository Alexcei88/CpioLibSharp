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
                if (IsPostExtractEntry())
                {
                    return InternalWriteArchiveEntry.GetFileName(_internalEntry.FileName);
                }
                else
                {
                    return _readableArchiveEntry.Writer.WriteEntryToDisk(destFolder);
                }
            }
            else
            {
                return null;
            }
        }

        string IArchiveEntryWriter.PostExtractEntryToDisk(string destFolder, List<IReadableCPIOArchiveEntry> entries)
        {
            if (_internalEntry.nLink > 0 && !_internalEntry.IsExtractToDisk)
            {
                if (IsPostExtractEntry())
                {
                    switch(_internalEntry.ArchiveType)
                    {
                        case ArchiveEntryType.FILE:
                            {
                                var hardLinkFiles = entries.Where(a => a.INode == _readableArchiveEntry.INode);
                                ExtractHardlinkFiles(destFolder, hardLinkFiles.ToList());
                            }
                            break;
                        case ArchiveEntryType.SYMBOLIC_LINK:
                            {
                                _readableArchiveEntry.Writer.WriteEntryToDisk(destFolder);
                            }
                            break;
                        default:
                            throw new Exception("Not expected type of entry");
                    }
                }
                else
                {
                    throw new Exception("The entry must be saved to disk already!");
                }
            }
            return InternalWriteArchiveEntry.GetFileName(_internalEntry.FileName);
        }

        string IArchiveEntryWriter.WriteEntryToDisk(string destFolder)
        {
            _internalEntry.IsExtractToDisk = Write(destFolder);
            return _internalEntry.IsExtractToDisk ? InternalWriteArchiveEntry.GetFileName(_internalEntry.FileName) : null;
        }

        public abstract bool IsPostExtractEntry();

        public abstract bool Write(string destFolder);

        /// <summary>
        /// extract hardlink files
        /// </summary>
        /// <param name="destFolder"></param>
        /// <param name="archiveEntries"></param>
        /// <returns></returns>
        private void ExtractHardlinkFiles(string destFolder, List<IReadableCPIOArchiveEntry> archiveEntries)
        {
            var originalEntry = archiveEntries.FirstOrDefault(a => a.DataSize > 0);
            if (originalEntry == null)
            {
                throw new Exception("Not found file with data for creating hardlink file");
            }
            originalEntry.Writer.WriteEntryToDisk(destFolder);

            archiveEntries.Remove(originalEntry);
            foreach (var entry in archiveEntries)
            {
                (entry.Writer as HardLinkEntryWriter).OriginalFilePath = InternalWriteArchiveEntry.GetFileName(originalEntry.FileName);
                entry.Writer.WriteEntryToDisk(destFolder);
            }
        }

    }
}
