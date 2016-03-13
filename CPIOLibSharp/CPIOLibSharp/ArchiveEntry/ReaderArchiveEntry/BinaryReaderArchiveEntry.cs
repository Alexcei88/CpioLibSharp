using CPIOLibSharp.FileStreams;
using System;
using System.Runtime.InteropServices;

namespace CPIOLibSharp.ArchiveEntry.ReaderFromDisk
{
    internal class BinaryReaderArchiveEntry
        : AbstractReaderCPIOArchiveEntry
    {
        private CpioStruct.header_old_cpio _entry = new CpioStruct.header_old_cpio();

        public BinaryReaderArchiveEntry(uint flags)
            : base(flags)
        { }

        public override ulong DataSize
        {
            get
            {
                unsafe
                {
                    fixed (ushort* pointer = _entry.c_filesize)
                    {
                        byte[] array =  GetByteArrayFromFixedArray(pointer, 2);
                        return (ulong)BitConverter.ToInt32(array, 0);
                    }
                }
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
                return _entry.c_namesize;
            }
        }

        public override bool FillEntry(byte[] data)
        {
            IntPtr @in = Marshal.AllocHGlobal(EntrySize);
            Marshal.Copy(data, 0, @in, EntrySize);
            _entry = (CpioStruct.header_old_cpio)Marshal.PtrToStructure(@in, _entry.GetType());
            Marshal.FreeHGlobal(@in);

            // check magic                
            if (_entry.c_magic != BinaryFormat.MAGIC_ARCHIVEENTRY_NUMBER)
            {
                return false;
            }
            return true;
        }

        protected override bool FillInternalEntry()
        {
            throw new NotImplementedException();
        }
    }
}