using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            using (CPIOLibSharp.CPIOFileStream sr = new CPIOLibSharp.CPIOFileStream("exampleOdc.cpio"))
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
