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
