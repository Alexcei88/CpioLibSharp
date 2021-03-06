﻿using CPIOLibSharp.Formats;
using CPIOLibSharp.Helper;
using System;
using System.Runtime.InteropServices;

namespace CPIOLibSharp.ArchiveEntry.ReaderFromDisk
{
    /// <summary>
    /// Reader for binary format
    /// </summary>
    internal class BinaryReadableArchiveEntry
        : AbstractReadableArchiveEntry
    {
        private CpioStructDefinition.header_old_cpio _entry = new CpioStructDefinition.header_old_cpio();

        public BinaryReadableArchiveEntry(uint flags)
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
                        byte[] array = GetByteArrayFromFixedArray(pointer, 2);
                        // +1 - NULL byte of end
                        return (ulong)BitConverter.ToInt32(array, 0) + 1;
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
                return _entry.c_namesize % 2 == 0 ? (ulong)_entry.c_namesize : (ulong)_entry.c_namesize + 1;
            }
        }

        public override bool HasData
        {
            get
            {
                unsafe
                {
                    fixed (ushort* pointer = _entry.c_filesize)
                    {
                        byte[] array = GetByteArrayFromFixedArray(pointer, 2);
                        return (ulong)BitConverter.ToInt32(array, 0) != 0;
                    }
                }
            }
        }

        public override bool FillEntry(byte[] data)
        {
            IntPtr @in = Marshal.AllocHGlobal(EntrySize);
            Marshal.Copy(data, 0, @in, EntrySize);
            _entry = (CpioStructDefinition.header_old_cpio)Marshal.PtrToStructure(@in, _entry.GetType());
            Marshal.FreeHGlobal(@in);

            // check magic
            if (_entry.c_magic != BinaryFormat.MAGIC_ARCHIVEENTRY_NUMBER)
            {
                return false;
            }
            return true;
        }

        public override void ReadMetadataEntry()
        {
            _archiveEntry.Dev = _entry.c_dev.ToString();
            _archiveEntry.INode = _entry.c_ino.ToString();

            // Type, Permission
            long mode = _entry.c_mode;
            _archiveEntry.ArchiveType = InternalWriteArchiveEntry.GetArchiveEntryType(mode);
            _archiveEntry.Permission = InternalWriteArchiveEntry.GetPermission(mode);

            _archiveEntry.Uid = _entry.c_uid;
            _archiveEntry.Gid = _entry.c_gid;

            unsafe
            {
                fixed (ushort* pointer = _entry.c_mtime)
                {
                    byte[] array = GetByteArrayFromFixedArray(pointer, 2);
                    _archiveEntry.mTime = ((long)BitConverter.ToInt32(array, 0)).ToUnixTime();
                }
            }
            _archiveEntry.nLink = _entry.c_nlink;
            // rDev
            _archiveEntry.rDev = _entry.c_rdev;
            _archiveEntry.ExtractFlags = _extractFlags;
        }
    }
}