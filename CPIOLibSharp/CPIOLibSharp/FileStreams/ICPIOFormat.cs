using static CPIOLibSharp.ArchiveTypes;

namespace CPIOLibSharp.FileStreams
{
    internal interface ICPIOFormat
    {
        /// <summary>
        /// CPIO format of file
        /// </summary>
        CpioFormats Format { get; }

        bool DetectFormat();

        /// <summary>
        /// Save archive to disk
        /// </summary>
        /// <param name="desFolder"></param>
        /// <returns></returns>
        bool Save(string desFolder, ArchiveTypes.ExtractArchiveFlags[] flags = null);
    }
}