using System;
using CPIOLibSharp;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CPIOLibSharp.ArchiveEntry
{
    class NewASCIIFormatArchiveEntry
        : AbstractCPIOArchiveEntry
    {
        private CpioStruct.cpio_newc_header _entry = new CpioStruct.cpio_newc_header();

        public override long DataSize
        {
            get
            {
                unsafe
                {
                    fixed (byte* pointer = _entry.c_filesize)
                    {
                        string dataSize = Encoding.ASCII.GetString(GetByteArrayFromFixedArray(pointer, 8));
                        int size = int.Parse(dataSize, System.Globalization.NumberStyles.HexNumber);
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

        public override long FileNameSize
        {
            get
            {
                unsafe
                {
                    fixed (byte* pointer = _entry.c_namesize)
                    {
                        byte[] buffer = GetByteArrayFromFixedArray(pointer, 8);
                        string fileNameSize = Encoding.ASCII.GetString(buffer);
                        int size = int.Parse(fileNameSize, System.Globalization.NumberStyles.HexNumber);
                        int commonSize = size + EntrySize;
                        return commonSize % 4 == 0 ? size : (4 - commonSize % 4) + size;
                    }

                }
            }
        }

        public override bool FillEntry(byte[] data)
        {
            IntPtr @in = Marshal.AllocHGlobal(EntrySize);
            Marshal.Copy(data, 0, @in, EntrySize);
            _entry = (CpioStruct.cpio_newc_header)Marshal.PtrToStructure(@in, _entry.GetType());
            Marshal.FreeHGlobal(@in);

            return FillEntry();
        }

        protected override bool FillEntry()
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
                _archiveEntry.Dev = Encoding.ASCII.GetString(majorBuffer) + Encoding.ASCII.GetString(minorBuffer);

                // Ino
                fixed (byte* pointer = _entry.c_ino)
                {
                    majorBuffer = GetByteArrayFromFixedArray(pointer, 8);
                }
                _archiveEntry.Ino = Encoding.ASCII.GetString(majorBuffer);

                // Type, Permission
                fixed (byte* pointer = _entry.c_mode)
                {
                    majorBuffer = GetByteArrayFromFixedArray(pointer, 8);
                }


                // Uid
                fixed (byte* pointer = _entry.c_uid)
                {
                    majorBuffer = GetByteArrayFromFixedArray(pointer, 8);
                }
                _archiveEntry.Uid = Encoding.ASCII.GetString(majorBuffer);

                // Gid
                fixed (byte* pointer = _entry.c_gid)
                {
                    majorBuffer = GetByteArrayFromFixedArray(pointer, 8);
                }
                _archiveEntry.Gid = Encoding.ASCII.GetString(majorBuffer);

                // mTime
                fixed (byte* pointer = _entry.c_mtime)
                {
                    majorBuffer = GetByteArrayFromFixedArray(pointer, 8);
                }
                _archiveEntry.mTime = BitConverter.ToInt64(majorBuffer, 0).ToUnixTime();

                // nLink
                fixed (byte* pointer = _entry.c_nlink)
                {
                    majorBuffer = GetByteArrayFromFixedArray(pointer, 8);
                }
                _archiveEntry.nLink = BitConverter.ToInt32(majorBuffer, 0);

                // rDev
                fixed (byte* pointer = _entry.c_rdevmajor)
                {
                    majorBuffer = GetByteArrayFromFixedArray(pointer, 8);
                }

                fixed (byte* pointer = _entry.c_rdevminor)
                {
                    minorBuffer = GetByteArrayFromFixedArray(pointer, 8);
                }
                _archiveEntry.rDev = Encoding.ASCII.GetString(majorBuffer) + Encoding.ASCII.GetString(minorBuffer);

                return true;
            }
        }
    }
}
