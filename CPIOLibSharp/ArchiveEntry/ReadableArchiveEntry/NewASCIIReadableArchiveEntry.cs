using CPIOLibSharp.Formats;
using CPIOLibSharp.Helper;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace CPIOLibSharp.ArchiveEntry
{
    /// <summary>
    /// Reader for NewASCII format
    /// </summary>
    internal class NewASCIIReadableArchiveEntry
        : AbstractReadableArchiveEntry
    {
        private CpioStructDefinition.cpio_newc_header _entry = new CpioStructDefinition.cpio_newc_header();

        /// <summary>
        /// Magic value
        /// </summary>
        public static byte[] CHECK_FIELD_VALUE = { (byte)'0', (byte)'0', (byte)'0', (byte)'0', (byte)'0', (byte)'0', (byte)'0', (byte)'0', };

        public NewASCIIReadableArchiveEntry(uint flags)
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
                        string dataSize = Encoding.ASCII.GetString(GetByteArrayFromFixedArray(pointer, 8));
                        ulong size = ulong.Parse(dataSize, System.Globalization.NumberStyles.HexNumber);
                        return size % 4 == 0 ? size : (size + 4) / 4 * 4;
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
                        byte[] buffer = GetByteArrayFromFixedArray(pointer, 8);
                        string fileNameSize = Encoding.ASCII.GetString(buffer);
                        ulong size = ulong.Parse(fileNameSize, System.Globalization.NumberStyles.HexNumber);
                        ulong commonSize = size + (ulong)EntrySize;
                        return commonSize % 4 == 0 ? size : (4 - commonSize % 4) + size;
                    }
                }
            }
        }

        public override bool HasData
        {
            get
            {
                unsafe
                {
                    fixed (byte* pointer = _entry.c_filesize)
                    {
                        string dataSize = Encoding.ASCII.GetString(GetByteArrayFromFixedArray(pointer, 8));
                        ulong size = ulong.Parse(dataSize, System.Globalization.NumberStyles.HexNumber);
                        return size != 0;
                    }
                }
            }
        }

        public override bool ReadMetadataEntry(byte[] data)
        {
            IntPtr @in = Marshal.AllocHGlobal(EntrySize);
            Marshal.Copy(data, 0, @in, EntrySize);
            _entry = (CpioStructDefinition.cpio_newc_header)Marshal.PtrToStructure(@in, _entry.GetType());
            Marshal.FreeHGlobal(@in);

            unsafe
            {
                byte[] buffer;
                // check magic
                fixed (byte* pointer = _entry.c_magic)
                {
                    buffer = GetByteArrayFromFixedArray(pointer, 6);
                }
                if (!AbstractCPIOFormat.ByteArrayCompare(buffer, NewASCIIFormat.MAGIC_ARCHIVEENTRY_NUMBER))
                {
                    return false;
                }

                // check check field
                fixed (byte* pointer = _entry.c_check)
                {
                    buffer = GetByteArrayFromFixedArray(pointer, 8);
                }
                if (!AbstractCPIOFormat.ByteArrayCompare(buffer, CHECK_FIELD_VALUE))
                {
                    return false;
                }
            }
            FillInternalEntry();
            return true;
        }

        private void FillInternalEntry()
        {
            unsafe
            {
                byte[] majorBuffer;
                byte[] minorBuffer;
                // Dev
                fixed (byte* pointer = _entry.c_devmajor)
                {
                    majorBuffer = GetByteArrayFromFixedArray(pointer, 8);
                }

                fixed (byte* pointer = _entry.c_devminor)
                {
                    minorBuffer = GetByteArrayFromFixedArray(pointer, 8);
                }
                _archiveEntry.Dev = GetValueFromHexValue(majorBuffer).ToString() + GetValueFromHexValue(minorBuffer).ToString();

                // Ino
                fixed (byte* pointer = _entry.c_ino)
                {
                    majorBuffer = GetByteArrayFromFixedArray(pointer, 8);
                }
                _archiveEntry.INode = GetValueFromHexValue(majorBuffer).ToString();

                // Type, Permission
                fixed (byte* pointer = _entry.c_mode)
                {
                    majorBuffer = GetByteArrayFromFixedArray(pointer, 8);
                }
                long mode = GetValueFromHexValue(majorBuffer);
                _archiveEntry.ArchiveType = InternalWriteArchiveEntry.GetArchiveEntryType(mode);
                _archiveEntry.Permission = InternalWriteArchiveEntry.GetPermission(mode);

                // Uid
                fixed (byte* pointer = _entry.c_uid)
                {
                    majorBuffer = GetByteArrayFromFixedArray(pointer, 8);
                }
                _archiveEntry.Uid = (int)GetValueFromHexValue(majorBuffer);

                // Gid
                fixed (byte* pointer = _entry.c_gid)
                {
                    majorBuffer = GetByteArrayFromFixedArray(pointer, 8);
                }
                _archiveEntry.Gid = (int)GetValueFromHexValue(majorBuffer);

                // mTime
                fixed (byte* pointer = _entry.c_mtime)
                {
                    majorBuffer = GetByteArrayFromFixedArray(pointer, 8);
                }
                _archiveEntry.mTime = GetValueFromHexValue(majorBuffer).ToUnixTime();

                // nLink
                fixed (byte* pointer = _entry.c_nlink)
                {
                    majorBuffer = GetByteArrayFromFixedArray(pointer, 8);
                }
                _archiveEntry.nLink = GetValueFromHexValue(majorBuffer);

                // rDev
                fixed (byte* pointer = _entry.c_rdevmajor)
                {
                    majorBuffer = GetByteArrayFromFixedArray(pointer, 8);
                }

                fixed (byte* pointer = _entry.c_rdevminor)
                {
                    minorBuffer = GetByteArrayFromFixedArray(pointer, 8);
                }
                _archiveEntry.rDev = (int)GetValueFromHexValue(majorBuffer) + (int)GetValueFromHexValue(minorBuffer);

                _archiveEntry.ExtractFlags = _extractFlags;
            }
        }

        private long GetValueFromHexValue(byte[] buffer)
        {
            string fileNameSize = Encoding.ASCII.GetString(buffer);
            return long.Parse(fileNameSize, System.Globalization.NumberStyles.HexNumber);
        }
    }
}