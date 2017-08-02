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

        //check file name and it size
        private Action<string, string> _checkFile = (fileName, scanDir) =>
        {
            var file = Directory.EnumerateFiles(scanDir).FirstOrDefault(g => Path.GetFileName(g) == fileName);
            Assert.IsFalse(file == null, $"Файл {0} не найден");

        };

        private Action<string, int> _checkSizeOfFile = (fileName, size) =>
        {
            FileInfo info = new FileInfo(Path.Combine(fileName));
            Assert.IsTrue(info.Length == size, $"Размер файла {0} не совпадает");
        };

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

        [TestMethod]
        public void TestCRCFormat()
        {
            using (var stream = new CPIOFileStream("exampleCrc.cpio"))
            {
                string destBinFolder = Path.Combine(_destFolder, Path.GetRandomFileName());
                stream.Extract(destBinFolder);
                CheckFiles(destBinFolder);

            }
        }

        [TestMethod]
        public void TestODCFormat()
        {
            using (var stream = new CPIOFileStream("exampleOdc.cpio"))
            {
                string destBinFolder = Path.Combine(_destFolder, Path.GetRandomFileName());
                stream.Extract(destBinFolder);
                CheckFiles(destBinFolder);

            }
        }

        [TestMethod]
        public void TestNewASCIIFormat()
        {
            using (var stream = new CPIOFileStream("example.cpio"))
            {
                string destBinFolder = Path.Combine(_destFolder, Path.GetRandomFileName());
                stream.Extract(destBinFolder);
                CheckNewASCIIFormatFiles(destBinFolder);

            }
        }



        private string GetTemporaryDir()
        {
            string tempFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            return tempFolder;

        }

        private void CheckNewASCIIFormatFiles(string scanDir)
        {
            Assert.AreEqual(Directory.EnumerateFiles(scanDir).Count(), 5, "В выходной папке не совпадает количество файлов, упакованных в архив");

            {
                _checkFile("FindCxxTest.cmake", scanDir);
                _checkFile("FindCygwin.cmake", scanDir);
                _checkFile("FindDart.cmake", scanDir);
                _checkFile("FindHardLink.cmake", scanDir);
                _checkFile("FindSymbolickLink.cmake", scanDir);
            }

            {
                _checkSizeOfFile(Path.Combine(scanDir, "FindCxxTest.cmake"), 8019);
                _checkSizeOfFile(Path.Combine(scanDir, "FindCygwin.cmake"), 955);
                _checkSizeOfFile(Path.Combine(scanDir, "FindDart.cmake"), 1385);
                _checkSizeOfFile(Path.Combine(scanDir, "FindHardLink.cmake"), 1385);
                _checkSizeOfFile(Path.Combine(scanDir, "FindSymbolickLink.cmake"), 0);
            }

        }

        private void CheckFiles(string scanDir)
        {
            Assert.AreEqual(Directory.EnumerateFiles(scanDir).Count(), 6, "В выходной папке не совпадает количество файлов, упакованных в архив");
            
            {
                _checkFile("FindCxxTest.cmake", scanDir);
                _checkFile("FindCygwin.cmake", scanDir);
                _checkFile("FindDart.cmake", scanDir);
                _checkFile("FindHardLink.cmake", scanDir);
                _checkFile("FindSymbolickLink.cmake", scanDir);
                _checkFile("exampleOdc.cpio", scanDir);
            }

            {
                _checkSizeOfFile(Path.Combine(scanDir, "FindCxxTest.cmake"), 8019);
                _checkSizeOfFile(Path.Combine(scanDir, "FindCygwin.cmake"), 955);
                _checkSizeOfFile(Path.Combine(scanDir, "FindDart.cmake"), 1385);
                _checkSizeOfFile(Path.Combine(scanDir, "FindHardLink.cmake"), 1385);
                _checkSizeOfFile(Path.Combine(scanDir, "FindSymbolickLink.cmake"), 0);
                _checkSizeOfFile(Path.Combine(scanDir, "exampleOdc.cpio"), 0);

            }
            
        }
    }
}
