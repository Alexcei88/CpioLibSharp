﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPIOLibSharp.ArchiveEntry;
using System.IO;

namespace CPIOLibSharp.FileStreams
{
    internal class CRCFormat
        : AbstractCPIOFormat
    {

        public readonly byte[] MAGIC_ARCHIVEENTRY_NUMBER = { (byte)'0', (byte)'7', (byte)'0', (byte)'7', (byte)'0', (byte)'2' };

        public CRCFormat(FileStream stream)
            : base(stream)
        { }

        public override bool DetectFormat()
        {
            _fileStream.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[MAGIC_ARCHIVEENTRY_NUMBER.Length];
            _fileStream.Read(buffer, 0, MAGIC_ARCHIVEENTRY_NUMBER.Length);
            return ByteArrayCompare(buffer, MAGIC_ARCHIVEENTRY_NUMBER);
        }

        public override ICPIOArchiveEntry GetArchiveEntry()
        {
            return new CRCFormatArchiveEntry();
        }
    }
}