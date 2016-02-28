using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPIOLibSharp
{
    /// <summary>
    /// Definition types of archive
    /// </summary>
    public static class ArchiveTypes
    {
        /// <summary>
        /// The "flags" argument selects optional behavior, 'OR' the flags you want.
        /// </summary>
        public enum ExtractArchiveFlags
        {
            /* Default: Do not try to set owner/group. */
            //ARCHIVE_EXTRACT_OWNER			=0x0001,
            /* Default: Do obey umask, do not restore SUID/SGID/SVTX bits. */
            //ARCHIVE_EXTRACT_PERM			=0x0002,
            /* Default: Do not restore mtime/atime. */
            ARCHIVE_EXTRACT_TIME			=0x0004,
            /* Default: Replace existing files. */
            //ARCHIVE_EXTRACT_NO_OVERWRITE 		=0x0008,
            /* Default: Try create first, unlink only if create fails with EEXIST. */
            //ARCHIVE_EXTRACT_UNLINK			=0x0010,
            /* Default: Do not restore ACLs. */
            //ARCHIVE_EXTRACT_ACL			=0x0020,
            /* Default: Do not restore fflags. */
            //ARCHIVE_EXTRACT_FFLAGS			=0x0040,
            /* Default: Do not restore xattrs. */
            //ARCHIVE_EXTRACT_XATTR 			=0x0080,
            /* Default: Do not try to guard against extracts redirected by symlinks. */
            /* Note: With ARCHIVE_EXTRACT_UNLINK, will remove any intermediate symlink. */
            //ARCHIVE_EXTRACT_SECURE_SYMLINKS		=0x0100,
            /* Default: Do not reject entries with '..' as path elements. */
            //ARCHIVE_EXTRACT_SECURE_NODOTDOT		=0x0200,
            /* Default: Create parent directories as needed. */
            //ARCHIVE_EXTRACT_NO_AUTODIR		=0x0400,
            /* Default: Overwrite files, even if one on disk is newer. */
            //ARCHIVE_EXTRACT_NO_OVERWRITE_NEWER	=0x0800,
            /* Detect blocks of 0 and write holes instead. */
            //ARCHIVE_EXTRACT_SPARSE			=0x1000,
            /* Default: Do not restore Mac extended metadata. */
            /* This has no effect except on Mac OS. */
            //ARCHIVE_EXTRACT_MAC_METADATA		=0x2000,
            /* Default: Use HFS+ compression if it was compressed. */
            /* This has no effect except on Mac OS v10.6 or later. */
            //ARCHIVE_EXTRACT_NO_HFS_COMPRESSION	=0x4000,
            /* Default: Do not use HFS+ compression if it was not compressed. */
            /* This has no effect except on Mac OS v10.6 or later. */
            //ARCHIVE_EXTRACT_HFS_COMPRESSION_FORCED	=0x8000,
            /* Default: Do not reject entries with absolute paths */
            //ARCHIVE_EXTRACT_SECURE_NOABSOLUTEPATHS =0x10000,
            /* Default: Do not clear no-change flags when unlinking object */
            //ARCHIVE_EXTRACT_CLEAR_NOCHANGE_FFLAGS	=0x20000,
        }
    }
}
