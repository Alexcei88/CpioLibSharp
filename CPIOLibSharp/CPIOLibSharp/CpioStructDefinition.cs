using System.Runtime.InteropServices;

namespace CPIOLibSharp
{
    /// <summary>
    /// Description of structs from format
    /// http://people.freebsd.org/~kientzle/libarchive/man/cpio.5.txt
    /// </summary>

    public class CpioStructDefinition
    {
        /// <summary>
        /// Name of last archive entry
        /// </summary>
        public static string LAST_ARCHIVEENTRY_FILENAME = "TRAILER!!!";

        /// <summary>
        /// Old Binary struct archive entry
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public unsafe struct header_old_cpio
        {
            public ushort c_magic;
            public ushort c_dev;
            public ushort c_ino;
            public ushort c_mode;
            public ushort c_uid;
            public ushort c_gid;
            public ushort c_nlink;
            public ushort c_rdev;
            public fixed ushort c_mtime[2];
            public ushort c_namesize;
            public fixed ushort c_filesize[2];
        };

        /// <summary>
        /// Portable ASCII struct archive entry(ODC)
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public unsafe struct cpio_odc_header
        {
            public fixed byte c_magic[6];
            public fixed byte c_dev[6];
            public fixed byte c_ino[6];
            public fixed byte c_mode[6];
            public fixed byte c_uid[6];
            public fixed byte c_gid[6];
            public fixed byte c_nlink[6];
            public fixed byte c_rdev[6];
            public fixed byte c_mtime[11];
            public fixed byte c_namesize[6];
            public fixed byte c_filesize[11];
        };

        /// <summary>
        /// New ASCII struct archive entry
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public unsafe struct cpio_newc_header
        {
            public fixed byte c_magic[6];
            public fixed byte c_ino[8];
            public fixed byte c_mode[8];
            public fixed byte c_uid[8];
            public fixed byte c_gid[8];
            public fixed byte c_nlink[8];
            public fixed byte c_mtime[8];
            public fixed byte c_filesize[8];
            public fixed byte c_devmajor[8];
            public fixed byte c_devminor[8];
            public fixed byte c_rdevmajor[8];
            public fixed byte c_rdevminor[8];
            public fixed byte c_namesize[8];
            public fixed byte c_check[8];
        };
    }
}