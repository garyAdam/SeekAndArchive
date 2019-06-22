

using System.Collections.Generic;
using System.IO;

namespace SeekAndArchive
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            FileSearcher fileSearcher = new FileSearcher();
            IList<FileInfo> files = fileSearcher.Search(args[0], args[1]);

            FileArchiver archiver = new FileArchiver(Path.Combine(args[1], "Archive"));
            LiveWatcher liveWatcher = new LiveWatcher(files,archiver);
            liveWatcher.Run(args[1]);


        }






    }
}
