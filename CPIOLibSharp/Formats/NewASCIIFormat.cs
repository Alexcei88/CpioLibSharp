using CPIOLibSharp.ArchiveEntry;
using System.IO;

namespace CPIOLibSharp.Formats
{
    /// <summary>
    /// decompessor of data from file to disk in new ASCII Format
    /// </summary>
    internal class NewASCIIFormat
        : AbstractCPIOFormat
    {
        public static byte[] MAGIC_ARCHIVEENTRY_NUMBER = { (byte)'0', (byte)'7', (byte)'0', (byte)'7', (byte)'0', (byte)'1' };

        public NewASCIIFormat(FileStream stream)
            : base(stream)
        {
            _format = ArchiveFormat.CpioFormats.NewASCII;
        }

        public override bool DetectFormat()
        {
            _fileStream.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[MAGIC_ARCHIVEENTRY_NUMBER.Length];
            _fileStream.Read(buffer, 0, MAGIC_ARCHIVEENTRY_NUMBER.Length);
            return ByteArrayCompare(buffer, MAGIC_ARCHIVEENTRY_NUMBER);
        }

        public override IReadableCPIOArchiveEntry CreateReadableArchiveEntry(CpioExtractFlags[] flags)
        {
            return new NewASCIIReadableArchiveEntry(ExtractArchiveFlagToUInt(flags));
        }
    }
}