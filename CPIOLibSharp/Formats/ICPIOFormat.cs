using static CPIOLibSharp.ArchiveFormat;

namespace CPIOLibSharp.Formats
{
    /// <summary>
    /// interface for decompessor of data from file to disk for different formats
    /// </summary>
    internal interface ICPIOFormat
    {
        /// <summary>
        /// CPIO format
        /// </summary>
        CpioFormats Format { get; }

        /// <summary>
        /// do input stream has this format
        /// </summary>
        /// <returns></returns>
        bool DetectFormat();

        /// <summary>
        /// extract files from archive to the disk
        /// </summary>
        /// <param name="desFolder"></param>
        /// <returns></returns>
        bool Extract(string desFolder, CpioExtractFlags[] flags = null);
    }
}