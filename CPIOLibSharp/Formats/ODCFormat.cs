using CPIOLibSharp.ArchiveEntry;
using CPIOLibSharp.ArchiveEntry.ReaderFromDisk;
using System.IO;

namespace CPIOLibSharp.Formats
{
    /// <summary>
    /// decompessor of data from file to disk in odc Format
    /// </summary>
    internal class ODCFormat
        : AbstractCPIOFormat
    {
        public static byte[] MAGIC_ARCHIVEENTRY_NUMBER = { (byte)'0', (byte)'7', (byte)'0', (byte)'7', (byte)'0', (byte)'7' };

        public ODCFormat(FileStream stream)
            : base(stream)
        {
            _format = ArchiveFormat.CpioFormats.ODC;
        }

        public override bool DetectFormat()
        {
            _fileStream.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[MAGIC_ARCHIVEENTRY_NUMBER.Length];
            _fileStream.Read(buffer, 0, MAGIC_ARCHIVEENTRY_NUMBER.Length);
            return InternalWriteArchiveEntry.ByteArrayCompare(buffer, MAGIC_ARCHIVEENTRY_NUMBER);
        }

        public override IReadableCPIOArchiveEntry CreateReadableArchiveEntry(CpioExtractFlags[] flags)
        {
            return new ODCReadableArchiveEntry(ExtractArchiveFlagToUInt(flags));
        }
    }
}