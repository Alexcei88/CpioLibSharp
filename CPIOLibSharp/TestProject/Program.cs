namespace TestProject
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (CPIOLibSharp.CPIOFileStream sr = new CPIOLibSharp.CPIOFileStream("exampleCrc.cpio"))
            {
                sr.Extract(@"F:\", new CPIOLibSharp.ArchiveTypes.ExtractArchiveFlags[]
                {
                   CPIOLibSharp.ArchiveTypes.ExtractArchiveFlags.ARCHIVE_EXTRACT_TIME
                });
            }
        }
    }
}