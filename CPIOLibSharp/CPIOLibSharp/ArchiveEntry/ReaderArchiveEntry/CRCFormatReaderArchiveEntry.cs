using System;
using System.Runtime.InteropServices;

namespace CPIOLibSharp.ArchiveEntry
{
    internal class CRCFormatReaderArchiveEntry
        : AbstractReaderCPIOArchiveEntry
    {
        private CpioStruct.cpio_newc_header _entry = new CpioStruct.cpio_newc_header();

        public CRCFormatReaderArchiveEntry(uint extractFlags)
            : base(extractFlags)
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
                return Marshal.SizeOf(_entry.GetType());
            }
        }

        public override ulong FileNameSize
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool HasData
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