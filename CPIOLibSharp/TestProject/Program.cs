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
            using (CPIOLibSharp.CPIOFileStream sr = new CPIOLibSharp.CPIOFileStream("cpiofromrpm.cpio"))
            {
                sr.Save("test");    
            }
        }
    }
}
