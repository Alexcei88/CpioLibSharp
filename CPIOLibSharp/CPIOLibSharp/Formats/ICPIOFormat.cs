using static CPIOLibSharp.ArchiveFormat;

namespace CPIOLibSharp.Formats
{
    internal interface ICPIOFormat
    {
        /// <summary>
        /// Current CPIO format
        /// </summary>
        CpioFormats Format { get; }

        /// <summary>
        /// compare format
        /// </summary>
        /// <returns></returns>
        bool DetectFormat();

        /// <summary>
        /// Save archive to disk
        /// </summary>
        /// <param name="desFolder"></param>
        /// <returns></returns>
        bool Save(string desFolder, ExtractFlags[] flags = null);
    }
}