using CPIOLibSharp;

namespace TestProject
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (CPIOLibSharp.CPIOFileStream sr = new CPIOLibSharp.CPIOFileStream("exampleCrc.cpio"))
            {
                sr.Extract(@"F:\", new CPIOLibSharp.ExtractFlags[]
                {
                   ExtractFlags.ARCHIVE_EXTRACT_TIME
                });
            }
        }
    }
}