using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;

namespace SeekAndArchive
{
    class Program
    {
        private static IList<FileInfo> files;
        private static FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();

        static void Main(string[] args)
        {

            Run(args);


        }
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        private static void Run(string[] args)
        {
            FileSearcher fileSearcher = new FileSearcher();
            files = fileSearcher.Search(args[0], args[1]);
            listFiles();
            setupWatcher(args);
            while (Console.ReadLine() != "q")
            {

            }
        }

        private static void setupWatcher(string[] args)
        {
            fileSystemWatcher.Path = args[1];
            fileSystemWatcher.IncludeSubdirectories = true;
            fileSystemWatcher.Filter = "*" + args[0] + "*";
            fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.Attributes | NotifyFilters.LastAccess | NotifyFilters.Security | NotifyFilters.Size;
            fileSystemWatcher.Changed += FileSystemWatcher_Changed;
            fileSystemWatcher.Created += FileSystemWatcher_Changed;
            fileSystemWatcher.Deleted += FileSystemWatcher_Changed;
            fileSystemWatcher.Renamed += FileSystemWatcher_Renamed;

            fileSystemWatcher.EnableRaisingEvents = true;
        }

        private static void listFiles()
        {
            Console.Clear();
            IEnumerable<string> result = from f in files
                                         select f.FullName;
            foreach (var item in result)
            {
                Console.WriteLine(item);
            }
        }

        private static void FileSystemWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            foreach (var item in files)
            {
                if (item.FullName == e.OldFullPath)
                {
                    files.Remove(item);
                    break;
                }
            }
            files.Add(new FileInfo(e.FullPath));
            listFiles();
        }

       

        private static void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                files.Add(new FileInfo(e.FullPath));
                listFiles();
            }
            else if (e.ChangeType == WatcherChangeTypes.Deleted)
            {

                foreach (var item in files)
                {
                    if (item.FullName==e.FullPath)
                    {
                        files.Remove(item);
                        break;
                    }
                }
                listFiles();
            }
            else if (e.ChangeType == WatcherChangeTypes.Changed)
            {

                listFiles();
            }
        }
    }
}
