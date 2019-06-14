using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;

namespace SeekAndArchive
{
    public class LiveWatcher
    {
        private IList<FileInfo> files;
        private readonly FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();
        private FileArchiver archiver;


        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public void Run(string[] args)
        {
            FileSearcher fileSearcher = new FileSearcher();
            archiver = new FileArchiver(Path.Combine(args[1], "Archive"));
            files = fileSearcher.Search(args[0], args[1]);
            listFiles();
            setupWatcher(args);
            while (Console.ReadLine() != "q")
            {

            }
        }
        private void setupWatcher(string[] args)
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

        private void listFiles()
        {
            Console.Clear();
            IEnumerable<string> result = from f in files
                                         select f.FullName;
            foreach (string item in result)
            {
                Console.WriteLine(item);
            }
        }

        private void FileSystemWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            archiver.ArchiveFile(new FileInfo(e.FullPath));
            foreach (FileInfo item in files)
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



        private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            FileInfo file = new FileInfo(e.FullPath);


            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                archiver.ArchiveFile(file);
                files.Add(file);
                listFiles();
            }
            else if (e.ChangeType == WatcherChangeTypes.Deleted)
            {

                foreach (FileInfo item in files)
                {
                    if (item.FullName == e.FullPath)
                    {
                        files.Remove(item);
                        break;
                    }
                }
                listFiles();
            }
            else if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                archiver.ArchiveFile(file);
                listFiles();
            }
        }
    }
}