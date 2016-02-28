using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CPIOLibSharp.ArchiveEntry
{
    class CRCFormatReaderArchiveEntry
        : AbstractReaderCPIOArchiveEntry
    {
        private CpioStruct.cpio_newc_header _entry = new CpioStruct.cpio_newc_header();

        public CRCFormatReaderArchiveEntry(ArchiveTypes.ExtractArchiveFlags flags)
            : base(flags)
        { }

        public override long DataSize
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

        public override long FileNameSize
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
