using static CPIOLibSharp.ArchiveFormat;

namespace CPIOLibSharp.Formats
{
    /// <summary>
    /// interface for decompessor of data from file to disk for different formats
    /// </summary>
    internal interface ICPIOFormat
    {
        /// <summary>
        /// Current CPIO format
        /// </summary>
        CpioFormats Format { get; }

        /// <summary>
        /// do input stream has this format
        /// </summary>
        /// <returns></returns>
        bool DetectFormat();

        /// <summary>
        /// Save archive to disk
        /// </summary>
        /// <param name="desFolder"></param>
        /// <returns></returns>
        bool Save(string desFolder, CpioExtractFlags[] flags = null);
    }
}