namespace TestProject
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (CPIOLibSharp.CPIOFileStream sr = new CPIOLibSharp.CPIOFileStream("exampleCrc.cpio"))
            {
                sr.Save(@"F:\", new CPIOLibSharp.ArchiveTypes.ExtractArchiveFlags[]
                {
                   CPIOLibSharp.ArchiveTypes.ExtractArchiveFlags.ARCHIVE_EXTRACT_TIME
                  ,CPIOLibSharp.ArchiveTypes.ExtractArchiveFlags.ARCHIVE_EXTRACT_PERM
                });
            }
        }
    }
}