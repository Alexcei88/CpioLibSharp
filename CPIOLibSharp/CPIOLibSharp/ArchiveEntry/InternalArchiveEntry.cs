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

        public string Permission { get; set; }

        public string Uid { get; set; }

        public string Gid { get; set; }

        public int nLink { get; set; }

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
            return retFileName.ToString();
        }

        /// <summary>
        /// Получение типа архива по значению
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ArchiveEntryType GetArchiveEntryType(int value)
        {
            return Enum.GetValues(typeof(ArchiveEntryType)).Cast<ArchiveEntryType>().First(g => (int)g == value);
        }
    }
}
