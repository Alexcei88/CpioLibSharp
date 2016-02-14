using System;
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
                byte[] buffer = new byte[8];
                unsafe
                {
                    fixed (byte* ptr = _entry.c_filesize)
                    {
                        int i = 0;
                        for (byte* d = ptr; i < 8; ++i, ++d)
                        {
                            buffer[i] = *d;
                        }
                    }
                }
                string dataSize = Encoding.ASCII.GetString(buffer);
                int size = int.Parse(dataSize, System.Globalization.NumberStyles.HexNumber);
                return size % 4 == 0 ? size : (size + 4) / 4 * 4;
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
                byte[] buffer = new byte[8];
                unsafe
                {
                    fixed (byte* ptr = _entry.c_namesize)
                    {
                        int i = 0;
                        for (byte* d = ptr; i < 8; ++i, ++d)
                        {
                            buffer[i] = *d;
                        }
                    }
                }
                string fileNameSize = Encoding.ASCII.GetString(buffer);
                int size = int.Parse(fileNameSize, System.Globalization.NumberStyles.HexNumber);
                int commonSize = size + EntrySize;
                return commonSize % 4 == 0 ? size : (4 - commonSize % 4) + size;
            }
        }

        public override bool FillEntry(byte[] data)
        {
            IntPtr @in = Marshal.AllocHGlobal(EntrySize);
            Marshal.Copy(data, 0, @in, EntrySize);
            _entry = (CpioStruct.cpio_newc_header)Marshal.PtrToStructure(@in, _entry.GetType());
            Marshal.FreeHGlobal(@in);
            return true;
        }
    }
}
