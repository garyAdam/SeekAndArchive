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
        private Dictionary<FileSystemEventArgs, Action> handlers = new Dictionary<FileSystemEventArgs, Action>();


        public FileArchiver Archiver { get => archiver; set => archiver = value; }

        public LiveWatcher(IList<FileInfo> files, FileArchiver archiver)
        {
            this.files = files;
            this.Archiver = archiver;
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public void Run(string path, string filter)
        {
            listFiles();
            setupWatcher(path, filter);
            while (Console.ReadLine() != "q")
            {

            }
        }
        private void setupWatcher(string path, string filter)
        {
            fileSystemWatcher.Path = path;
            fileSystemWatcher.IncludeSubdirectories = true;
            fileSystemWatcher.Filter = "*" + filter + "*";
            fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.Attributes | NotifyFilters.LastAccess | NotifyFilters.Security | NotifyFilters.Size;
            fileSystemWatcher.Created += OnCreate;
            fileSystemWatcher.Changed += (object sender, FileSystemEventArgs e) => Archiver.ArchiveFile(new FileInfo(e.FullPath));
            fileSystemWatcher.Deleted += OnDelete;
            fileSystemWatcher.Renamed += OnRename;
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

        private void OnRename(object sender, RenamedEventArgs e)
        {
            Archiver.ArchiveFile(new FileInfo(e.FullPath));
            files.Add(new FileInfo(e.FullPath));
            foreach (FileInfo item in files)
            {
                if (item.FullName == e.OldFullPath)
                {
                    files.Remove(item);
                    break;
                }
            }
            listFiles();
        }


        private void OnDelete(object sender, FileSystemEventArgs e)
        {
            FileInfo file = new FileInfo(e.FullPath);



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

        private void OnCreate(object sender, FileSystemEventArgs e)
        {
            FileInfo file = new FileInfo(e.FullPath);

            Archiver.ArchiveFile(file);
            files.Add(file);
            listFiles();
        }
    }
}