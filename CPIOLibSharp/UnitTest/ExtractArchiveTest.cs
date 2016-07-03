using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using CPIOLibSharp;
using System.Linq;

namespace UnitTest
{
    [TestClass]
    public class ExtractArchiveTest
    {
        /// <summary>
        /// destinition folder where will extract files from archive
        /// </summary>
        private string _destFolder;

        [TestInitialize]
        public void Initialize()
        {
            _destFolder = GetTemporaryDir();
        }

        [TestCleanup]
        public void Cleanup()
        {
            Directory.Delete(_destFolder, true);
        }

        [TestMethod]
        public void TestBinFormat()
        {
            using (var stream = new CPIOFileStream("exampleBin.cpio"))
            {
                string destBinFolder = Path.Combine(_destFolder, Path.GetRandomFileName());
                stream.Extract(destBinFolder);
                CheckFiles(destBinFolder);

            }
        }

        private string GetTemporaryDir()
        {
            string tempFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            return tempFolder;

        }

        private void CheckFiles(string scanDir)
        {
            Assert.AreEqual(Directory.EnumerateFiles(scanDir).Count(), 6, "В выходной папке не совпадает количество файлов, упакованных в архив");
            //check file name and it size
            Action<string> checkFile = (fileName) =>
            {
                var file = Directory.EnumerateFiles(scanDir).FirstOrDefault(g => Path.GetFileName(g) == fileName);
                Assert.IsFalse(file == null, $"Файл {0} не найден");

            };
            checkFile("FindCxxTest.cmake");
            checkFile("FindCygwin.cmake");
            checkFile("FindDart.cmake");
            checkFile("FindHardLink.cmake");
            checkFile("FindSymbolickLink.cmake");
            checkFile("exampleOdc.cpio");

            //foreach(var file in Directory.EnumerateFiles(scanDir))
            //{
            //    f
            //}

        }
    }
}
