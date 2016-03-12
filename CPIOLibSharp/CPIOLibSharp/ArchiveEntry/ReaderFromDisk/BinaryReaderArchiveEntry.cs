using System;

namespace CPIOLibSharp.ArchiveEntry.ReaderFromDisk
{
    internal class BinaryReaderArchiveEntry
        : AbstractReaderCPIOArchiveEntry
    {
        public BinaryReaderArchiveEntry(uint flags)
            : base(flags)
        { }

        public override ulong DataSize
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override int EntrySize
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override ulong FileNameSize
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool FillEntry(byte[] data)
        {
            throw new NotImplementedException();
        }

        protected override bool FillInternalEntry()
        {
            throw new NotImplementedException();
        }
    }
}