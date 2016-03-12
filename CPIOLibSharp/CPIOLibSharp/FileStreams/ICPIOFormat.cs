namespace CPIOLibSharp.FileStreams
{
    internal interface ICPIOFormat
    {
        bool DetectFormat();

        /// <summary>
        /// Save archive to disk
        /// </summary>
        /// <param name="desFolder"></param>
        /// <returns></returns>
        bool Save(string desFolder, ArchiveTypes.ExtractArchiveFlags[] flags = null);
    }
}