﻿using CPIOLibSharp.Formats;
using System.IO;
using System.Linq;
using static CPIOLibSharp.ArchiveFormat;

namespace CPIOLibSharp
{
    /// <summary>
    /// A file stream in cpio format(main class in library)
    /// </summary>
    public class CPIOFileStream
        : FileStream
    {
        /// <summary>
        /// A format of cpio file
        /// </summary>
        public CpioFormats Format
        {
            get
            {
                return _currentCpioFileStream.Format;
            }
        }

        /// <summary>
        /// the array of supporting formats
        /// </summary>
        private ICPIOFormat[] _cpioFileStream;

        /// <summary>
        /// the current format
        /// </summary>
        private ICPIOFormat _currentCpioFileStream;

        public CPIOFileStream(string fileName)
            : base(fileName, FileMode.Open)
        {
            _cpioFileStream = new ICPIOFormat[]
            {
                 new CRCFormat(this)
                ,new NewASCIIFormat(this)
                ,new ODCFormat(this)
                ,new BinaryFormat(this)
            };
            _currentCpioFileStream = _cpioFileStream.FirstOrDefault(g => g.DetectFormat());
            if (_currentCpioFileStream == null)
            {
                throw new InvalidDataException(string.Format("File {0} has invalid format", fileName));
            }
        }

        /// <summary>
        /// Extract archive to disk
        /// </summary>
        /// <param name="destFolder">Destinition folder</param>
        /// <param name="flags">Optional flags for additional behaviour</param>
        /// <returns></returns>
        public bool Extract(string destFolder, ExtractFlags[] flags = null)
        {
            return _currentCpioFileStream.Save(destFolder, flags);
        }
    }
}