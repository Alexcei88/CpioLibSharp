using CPIOLibSharp.ArchiveEntry;
using System.IO;

namespace CPIOLibSharp.FileStreams
{
    /// <summary>
    /// New ASCII Format
    /// </summary>
    internal class NewASCIIFormat
        : AbstractCPIOFormat
    {
        public static byte[] MAGIC_ARCHIVEENTRY_NUMBER = { (byte)'0', (byte)'7', (byte)'0', (byte)'7', (byte)'0', (byte)'1' };

        public NewASCIIFormat(FileStream stream)
            : base(stream)
        {
            _format = ArchiveTypes.CpioFormats.NewASCII;
        }

        public override bool DetectFormat()
        {
            _fileStream.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[MAGIC_ARCHIVEENTRY_NUMBER.Length];
            _fileStream.Read(buffer, 0, MAGIC_ARCHIVEENTRY_NUMBER.Length);
            return ByteArrayCompare(buffer, MAGIC_ARCHIVEENTRY_NUMBER);
        }

        public override IReaderCPIOArchiveEntry GetArchiveEntry(ExtractFlags[] flags)
        {
            return new NewASCIIReaderFormatArchiveEntry(GetUintFromExtractArchiveFlags(flags));
        }
    }
}