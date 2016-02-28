using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPIOLibSharp.FileStreams
{
    interface ICPIOFormat
    {
        bool DetectFormat();

        /// <summary>
        /// Save archive to disk
        /// </summary>
        /// <param name="desFolder"></param>
        /// <returns></returns>
        bool Save(string desFolder, ArchiveTypes.ExtractArchiveFlags flags = 0);
    }
}
