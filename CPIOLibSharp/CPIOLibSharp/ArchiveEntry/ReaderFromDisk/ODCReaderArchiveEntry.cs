using CPIOLibSharp.FileStreams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CPIOLibSharp.ArchiveEntry.ReaderFromDisk
{
    class ODCReaderArchiveEntry
        : AbstractReaderCPIOArchiveEntry
    {
        private CpioStruct.cpio_odc_header _entry = new CpioStruct.cpio_odc_header();


        public ODCReaderArchiveEntry(uint flags)
            : base(flags)
        { }

        public override ulong DataSize
        {
            get
            {
                unsafe
                {
                    fixed (byte* pointer = _entry.c_filesize)
                    {
                        string dataSize = Encoding.ASCII.GetString(GetByteArrayFromFixedArray(pointer, 11));
                        return Convert.ToUInt64(dataSize, 8);
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
                unsafe
                {
                    fixed (byte* pointer = _entry.c_namesize)
                    {
                        string dataSize = Encoding.ASCII.GetString(GetByteArrayFromFixedArray(pointer, 6));
                        return Convert.ToUInt64(dataSize, 8);
                    }
                }
            }
        }

        public override bool FillEntry(byte[] data)
        {
            IntPtr @in = Marshal.AllocHGlobal(EntrySize);
            Marshal.Copy(data, 0, @in, EntrySize);
            _entry = (CpioStruct.cpio_odc_header)Marshal.PtrToStructure(@in, _entry.GetType());
            Marshal.FreeHGlobal(@in);

            unsafe
            {
                byte[] buffer;
                // check magic
                fixed (byte* pointer = _entry.c_magic)
                {
                    buffer = GetByteArrayFromFixedArray(pointer, 6);
                }
                if (!AbstractCPIOFormat.ByteArrayCompare(buffer, ODCFormat.MAGIC_ARCHIVEENTRY_NUMBER))
                {
                    return false;
                }
            }
            return true;
        }

        protected override bool FillInternalEntry()
        {
            unsafe
            {
                byte[] majorBuffer;
                byte[] minorBuffer;
                // Dev
                fixed (byte* pointer = _entry.c_dev)
                {
                    majorBuffer = GetByteArrayFromFixedArray(pointer, 6);
                }

                _archiveEntry.Dev = GetValueFromOctalValue(majorBuffer).ToString();

                // Ino
                fixed (byte* pointer = _entry.c_ino)
                {
                    majorBuffer = GetByteArrayFromFixedArray(pointer, 6);
                }
                _archiveEntry.INode = GetValueFromOctalValue(majorBuffer).ToString();

                // Type, Permission
                fixed (byte* pointer = _entry.c_mode)
                {
                    majorBuffer = GetByteArrayFromFixedArray(pointer, 6);
                }
                long mode = (long)GetValueFromOctalValue(majorBuffer);
                _archiveEntry.ArchiveType = InternalWriteArchiveEntry.GetArchiveEntryType(mode);
                _archiveEntry.Permission = InternalWriteArchiveEntry.GePermission(mode);

                // Uid
                fixed (byte* pointer = _entry.c_uid)
                {
                    majorBuffer = GetByteArrayFromFixedArray(pointer, 6);
                }
                _archiveEntry.Uid = GetValueFromOctalValue(majorBuffer).ToString();

                // Gid
                fixed (byte* pointer = _entry.c_gid)
                {
                    majorBuffer = GetByteArrayFromFixedArray(pointer, 6);
                }
                _archiveEntry.Gid = GetValueFromOctalValue(majorBuffer).ToString();

                // mTime
                fixed (byte* pointer = _entry.c_mtime)
                {
                    majorBuffer = GetByteArrayFromFixedArray(pointer, 11);
                }
                _archiveEntry.mTime = ((long)(GetValueFromOctalValue(majorBuffer))).ToUnixTime();

                // nLink
                fixed (byte* pointer = _entry.c_nlink)
                {
                    majorBuffer = GetByteArrayFromFixedArray(pointer, 6);
                }
                _archiveEntry.nLink = (long)GetValueFromOctalValue(majorBuffer);

                // rDev
                fixed (byte* pointer = _entry.c_rdev)
                {
                    majorBuffer = GetByteArrayFromFixedArray(pointer, 6);
                }

                _archiveEntry.rDev = GetValueFromOctalValue(majorBuffer).ToString();
                _archiveEntry.ExtractFlags = _extractFlags;
                return true;
            }
        }

        private ulong GetValueFromOctalValue(byte[] buffer)
        {
            string value = Encoding.ASCII.GetString(buffer);
            return Convert.ToUInt64(value, 8);
        }
    }
}
