﻿using CPIOLibSharp.ArchiveEntry;
using CPIOLibSharp.Helper;
using System.IO;

namespace CPIOLibSharp.Formats
{
    /// <summary>
    /// decompessor of data from file to disk in CRC Format
    /// </summary>
    internal class CRCFormat
        : AbstractCPIOFormat
    {
        public static byte[] MAGIC_ARCHIVEENTRY_NUMBER = { (byte)'0', (byte)'7', (byte)'0', (byte)'7', (byte)'0', (byte)'2' };

        public CRCFormat(FileStream stream)
            : base(stream)
        {
            _format = ArchiveFormat.CpioFormats.CRC;
        }

        public override bool DetectFormat()
        {
            _fileStream.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[MAGIC_ARCHIVEENTRY_NUMBER.Length];
            _fileStream.Read(buffer, 0, MAGIC_ARCHIVEENTRY_NUMBER.Length);
            return buffer.Compare(MAGIC_ARCHIVEENTRY_NUMBER);
        }

        public override IReadableCPIOArchiveEntry CreateReadableArchiveEntry(CpioExtractFlags[] flags)
        {
            return new CRCReadableArchiveEntry(ExtractArchiveFlagToUInt(flags));
        }
    }
}