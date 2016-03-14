using CPIOLibSharp.ArchiveEntry;
using CPIOLibSharp.ArchiveEntry.ReaderFromDisk;
using System;
using System.IO;
using System.Text;

namespace CPIOLibSharp.FileStreams
{
    internal class BinaryFormat
        : AbstractCPIOFormat
    {
        /// <summary>
        /// Магическое число архива(070707)
        /// </summary>
        public static short MAGIC_ARCHIVEENTRY_NUMBER = 29127; 

        public BinaryFormat(FileStream stream)
            : base(stream)
        {
            _format = ArchiveTypes.CpioFormats.Binary;
        }

        public override bool DetectFormat()
        {
            _fileStream.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[2];
            _fileStream.Read(buffer, 0, 2);

            short fileNumber = BitConverter.ToInt16(buffer, 0);
            return fileNumber == MAGIC_ARCHIVEENTRY_NUMBER;
        }

        public override IReaderCPIOArchiveEntry GetArchiveEntry(ArchiveTypes.ExtractArchiveFlags[] flags)
        {
            return new BinaryReaderArchiveEntry(GetUintFromExtractArchiveFlags(flags));
        }
    }
}