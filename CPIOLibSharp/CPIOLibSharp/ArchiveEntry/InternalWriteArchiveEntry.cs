using CPIOLibSharp.ArchiveEntry.WriterToDisk;
using CPIOLibSharp.FileStreams;
using System;
using System.Linq;
using System.Text;

namespace CPIOLibSharp.ArchiveEntry
{
    /// <summary>
    /// Данные архива
    /// </summary>
    internal class InternalWriteArchiveEntry
    {
        public string Dev { get; set; }

        public string INode { get; set; }

        public ArchiveEntryType ArchiveType { get; set; }

        public int Permission { get; set; }

        public int Uid { get; set; }

        public int Gid { get; set; }

        public long nLink { get; set; }

        public int rDev { get; set; }

        public DateTime mTime { get; set; }

        public byte[] FileName { get; set; }

        public byte[] Data { get; set; }

        /// <summary>
        /// Option for extract entry to disk
        /// </summary>
        public uint ExtractFlags { get; set; }

        /// <summary>
        /// Flag is was extracted entry to disk
        /// </summary>
        public bool IsExtractToDisk { get; set; }

        /// <summary>
        /// hardLink to entry of archive
        /// </summary>
        public InternalWriteArchiveEntry LinkEntry { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder("ArchiveEntry:");
            builder.AppendLine(string.Format("FileName: {0}", GetFileName(this.FileName)));
            builder.AppendLine(string.Format("Type {0}", this.ArchiveType));
            builder.AppendLine(string.Format("Permission: {0}", this.Permission));
            return builder.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is InternalWriteArchiveEntry))
            {
                return false;
            }

            InternalWriteArchiveEntry entry = obj as InternalWriteArchiveEntry;
            return AbstractCPIOFormat.ByteArrayCompare(entry.FileName, FileName)
                && entry.INode == entry.INode;
        }

        public override int GetHashCode()
        {
            return FileName.GetHashCode();
        }

        /// <summary>
        /// Получение имени файла из переданного массива байт
        /// </summary>
        /// <param name="fileName">входной массив байт</param>
        /// <returns></returns>
        public static string GetFileName(byte[] fileName)
        {
            StringBuilder retFileName = new StringBuilder();
            int i = 0;
            while (i < fileName.Length && fileName[i] != '\0')
            {
                retFileName.Append((char)fileName[i++]);
            }
            string name = retFileName.ToString();
            return name.Replace('/', '\\');
        }

        public static string GetTargetFileToLink(byte[] data)
        {
            StringBuilder retFileName = new StringBuilder();
            int i = 0;
            while (i < data.Length && data[i] != '\0')
            {
                retFileName.Append((char)data[i++]);
            }
            string name = retFileName.ToString();
            return name.Replace('/', '\\');
        }

        /// <summary>
        /// Получение типа раздела архива по полю mode
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ArchiveEntryType GetArchiveEntryType(long mode)
        {
            long type = mode >> 9;
            return Enum.GetValues(typeof(ArchiveEntryType)).Cast<ArchiveEntryType>().First(g => (int)g == type);
        }

        /// <summary>
        /// Получение прав раздела по поле mode
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static int GePermission(long mode)
        {
            return (int)mode & 0x1ff;
        }

        public static IWriterEntry GetWriter(InternalWriteArchiveEntry _entry)
        {
            switch (_entry.ArchiveType)
            {
                case ArchiveEntryType.DIRECTORY:
                    return new DirectoryWriterEntry();

                case ArchiveEntryType.FILE:
                    if (_entry.nLink > 1)
                    {
                        return new HardLinkFileWriterEntry();
                    }
                    else
                    {
                        return new FileWriterEntry();
                    }
                case ArchiveEntryType.SYMBOLIC_LINK:
                    return new SymbolicLinkFileWriterEntry();

                default:
                    throw new Exception("Нет класса, реализующего запись для данного типа");
            }
        }
    }
}