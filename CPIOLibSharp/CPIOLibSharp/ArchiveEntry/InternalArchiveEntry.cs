using CPIOLibSharp.ArchiveEntry.WriterToDisk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPIOLibSharp.ArchiveEntry
{
    /// <summary>
    /// Данные архива
    /// </summary>
    class InternalArchiveEntry
    {
        public string Dev { get; set; }

        public string Ino { get; set; }

        public ArchiveEntryType Type { get; set; }

        public int Permission { get; set; }

        public string Uid { get; set; }

        public string Gid { get; set; }

        public long nLink { get; set; }

        public string rDev { get; set; }

        public DateTime mTime { get; set; }

        public byte[] FileName { get; set; }

        public byte[] Data { get; set; }

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

        public static IWriterEntry GetWriter(ArchiveEntryType type)
        {
            switch(type)
            {
                case ArchiveEntryType.DIRECTORY:
                    return new DirectoryWriterEntry();

                case ArchiveEntryType.FILE:
                    return new FileWriterEntry();
                default:
                    throw new Exception("Нет класса, реализующего запись для данного типа");
            }
        }

    }
}
