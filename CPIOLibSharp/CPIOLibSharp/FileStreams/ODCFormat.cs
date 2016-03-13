using CPIOLibSharp.ArchiveEntry;
using CPIOLibSharp.ArchiveEntry.ReaderFromDisk;
using System.IO;

namespace CPIOLibSharp.FileStreams
{
    internal class ODCFormat
        : AbstractCPIOFormat
    {
        public static byte[] MAGIC_ARCHIVEENTRY_NUMBER = { (byte)'0', (byte)'7', (byte)'0', (byte)'7', (byte)'0', (byte)'7' };

        public ODCFormat(FileStream stream)
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
            return new ODCReaderArchiveEntry(GetUintFromExtractArchiveFlags(flags));
        }
    }
}