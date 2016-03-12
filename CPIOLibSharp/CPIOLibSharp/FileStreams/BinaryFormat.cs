using CPIOLibSharp.ArchiveEntry;
using CPIOLibSharp.ArchiveEntry.ReaderFromDisk;
using System.IO;

namespace CPIOLibSharp.FileStreams
{
    internal class BinaryFormat
        : AbstractCPIOFormat
    {
        public static byte[] MAGIC_ARCHIVEENTRY_NUMBER = { (byte)'0', (byte)'7', (byte)'0', (byte)'7', (byte)'0', (byte)'7' };

        public BinaryFormat(FileStream stream)
            : base(stream)
        { }

        public override bool DetectFormat()
        {
            _fileStream.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[MAGIC_ARCHIVEENTRY_NUMBER.Length];
            _fileStream.Read(buffer, 0, MAGIC_ARCHIVEENTRY_NUMBER.Length);
            return ByteArrayCompare(buffer, MAGIC_ARCHIVEENTRY_NUMBER);
        }

        public override IReaderCPIOArchiveEntry GetArchiveEntry(ArchiveTypes.ExtractArchiveFlags[] flags)
        {
            return new BinaryReaderArchiveEntry(GetUintFromExtractArchiveFlags(flags));
        }

        protected override bool SkipExtractEntry(IReaderCPIOArchiveEntry entry)
        {
            return false;
        }
    }
}