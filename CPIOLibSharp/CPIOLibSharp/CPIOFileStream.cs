﻿using CPIOLibSharp.FileStreams;
using System.IO;
using System.Linq;

namespace CPIOLibSharp
{
    /// <summary>
    /// A file stream in cpio format
    /// </summary>
    public class CPIOFileStream
        : FileStream
    {
        private ICPIOFormat[] _cpioFormats;

        private ICPIOFormat _currentCpioFormats;

        public CPIOFileStream(string fileName)
            : base(fileName, FileMode.Open)
        {
            _cpioFormats = new ICPIOFormat[]
            {
                 new CRCFormat(this)
                ,new NewASCIIFormat(this)
                ,new ODCFormat(this, fileName)
            };
            _currentCpioFormats = _cpioFormats.FirstOrDefault(g => g.DetectFormat());
            if (_currentCpioFormats == null)
            {
                throw new InvalidDataException(string.Format("File {0} has invalid format", fileName));
            }
        }

        /// <summary>
        /// Save archive to disk
        /// </summary>
        /// <param name="destFolder"></param>
        /// <returns></returns>
        public bool Save(string destFolder, ArchiveTypes.ExtractArchiveFlags[] flags = null)
        {
            return _currentCpioFormats.Save(destFolder, flags);
        }
    }
}