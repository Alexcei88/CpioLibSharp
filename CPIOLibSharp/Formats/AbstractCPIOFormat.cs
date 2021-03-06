﻿using CPIOLibSharp.ArchiveEntry;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static CPIOLibSharp.ArchiveFormat;

namespace CPIOLibSharp.Formats
{
    /// <summary>
    /// base class a decompessor of data from archive file to save to disk
    /// </summary>
    internal abstract class AbstractCPIOFormat
        : ICPIOFormat
    {
        /// <summary>
        /// Stream to archive file
        /// </summary>
        protected FileStream _fileStream;

        /// <summary>
        /// CPIO format
        /// </summary>
        protected CpioFormats _format;

        public CpioFormats Format
        {
            get
            {
                return _format;
            }
        }

        public AbstractCPIOFormat(FileStream stream)
        {
            _fileStream = stream;
        }

        /// <summary>
        /// Extract archive to disk
        /// </summary>
        /// <param name="destFolder"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public bool Extract(string destFolder, CpioExtractFlags[] flags = null)
        {
            if (!Directory.Exists(destFolder))
            {
                if(Directory.CreateDirectory(destFolder) == null)
                {
                    throw new Exception(string.Format("The destinition directory {0} can not be created", destFolder));
                }
            }

                bool findTrailer = false;
                _fileStream.Seek(0, SeekOrigin.Begin);

                // list all the archive entry
                List<IReadableCPIOArchiveEntry> archiveEntries = new List<IReadableCPIOArchiveEntry>();

                IReadableCPIOArchiveEntry archiveEntry = CreateReadableArchiveEntry(flags);
                int sizeBuffer = archiveEntry.EntrySize;
                byte[] buffer = new byte[sizeBuffer];

                while (_fileStream.Read(buffer, 0, sizeBuffer) == sizeBuffer)
                {
                    archiveEntry = CreateReadableArchiveEntry(flags);
                    archiveEntry.FillEntry(buffer);

                    ulong fileNameSize = archiveEntry.FileNameSize;
                    if (fileNameSize != 0)
                    {
                        byte[] fileName = new byte[fileNameSize];
                        _fileStream.Read(fileName, 0, (int)fileNameSize);
                        archiveEntry.FileName = fileName;
                    }
                    if (archiveEntry.IsLastArchiveEntry())
                    {
                        findTrailer = true;
                        break;
                    }

                    archiveEntry.ReadMetadataEntry();
                    if (archiveEntry.HasData)
                    {
                        ulong dataSize = archiveEntry.DataSize;
                        byte[] data = new byte[dataSize];
                        _fileStream.Read(data, 0, (int)dataSize);
                        archiveEntry.Data  = data;
                    }

                    // save entry to disk
                    if (archiveEntry.Writer.ExtractEntryToDisk(destFolder) == null)
                    {
                        Console.WriteLine("Fail to extract the archive entry: {0}", archiveEntry.ToString());
                        Directory.Delete(destFolder, true);
                        return false;
                    }
                    archiveEntries.Add(archiveEntry);
                }
                if (!findTrailer)
                {
                    Console.WriteLine("Not find the end entry in file. File is invalid format");
                    Directory.Delete(destFolder, true);
                    return false;
                }
                else
                {
                    return PostProcessingEntries(destFolder, archiveEntries);
                }
        }

        /// <summary>
        /// get reader of archive entry(fabric method)
        /// </summary>
        /// <returns></returns>
        public abstract IReadableCPIOArchiveEntry CreateReadableArchiveEntry(CpioExtractFlags[] flags);
        
        /// <summary>
        /// detect a CPIO format
        /// </summary>
        /// <returns></returns>
        public abstract bool DetectFormat();
               
        /// <summary>
        /// Post extract entry to disk(after read all entry from file)
        /// </summary>
        /// <param name="destFolder"></param>
        /// <param name="archiveEntries"></param>
        /// <returns></returns>
        private bool PostProcessingEntries(string destFolder, List<IReadableCPIOArchiveEntry> archiveEntries)
        {
            foreach (var entry in archiveEntries)
            {
                if (entry.Writer.PostExtractEntryToDisk(destFolder, archiveEntries) == null)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// transfrormation extract flags enum to uint
        /// </summary>
        /// <param name="flags"></param>
        /// <returns></returns>
        static protected uint ExtractArchiveFlagToUInt(CpioExtractFlags[] flags)
        {
            uint exFlags = 0;
            if (flags == null)
                return exFlags;

            foreach (CpioExtractFlags flag in flags)
            {
                exFlags = exFlags | (uint)flag;
            }
            return exFlags;
        }
    }
}