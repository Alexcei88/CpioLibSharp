using CPIOLibSharp.FileStreams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CPIOLibSharp
{
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
            };
            _currentCpioFormats = _cpioFormats.FirstOrDefault(g => g.DetectFormat());
            if(_currentCpioFormats == null)
            {
                throw new InvalidDataException(string.Format("File {0} has i format of cpio", fileName));
            }
        }

        /// <summary>
        /// Save archive to disk
        /// </summary>
        /// <param name="destFolder"></param>
        /// <returns></returns>
        public bool Save(string destFolder, ArchiveTypes.ExtractArchiveFlags flags = 0)
        {
            return _currentCpioFormats.Save(destFolder, flags);
        }
    }
}
