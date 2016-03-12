using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPIOLibSharp.ArchiveEntry;
using CPIOLibSharp.ArchiveEntry.ReaderFromDisk;
using System.IO;

namespace CPIOLibSharp.FileStreams
{
    class ODCFormat
        : AbstractCPIOFormat
    {
        public static byte[] MAGIC_ARCHIVEENTRY_NUMBER = { (byte)'0', (byte)'7', (byte)'0', (byte)'7', (byte)'0', (byte)'7' };

        /// <summary>
        /// Имя входного файла
        /// </summary>
        private readonly string _fileName;

        public ODCFormat(FileStream stream, string fileName)
            : base(stream)
        {
            _fileName = fileName;
        }

        public override bool DetectFormat()
        {
            _fileStream.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[MAGIC_ARCHIVEENTRY_NUMBER.Length];
            _fileStream.Read(buffer, 0, MAGIC_ARCHIVEENTRY_NUMBER.Length);
            return ByteArrayCompare(buffer, MAGIC_ARCHIVEENTRY_NUMBER);
        }

        public override IReaderCPIOArchiveEntry GetArchiveEntry(ArchiveTypes.ExtractArchiveFlags[] flags)
        {
            return new ODCReaderArchiveEntry(GetUintFromExtractArchiveFlags(flags));
        }

        protected override bool SkipExtractEntry(IReaderCPIOArchiveEntry entry)
        {
            return entry.FileName.Equals(_fileName);
        }
    }
}
