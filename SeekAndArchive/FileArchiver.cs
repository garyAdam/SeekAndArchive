using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace SeekAndArchive
{
    class FileArchiver
    {
        private DirectoryInfo archiveDirectory;
        public FileArchiver(string archiveDirectory)
        {
            this.ArchiveDirectory = new DirectoryInfo(archiveDirectory);
            checkDirectory(this.archiveDirectory);
        }

        public DirectoryInfo ArchiveDirectory { get => archiveDirectory; set => archiveDirectory = value; }

        public void ArchiveFile(FileInfo file)
        {

            if (file.Extension!=".gz")
            {
                DirectoryInfo specDirectory = new DirectoryInfo(Path.Combine(archiveDirectory.FullName, file.Name));
                checkDirectory(specDirectory);
                Compress(file, specDirectory);
            }
        }
        private void Compress(FileInfo fileToCompress, DirectoryInfo destinationDirectory)
        {
            try
            {
                using (FileStream originalFileStream = fileToCompress.OpenRead())
                {
                    if ((File.GetAttributes(fileToCompress.FullName) &
                       FileAttributes.Hidden) != FileAttributes.Hidden)
                    {
                        using (FileStream compressedFileStream = File.Create(Path.Combine(destinationDirectory.FullName, fileToCompress.Name + "_" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".gz")))
                        {
                            using (GZipStream compressionStream = new GZipStream(compressedFileStream,
                               CompressionMode.Compress))
                            {
                                originalFileStream.CopyTo(compressionStream);

                            }
                        }
                    }

                }
            }
            catch
            {

            }
        }
        private void checkDirectory(DirectoryInfo directory)
        {
            if (!directory.Exists)
            {
                directory.Create();
            }
        }
    }
}
